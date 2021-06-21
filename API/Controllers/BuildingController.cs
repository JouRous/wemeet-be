using System;
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
		public async Task<ActionResult<Response<IEnumerable<BuildingDTO>>>> GetAllBuildings([FromQuery] PaginationParams paginationParams)
		{
			var result = await _unitOfWork.BuildingRepository.GetAllByPaginationAsync(paginationParams);
			var data = result.Items.ConvertAll(x =>
													{
														x.RoomCount = _unitOfWork.RoomRepository.GetSizeOfEntity(o => o.BuildingId == x.Id);
														return x;
													});

			var response = new ResponseBuilder<IEnumerable<BuildingDTO>>()
													.AddData(data)
													.AddPagination(new PaginationDTO
													{
														CurrentPage = result.CurrentPage,
														PageSize = result.PageSize,
														TotalItems = result.TotalItems
													})
													.Build();

      return response;
    }

		[HttpGet("{buildingId}")]
		public async Task<ActionResult<Response<BuildingDTO>>> GetBuildingInfo(string buildingId)
		{
			var buildingInfo = await _unitOfWork.BuildingRepository.GetOneAsync(buildingId);
			buildingInfo.RoomCount = _unitOfWork.RoomRepository.GetSizeOfEntity(x => x.BuildingId == buildingId);
			return new ResponseBuilder<BuildingDTO>().AddData(buildingInfo).Build();
		}

		[HttpPost]
		public async Task<ActionResult<Response<BuildingDTO>>> AddBuilding([FromBody] BuildingModel buildingInfo)
		{
			var transaction = await DbContext.Database.BeginTransactionAsync();

			var building = _mapper.Map<Building>(buildingInfo);

			building.Id = Guid.NewGuid().ToString();

			_unitOfWork.BuildingRepository.AddOne(building);

			var isCreated = await _unitOfWork.Complete();

			if (!isCreated)
			{
				return BadRequest();
			}

			var res = new ResponseBuilder<BuildingDTO>()
											.AddData(_mapper.Map<BuildingDTO>(building))
											.Build();

			return res;
		}

		[HttpPut("{buildingId}")]
		public async Task<ActionResult<Response<BuildingDTO>>> EditInfoBuilding(string buildingId, [FromBody] BuildingModel body)
		{
			var building = _mapper.Map<BuildingDTO>(body);

			building.Id = buildingId;

			_unitOfWork.BuildingRepository.ModifyOne(building);
			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted)
			{
				return BadRequest();
			}

			var res = new ResponseBuilder<BuildingDTO>()
											.AddData(building)
											.Build();

			return res;
		}

		[HttpDelete("{buildingId}")]
		public async Task<ActionResult<Response<string>>> RemoveBuilding(string buildingId)
		{
			_unitOfWork.BuildingRepository.DeletingOne(buildingId);

			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted)
			{
				return BadRequest();
			}

			var res = new ResponseBuilder<string>()
											.AddData(_mapper.Map<string>("deleted"))
											.Build();

			return res;
		}


	}
}