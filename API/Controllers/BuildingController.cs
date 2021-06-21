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
  public class BuildingController : BaseApiController
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BuildingController(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<BuildingDTO>>>> GetTeams([FromQuery] PaginationParams paginationParams)
    {
      var result = await _unitOfWork.BuildingRepository.GetAllByPaginationAsync(paginationParams);

      var response = new ResponseBuilder<IEnumerable<BuildingDTO>>()
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

    [HttpGet("{buildingId}")]
    public async Task<ActionResult<Response<BuildingDTO>>> GetBuildingInfo(string buildingId)
    {
      var buildingInfo = await _unitOfWork.BuildingRepository.GetOneAsync(buildingId);
      return new ResponseBuilder<BuildingDTO>().AddData(buildingInfo).Build();
    }
  }
}