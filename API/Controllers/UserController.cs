using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }


    [Authorize(Roles = "Staff")]
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDTO>> GetUser(string username)
    {
      return await _userRepository.GetUserAsync(username);
    }

    public async Task<ActionResult<Pagination<UserDTO>>> GetUsers()
    {
      return await _userRepository.GetUsersAsync();
    }

  }
}
