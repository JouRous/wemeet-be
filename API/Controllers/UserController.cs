using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Types;
using API.Utils;
using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IMapper mapper, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
    {
      _userManager = userManager;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
    }


    [HttpGet("{username}")]
    public async Task<ActionResult<UserDTO>> GetUser(string username)
    {
      return await _unitOfWork.USerRepository.GetUserAsync(username);
    }

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
  }
}
