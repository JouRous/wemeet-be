using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Models;
using API.Services;
using API.Types;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class UsersController : BaseApiController
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;

    public UsersController(
      IMapper mapper,
      UserManager<AppUser> userManager,
      IUnitOfWork unitOfWork,
      ITokenService tokenService,
      IEmailService emailService
      )
    {
      _userManager = userManager;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _emailService = emailService;
      _tokenService = tokenService;
    }

    private async Task<bool> CheckUserExist(string email)
    {
      return await _userManager.Users.AnyAsync(user => user.Email == email);
    }

    [HttpPost("create-user")]
    public async Task<ActionResult<Response<AuthModel>>> CreateUser([FromBody] RegisterModel registerModel)
    {
      if (await CheckUserExist(registerModel.Email))
      {
        return BadRequest("User already taken");
      }

      var transaction = await DbContext.Database.BeginTransactionAsync();
      var user = _mapper.Map<AppUser>(registerModel);

      user.UserName = user.Email;
      user.AppUserTeams = new List<AppUserTeam>();

      var randomPassword = Utils.Utils.RandomString(9);

      var createStatus = await _userManager.CreateAsync(user, randomPassword);

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

      if (!String.IsNullOrEmpty(registerModel.TeamId))
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

      await _emailService.sendMailAsync(user.Email, "Dang ky thanh cong.", $"Mat khau la {randomPassword}");

      var auth = new AuthModel
      {
        token = await _tokenService.CreateToken(user),
        User = _mapper.Map<UserDTO>(user),
        Role = await _userManager.GetRolesAsync(user)
      };

      return new Response<AuthModel>
      {
        status = 200,
        success = true,
        Data = auth
      };

    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDTO>> GetUser(string username)
    {
      return await _unitOfWork.USerRepository.GetUserAsync(username);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<UserDTO>>>> GetUsers(
    [FromQuery] PaginationParams paginationParams)
    {
      var token = await HttpContext.GetTokenAsync("access_token");
      var handler = new JwtSecurityTokenHandler();
      var roles = handler.ReadJwtToken(token)
             .Claims.Where(c => c.Type.Equals("role")).Select(c => c.Value).ToList();

      var checkRole = roles.Any(role => role.Equals("Admin"));

      if (!checkRole)
      {
        return Unauthorized("Admin only! Permission denied");
      }

      var res = await _unitOfWork.USerRepository.GetUsersAsync(paginationParams);

      var response = new ResponseBuilder<IEnumerable<UserDTO>>()
             .AddData(res.Items)
             .AddPagination(new PaginationDTO
             {
               CurrentPage = res.CurrentPage,
               PageSize = res.PageSize,
               TotalItems = res.TotalItems
             })
             .Build();

      return response;
    }

    [HttpGet("me")]
    public async Task<ActionResult> GetProfile()
    {
      var token = await HttpContext.GetTokenAsync("access_token");
      var handler = new JwtSecurityTokenHandler();

      var email = handler.ReadJwtToken(token)
             .Claims.Where(c => c.Type.Equals("email")).Select(c => c.Value).SingleOrDefault();
      var roles = handler.ReadJwtToken(token)
             .Claims.Where(c => c.Type.Equals("role")).Select(c => c.Value).ToList();

      var User = await _unitOfWork.USerRepository.GetUserAsync(email);
      var profile = new
      {
        User = User,
        Roles = roles
      };

      return Ok(profile);
    }

  }
}
