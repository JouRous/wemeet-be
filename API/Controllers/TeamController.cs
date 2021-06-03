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
    public async Task<ActionResult<Response<IEnumerable<TeamDTO>>>> GetTeams([FromQuery] PaginationParams paginationParams)
    {
      var result = await _unitOfWork.TeamRepository.GetAllAsync(paginationParams);

      var response = new ResponseBuilder<IEnumerable<TeamDTO>>()
                          .AddData(result.Items)
                          .AddPagination(new PaginationDTO
                          {
                            CurrentPage = result.CurrentPage,
                            PageSize = result.PageSize,
                            TotalItems = result.TotalItems
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

  }
}
