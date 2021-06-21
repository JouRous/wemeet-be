using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
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
    public async Task<ActionResult<Response<IEnumerable<TeamDTO>>>> GetTeams(
      [FromQuery] PaginationParams paginationParams, string filter = "", string sort = "created_at")
    {
      var result = await _unitOfWork.TeamRepository.GetAllAsync(paginationParams, filter, sort);

      var response = new ResponseBuilder<IEnumerable<TeamDTO>>()
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
      team.Users = new List<AppUser>();

      _unitOfWork.TeamRepository.AddTeam(team);

      bool saveStatus = await _unitOfWork.Complete();

      if (!saveStatus)
      {
        return BadRequest();
      }

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
        message = "Team had been updated"
      });
    }

    [HttpPost("add-user")]
    public async Task<ActionResult> AddUserToTeam([FromBody] UserTeamActionModel userTeamActionModel)
    {
      await _unitOfWork.TeamRepository.AddUserToTeamAsync(userTeamActionModel.TeamId, userTeamActionModel.UserIds);

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
      await _unitOfWork.TeamRepository.RemoveUserFromTeam(userTeamActionModel.TeamId, userTeamActionModel.UserIds);
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
