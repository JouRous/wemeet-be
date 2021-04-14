using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Types;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AuthController : BaseApiController
  {
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
    {
      _tokenService = tokenService;
      _mapper = mapper;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    private async Task<bool> CheckUserExist(string username)
    {
      return await _userManager.Users.AnyAsync(user => user.UserName == username);
    }

    [HttpPost("register")]
    public async Task<ActionResult<Response<AuthDTO>>> Register(RegisterDTO registerDTO)
    {
      if (await CheckUserExist(registerDTO.Username))
      {
        return BadRequest("User already taken");
      }

      var user = _mapper.Map<AppUser>(registerDTO);

      user.UserName = registerDTO.Username.ToLower();

      var result = await _userManager.CreateAsync(user, registerDTO.Password);

      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      var returnUser = _mapper.Map<UserDTO>(user);

      var authDTO = new AuthDTO
      {
        token = _tokenService.CreateToken(user),
        User = returnUser
      };

      return new Response<AuthDTO>
      {
        status = 200,
        success = true,
        Data = authDTO
      };

    }

    [HttpPost("login")]
    public async Task<ActionResult<Response<AuthDTO>>> Login(LoginDTO loginDTO)
    {
      var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

      if (user == null)
      {
        return Unauthorized("Invalid User");
      }

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

      if (!result.Succeeded)
      {
        return Unauthorized();
      }

      var returnUser = _mapper.Map<UserDTO>(user);

      var authDTO = new AuthDTO
      {
        User = returnUser,
        token = _tokenService.CreateToken(user)
      };

      return new Response<AuthDTO>
      {
        Data = authDTO,
        success = true,
        status = 200
      };
    }
  }
}
