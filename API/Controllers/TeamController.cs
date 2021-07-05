using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using MediatR;
using Application.Features.Team.Queries;

namespace API.Controllers
{
    public class TeamController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TeamController(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public async Task<ActionResult<Response<TeamWithUserDTO>>> GetTeam(int teamId)
        {
            var team = await _unitOfWork.TeamRepository.GetTeamAsync(teamId);

            return new ResponseBuilder<TeamWithUserDTO>()
                        .AddData(team)
                        .Build();
        }


        [HttpPost]
        public async Task<ActionResult> CreateTeam(TeamModel teamModel)
        {
            var team = _mapper.Map<Team>(teamModel);
            var leader = await _unitOfWork.UserRepository.GetUserEntityAsync(teamModel.l_id);

            if (leader == null)
            {
                return BadRequest(new
                {
                    status = 400,
                    success = false,
                    message = "Leader not found"
                });
            }

            var isLeaderRole = leader.Role.Equals(UserRoles.LEAD);

            if (!isLeaderRole)
            {
                return BadRequest(new
                {
                    success = false,
                    status = 400,
                    message = "Staff can't be a leader"
                });
            }

            team.Leader = leader;
            team.LeaderId = leader.Id;

            await _unitOfWork.TeamRepository.AddTeamAsync(team);

            team.AppUserTeams.Add(new AppUserTeam
            {
                AppUserId = team.LeaderId,
                TeamId = team.Id
            });

            await _unitOfWork.Complete();

            var response = new ResponseBuilder<TeamDTO>()
                                .AddData(_mapper.Map<TeamDTO>(team))
                                .Build();

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTeam(TeamModel teamModel)
        {
            var team = _mapper.Map<Team>(teamModel);

            await _unitOfWork.TeamRepository.UpdateTeamAsync(team);

            await _unitOfWork.Complete();

            return Accepted(new
            {
                status = 202,
                success = true,
                message = "Team had been updated",
            });
        }

        [HttpPost("add-user")]
        public async Task<ActionResult> AddUserToTeam([FromBody] UserTeamActionModel userTeamActionModel)
        {
            await _unitOfWork.TeamRepository.AddUserToTeamAsync(userTeamActionModel.Team_Id, userTeamActionModel.User_Ids);

            await _unitOfWork.Complete();

            return Ok(new
            {
                success = true,
                status = 200,
                message = "Users had beed add to team"
            });
        }

        [HttpPost("remove-user")]
        public async Task<ActionResult> RemoveUserFromTeam([FromBody] UserTeamActionModel userTeamActionModel)
        {
            await _unitOfWork.TeamRepository.RemoveUserFromTeam(userTeamActionModel.Team_Id, userTeamActionModel.User_Ids);
            await _unitOfWork.Complete();

            return Ok(new
            {
                success = true,
                status = 200,
                message = "Users had beed removed to team"
            });
        }

    }
}
