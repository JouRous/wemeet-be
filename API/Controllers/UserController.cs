using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserRepository _userRepository;
    public UsersController(UserManager<AppUser> userManager, IUserRepository userRepository)
    {
      _userManager = userManager;
      _userRepository = userRepository;
    }


    [HttpGet("{username}")]
    public async Task<ActionResult<UserDTO>> GetUser(string username)
    {
      return await _userRepository.GetUserAsync(username);
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<UserDTO>>> GetUsers()
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

      return await _userRepository.GetUsersAsync();
    }
  }
}
