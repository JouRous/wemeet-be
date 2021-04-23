using System;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Models;
using API.Types;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Controllers
{
  public class AuthController : BaseApiController
  {
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      RoleManager<AppRole> roleManager,
      ITokenService tokenService,
      IUnitOfWork unitOfWork,
      IMapper mapper,
      IWebHostEnvironment hostEnvironment
    )
    {
      _tokenService = tokenService;
      _mapper = mapper;
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
      _hostEnvironment = hostEnvironment;
      _unitOfWork = unitOfWork;
    }

    private async Task<bool> CheckUserExist(string email)
    {
      return await _userManager.Users.AnyAsync(user => user.Email == email);
    }

    [HttpPost("register")]
    public async Task<ActionResult<Response<AuthModel>>> Register([FromForm] RegisterModel registerModel)
    {
      if (await CheckUserExist(registerModel.Email))
      {
        return BadRequest("User already taken");
      }

      var transaction = await DbContext.Database.BeginTransactionAsync();
      var user = _mapper.Map<AppUser>(registerModel);

      user.UserName = user.Email;
      user.Avatar = await SaveImage(registerModel.AvatarFile);
      user.AppUserTeams = new List<AppUserTeam>();

      var createStatus = await _userManager.CreateAsync(user, registerModel.Password);

      if (!createStatus.Succeeded)
      {
        await transaction.RollbackAsync();
        return BadRequest(createStatus.Errors);
      }

      var addRoleStatus = await _userManager.AddToRoleAsync(user, "Staff");

      if (!addRoleStatus.Succeeded)
      {
        await transaction.RollbackAsync();
        return BadRequest(addRoleStatus.Errors);
      }

      if (!String.IsNullOrEmpty(registerModel.TeamId.ToString()))
      {
        var appUserTeam = new AppUserTeam
        {
          AppUserId = user.Id,
          TeamId = registerModel.TeamId
        };

        user.AppUserTeams.Add(appUserTeam);
        var addTeamResult = await _userManager.UpdateAsync(user);

        if (!addTeamResult.Succeeded)
        {
          await transaction.RollbackAsync();
          return BadRequest();
        }
      }

      transaction.Commit();

      var auth = new AuthModel
      {
        token = await _tokenService.CreateToken(user),
        User = _mapper.Map<UserDTO>(user),
        Role = await _userManager.GetRolesAsync(user)
      }; ;

      return new Response<AuthModel>
      {
        status = 200,
        success = true,
        Data = auth
      };

    }

    [HttpPost("login")]
    public async Task<ActionResult<Response<AuthModel>>> Login(LoginModel loginModel)
    {
      var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginModel.Email.ToLower());

      if (user == null)
      {
        return Unauthorized("Invalid User");
      }

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

      if (!result.Succeeded)
      {
        return Unauthorized();
      }

      var authDTO = new AuthModel
      {
        User = _mapper.Map<UserDTO>(user),
        token = await _tokenService.CreateToken(user),
        Role = await _userManager.GetRolesAsync(user)
      };

      return new Response<AuthModel>
      {
        Data = authDTO,
        success = true,
        status = 200
      };
    }

    private async Task<string> SaveImage(IFormFile imageFile)
    {
      string imageName = DateTime.Now.ToString("yyyymmssfff") + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

      var uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", "Avatars", imageName);

      using (var fileStream = new FileStream(uploadPath, FileMode.Create))
      {
        await imageFile.CopyToAsync(fileStream);
      }

      return imageName;
    }
  }
}
