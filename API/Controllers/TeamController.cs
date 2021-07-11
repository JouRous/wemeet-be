using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using MediatR;
using Application.Features.Queries;
using System;
using Application.Features.Commands;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class TeamController : BaseApiController
    {
        private readonly IMediator _mediator;

        public TeamController(IMapper mapper, IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetTeams(
            [FromQuery] Dictionary<string, int> page,
            [FromQuery] Dictionary<string, string> filter,
            [FromQuery] Dictionary<string, string> sort)
        {
            var teamQuery = QueryBuilder<FilterTeamModel>.Build(page, filter, sort);
            var query = new GetAllTeamQuery(teamQuery);

            var result = await _mediator.Send(query);
            var paginationDTO = new PaginationDTO
            {
                CurrentPage = result.CurrentPage,
                PerPage = result.PerPage,
                Total = result.Total,
                Count = result.Count,
                TotalPages = result.TotalPages
            };

            var response = new ResponseWithPaginationBuilder<IEnumerable<TeamWithUserDTO>>()
                .AddData(result.Items)
                .AddPagination(paginationDTO)
                .Build();

            return Ok(response);
        }

        [HttpGet("{teamId}")]
        public async Task<ActionResult> GetTeam(Guid teamId)
        {
            var query = new GetTeamQuery(teamId);
            var result = await _mediator.Send(query);

            var response = new ResponseBuilder<TeamWithUserDTO>()
                                .AddData(result)
                                .Build();

            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult> CreateTeam(CreateTeamCommand command)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var creatorId = handler.ReadJwtToken(token).Claims
                .Where(c => c.Type.Equals("UserId"))
                .Select(c => c.Value)
                .SingleOrDefault();

            command.CreatorId = new Guid(creatorId);
            Guid result = Guid.Empty;
            try
            {
                result = await _mediator.Send(command);
            }
            catch (NotFoundException notFoundException)
            {
                var notFoundRes = new ResponseBuilder<Unit>()
                                    .AddMessage(notFoundException.Message)
                                    .AddHttpStatus(404, false)
                                    .Build();
                return BadRequest(notFoundRes);
            }
            catch (ForbiddenException forbiddenException)
            {
                var forbiddentRes = new ResponseBuilder<Unit>()
                                    .AddMessage(forbiddenException.Message)
                                    .AddHttpStatus(403, false)
                                    .Build();
                return StatusCode(StatusCodes.Status403Forbidden, forbiddentRes);
            }

            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .AddMessage("Team has been created")
                                .Build();

            return Ok(response);

        }

        [HttpPut("{teamId}")]
        public async Task<ActionResult> UpdateTeam(Guid teamId, [FromBody] UpdateTeamCommand command)
        {
            command.Id = teamId;
            var result = Guid.Empty;

            try
            {
                result = await _mediator.Send(command);
            }
            catch (NotFoundException notFoundException)
            {
                var notFoundRes = new ResponseBuilder<Unit>()
                                    .AddMessage(notFoundException.Message)
                                    .AddHttpStatus(404, false)
                                    .Build();
                return BadRequest(notFoundRes);
            }

            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .AddMessage("Team has beed updated")
                                .Build();

            return Ok(response);

        }

        [HttpPost("add-user")]
        public async Task<ActionResult> AddUserToTeam([FromBody] UserTeamActionModel userTeamActionModel)
        {
            var command = new AddUserToTeamCommand(userTeamActionModel.Team_Id, userTeamActionModel.User_Ids);
            await _mediator.Send(command);

            return Ok(new ResponseBuilder<Unit>()
                        .Build());
        }

        [HttpPost("remove-user")]
        public async Task<ActionResult> RemoveUserFromTeam([FromBody] UserTeamActionModel userTeamActionModel)
        {
            var command = new RemoveUserFromTeamCommand(userTeamActionModel.Team_Id, userTeamActionModel.User_Ids);

            await _mediator.Send(command);

            return Ok(new ResponseBuilder<Unit>().Build());
        }

        [HttpGet("me")]
        public async Task<ActionResult> GetLeadingTeam()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var leaderId = handler.ReadJwtToken(token).Claims
                .Where(c => c.Type.Equals("UserId"))
                .Select(c => c.Value)
                .SingleOrDefault();

            var query = new GetLeadingTeamQuery(new Guid(leaderId));
            var result = await _mediator.Send(query);

            var response = new ResponseBuilder<IEnumerable<TeamBaseDTO>>()
                                .AddData(result)
                                .Build();

            return Ok(response);
        }

    }
}
