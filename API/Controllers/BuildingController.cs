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
		public async Task<ActionResult<Response<IEnumerable<BuildingDTO>>>> GetAllBuildings(
			[FromQuery] PaginationParams paginationParams, string filter = "", string sort = "created_at"
			)
		{
			var result = await _unitOfWork.BuildingRepository.GetAllByPaginationAsync(paginationParams, filter, sort);


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

		[HttpPost]
		public async Task<ActionResult<Response<BuildingDTO>>> AddBuilding([FromBody] BuildingModel buildingInfo)
		{
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

		[HttpPut]
		[Route("{buildingId}")]
		public async Task<ActionResult> EditInfoBuilding([FromRoute] string buildingId, [FromBody] BuildingModel body)
		{
			var building = _mapper.Map<BuildingDTO>(body);

			building.Id = buildingId;

			_unitOfWork.BuildingRepository.ModifyOne(building);
			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted)
			{
				return BadRequest();
			}

			return Accepted(new
			{
				status = 202,
				success = true,
				message = "Building had been updated",
				updated = building
			});
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