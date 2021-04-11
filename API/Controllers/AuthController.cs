using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AuthController : BaseApiController
  {
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthController(AppDbContext context, ITokenService tokenService, IMapper mapper)
    {
      _context = context;
      _tokenService = tokenService;
      _mapper = mapper;
    }

    private async Task<bool> CheckUserExist(string username)
    {
      return await _context.Users.AnyAsync(user => user.Username == username);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthDTO>> Register(RegisterDTO registerDTO)
    {
      if (await CheckUserExist(registerDTO.Username))
      {
        return BadRequest("Username already used!");
      }

      using var hmac = new HMACSHA512();

      var appUser = new AppUser
      {
        Username = registerDTO.Username,
        Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
        PasswordSalt = hmac.Key
      };

      _context.Users.Add(appUser);
      await _context.SaveChangesAsync();

      var user = _mapper.Map<UserDTO>(appUser);

      return new AuthDTO
      {
        User = user,
        token = _tokenService.CreateToken(appUser)
      };
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthDTO>> Login(LoginDTO loginDTO)
    {
      var appUser = await _context.Users.SingleOrDefaultAsync(user => user.Username == loginDTO.Username);

      if (appUser == null)
      {
        return Unauthorized("Invalid user!");
      }

      using var hmac = new HMACSHA512(appUser.PasswordSalt);

      var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

      for (int i = 0; i < computeHash.Length; i++)
      {
        if (computeHash[i] != appUser.Password[i])
        {
          return Unauthorized("Invalid user!");
        }
      }

      var user = _mapper.Map<UserDTO>(appUser);

      return new AuthDTO
      {
        User = user,
        token = _tokenService.CreateToken(appUser)
      };
    }
  }
}