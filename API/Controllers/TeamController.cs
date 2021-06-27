using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Enums;
using API.Interfaces;
using API.Models;
using API.Types;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class TeamController : BaseApiController
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TeamController(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<TeamWithUserDTO>>>> GetTeams(
      [FromQuery] Dictionary<string, int> page,
      [FromQuery] Dictionary<string, string> filter,
      [FromQuery] Dictionary<string, string> sort)
    {
      var _sort = sort.GetValueOrDefault("");
      var result = await _unitOfWork.TeamRepository.GetAllAsync(page, filter, _sort);

      var response = new ResponseBuilder<IEnumerable<TeamWithUserDTO>>()
                          .AddData(result.Items)
                          .AddPagination(new PaginationDTO
                          {
                            CurrentPage = result.CurrentPage,
                            PerPage = result.PerPage,
                            Total = result.Total,
                            Count = result.Count,
                            TotalPage = result.TotalPages
                          })
                          .Build();

      return response;
    }

    [HttpGet("{teamId}")]
    public async Task<ActionResult<Response<TeamDTO>>> GetTeam(int teamId)
    {
      var team = await _unitOfWork.TeamRepository.GetTeamAsync(teamId);

      return new ResponseBuilder<TeamDTO>()
                  .AddData(team)
                  .Build();
    }


    [HttpPost]
    public async Task<ActionResult<Response<TeamDTO>>> CreateTeam(TeamModel teamModel)
    {
      var team = _mapper.Map<Team>(teamModel);
      // team.Users = new List<AppUser>();\
      var leader = await _unitOfWork.USerRepository.FindById(teamModel.l_id);

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
          message = "Staff can't be leader"
        });
      }

      team.Leader = leader;
      team.LeaderId = leader.Id;

      _unitOfWork.TeamRepository.AddTeam(team);

      bool saveStatus = await _unitOfWork.Complete();

      if (!saveStatus)
      {
        return BadRequest();
      }

      team.AppUserTeams.Add(new AppUserTeam
      {
        AppUserId = team.LeaderId,
        TeamId = team.Id
      });

      await _unitOfWork.Complete();

      var response = new ResponseBuilder<TeamDTO>()
                          .AddData(_mapper.Map<TeamDTO>(team))
                          .Build();

      return response;
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
