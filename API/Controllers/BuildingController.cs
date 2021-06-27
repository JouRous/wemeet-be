using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Services;
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
		private NotificationService _notificationService = new NotificationService();

		public BuildingController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<Response<IEnumerable<BuildingResDTO>>>> GetAllBuildings(
			[FromQuery] PaginationParams paginationParams, string filter = "", string sort = "created_at"
			)
		{
			var result = await _unitOfWork.BuildingRepository.GetAllByPaginationAsync(paginationParams, filter, sort);

			var list = new List<BuildingResDTO>();
			foreach (var item in result.Items)
			{
				var count = _unitOfWork.RoomRepository.GetSizeOfEntity(o => o.BuildingId == item.Id);
				list.Add(new BuildingResDTO()
				{
					Building = item,
					RoomNumber = count
				});
			}

			var response = new ResponseBuilder<IEnumerable<BuildingResDTO>>()
													.AddData(list)
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
		public async Task<ActionResult<Response<BuildingResDTO>>> GetBuildingInfo(int buildingId)
		{
			var buildingInfo = await _unitOfWork.BuildingRepository.GetOneAsync(buildingId);
			var data = new BuildingResDTO()
			{
				Building = buildingInfo,
				RoomNumber = _unitOfWork.RoomRepository.GetSizeOfEntity(x => x.BuildingId == buildingId)
			};
			return new ResponseBuilder<BuildingResDTO>().AddData(data).Build();
		}

		[HttpPost]
		public async Task<ActionResult<Response<BuildingDTO>>> AddBuilding([FromBody] BuildingModel buildingInfo)
		{
			try
			{
				var building = _mapper.Map<Building>(buildingInfo);

				_unitOfWork.BuildingRepository.AddOne(building);

				var isCreated = await _unitOfWork.Complete();

				if (!isCreated)
				{
					return BadRequest();
				}

				// var msg = new Notification()
				// {
				// 	EntityType = Enums.EntityEnum.Building,
				// 	EntityId = building.Id,
				// 	EndpointDetails = $"/api/building/{building.Id}",
				// 	Message = "New building has created !"
				// };

				// var msgDto = _mapper.Map<NotificationMessageDTO>(msg);

				// await _notificationService.CreateNotify(msgDto);

				var res = new ResponseBuilder<BuildingDTO>()
												.AddData(_mapper.Map<BuildingDTO>(building))
												.Build();

				return res;
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		[HttpPut]
		[Route("{buildingId}")]
		public async Task<ActionResult> EditInfoBuilding([FromRoute] int buildingId, [FromBody] BuildingModel body)
		{
			var building = _mapper.Map<BuildingDTO>(body);

			building.Id = buildingId;

			_unitOfWork.BuildingRepository.ModifyOne(building);
			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted)
			{
				return BadRequest();
			}

			// var msg = new Notification()
			// {
			// 	EntityType = Enums.EntityEnum.Building,
			// 	EntityId = building.Id,
			// 	EndpointDetails = $"/api/building/{building.Id}",
			// 	Message = "The building has updated !"
			// };

			// var msgDto = _mapper.Map<NotificationMessageDTO>(msg);

			// await _notificationService.CreateNotify(msgDto);

			return Accepted(new
			{
				status = 202,
				success = true,
				message = "Building had been updated",
				updated = building
			});
		}

		[HttpDelete("{buildingId}")]
		public async Task<ActionResult<Response<string>>> RemoveBuilding(int buildingId)
		{
			BuildingDTO building = await _unitOfWork.BuildingRepository.GetOneAsync(buildingId);

			_unitOfWork.BuildingRepository.DeletingOne(buildingId);

			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted)
			{
				return BadRequest();
			}

			// var msg = new Notification()
			// {
			// 	EntityType = Enums.EntityEnum.Building,
			// 	EntityId = building.Id,
			// 	EndpointDetails = $"/api/building/{building.Id}",
			// 	Message = "New building has deleted !"
			// };

			// var msgDto = _mapper.Map<NotificationMessageDTO>(msg);

			// await _notificationService.CreateNotify(msgDto);

			var res = new ResponseBuilder<string>()
											.AddData(_mapper.Map<string>("deleted"))
											.Build();

			return res;
		}


	}
}