using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Utils;

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

        [HttpPost()]
        public async Task<ActionResult<Response<AuthModel>>> CreateUser([FromBody] UserActionModel userActionModel)
        {
            if (await CheckUserExist(userActionModel.Email))
            {
                return StatusCode(StatusCodes.Status409Conflict, new
                {
                    status = 409,
                    success = false,
                    message = "User already exist"
                });
            }

            var transaction = await DbContext.Database.BeginTransactionAsync();
            var user = _mapper.Map<AppUser>(userActionModel);

            user.UserName = user.Email;
            user.isFirstLogin = true;
            user.AppUserTeams = new List<AppUserTeam>();
            user.UnsignedName = StringHelper.RemoveAccentedString(user.Fullname);
            user.Role = userActionModel.Role;
            var randomPassword = StringHelper.RandomString(9);

            var createStatus = await _userManager.CreateAsync(user, randomPassword);

            if (!createStatus.Succeeded)
            {
                await transaction.RollbackAsync();
                return BadRequest(createStatus.Errors);
            }

            transaction.Commit();

            await _emailService.sendMailAsync(user.Email, "Dang ky thanh cong.", $"Mat khau la {randomPassword}");

            return Ok(new
            {
                success = true,
                status = 200,
                message = "Create user success"
            });

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithTeamUsersDTO>> GetUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(id);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    status = 404,
                    success = false
                });
            }

            return Ok(new
            {
                data = user,
                status = 200,
                success = true
            });
        }

        [HttpGet("get-by-email/{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);

            // if (user == null)
            // {
            //   return StatusCode(StatusCodes.Status404NotFound, new
            //   {
            //     status = 404,
            //     success = false
            //   });
            // }

            return Ok(new
            {
                data = user,
                status = 200,
                success = true
            });
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<UserWithTeamDTO>>>> GetUsers(
        [FromQuery] Dictionary<string, int> page,
        [FromQuery] Dictionary<string, string> filter,
        [FromQuery] Dictionary<string, string> sort)
        {
            var userQuery = QueryBuilder<UserFilterModel>.Build(page, filter, sort);

            var result = await _unitOfWork.UserRepository.GetUsersAsync(userQuery);

            var response = new ResponseWithPaginationBuilder<IEnumerable<UserWithTeamDTO>>()
                   .AddData(result.Items)
                   .AddPagination(new PaginationDTO
                   {
                       CurrentPage = result.CurrentPage,
                       PerPage = result.PerPage,
                       Total = result.Total,
                       Count = (int)result.Count,
                       TotalPages = result.TotalPages
                   })
                   .Build();

            return response;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser([FromBody] UserActionModel userActionModel, int id)
        {
            var user = _mapper.Map<AppUser>(userActionModel);
            var _user = await _unitOfWork.UserRepository.UpdateUserAsync(user, id);
            _user.isActive = userActionModel.is_active;

            await _unitOfWork.Complete();


            return Accepted(new
            {
                status = 202,
                success = true,
                message = "User had been updated"
            });

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeactivateUser(int id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return NotFound(new
                {
                    status = 404,
                    susscess = true
                });
            }

            _unitOfWork.UserRepository.DeactivateUser(user);
            if (!(await _unitOfWork.Complete()))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = 500,
                    sussess = false,
                    message = "Internal Server Error"
                });
            }

            return Accepted(new
            {
                status = 202,
                success = true,
                message = "User had been deactivate"
            });
        }

        [HttpGet("retrieve/{id}")]
        public async Task<ActionResult> RetrieveUser(int id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return NotFound(new
                {
                    status = 404,
                    susscess = true
                });
            }

            _unitOfWork.UserRepository.RetrieveUser(user);
            if (!(await _unitOfWork.Complete()))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = 500,
                    sussess = false,
                    message = "Internal Server Error"
                });
            }

            return Accepted(new
            {
                status = 202,
                success = true,
                message = "User had been retrieve"
            });

        }

    }
}
