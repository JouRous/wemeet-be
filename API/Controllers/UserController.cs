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
using System;
using Application.Features.Commands;
using MediatR;
using Application.Features.Queries;
using Application.Exceptions;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;

        public UsersController(
            IMapper mapper,
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IEmailService emailService,
            IMediator mediator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _tokenService = tokenService;
            _mediator = mediator;
        }

        private async Task<bool> CheckUserExist(string email)
        {
            return await _userManager.Users.AnyAsync(user => user.Email == email);
        }


        [HttpPost()]
        public async Task<ActionResult> CreateUser([FromForm] CreateUserCommand commnad)
        {
            var result = Guid.Empty;

            try
            {
                result = await _mediator.Send(commnad);
            }
            catch (ApplicationException ex)
            {
                var exRes = new ResponseBuilder<Unit>()
                                .AddMessage(ex.Message)
                                .AddHttpStatus(400, false)
                                .Build();
                return BadRequest(exRes);
            }

            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .AddMessage("Create user success")
                                .Build();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(Guid id)
        {
            var result = await _mediator.Send(new GetUserQuery(id));

            var response = new ResponseBuilder<UserWithTeamUsersDTO>()
                                .AddData(result)
                                .Build();
            return Ok(response);
        }

        [HttpGet("get-by-email/{email}")]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            var result = await _mediator.Send(new GetUserByEmailQuery(email));

            var response = new ResponseBuilder<UserDTO>()
                                .AddData(result)
                                .Build();
            return Ok(response);
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
        public async Task<ActionResult> UpdateUser([FromForm] UpdateUserCommand command, Guid id)
        {
            command.Id = id;
            var result = Guid.Empty;

            try
            {
                result = await _mediator.Send(command);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseBuilder<Unit>()
                                    .AddMessage(ex.Message)
                                    .AddHttpStatus(404, false)
                                    .Build());
            }
            return Ok(new ResponseBuilder<Guid>()
                            .AddData(result)
                            .Build());
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeactivateUser(Guid id)
        {
            try
            {
                await _mediator.Send(new DeactivateUserCommand(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseBuilder<Unit>()
                                    .AddMessage(ex.Message).Build());
            }

            return Accepted(new ResponseBuilder<Unit>().Build());
        }

        [HttpGet("retrieve/{id}")]
        public async Task<ActionResult> RetrieveUser(Guid id)
        {
            try
            {
                await _mediator.Send(new RetrieveUserCommand(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseBuilder<Unit>()
                                    .AddMessage(ex.Message).Build());
            }

            return Accepted(new ResponseBuilder<Unit>().Build());
        }

    }
}
