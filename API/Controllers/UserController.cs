using System.Threading.Tasks;
using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [Authorize]
  public class UserController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDTO>> GetUser(string username)
    {
      return await _userRepository.GetUserAsync(username);
    }
  }
}