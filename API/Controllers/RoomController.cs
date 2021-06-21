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
	public class RoomController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RoomController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<Response<IEnumerable<RoomDTO>>>> GetTeams([FromQuery] PaginationParams paginationParams)
		{
			var result = await _unitOfWork.RoomRepository.GetAllByPaginationAsync(paginationParams);

			var response = new ResponseBuilder<IEnumerable<RoomDTO>>()
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

		[HttpGet("{RoomId}")]
		public async Task<ActionResult<Response<RoomDTO>>> GetRoomInfo(string RoomId)
		{
			var RoomInfo = await _unitOfWork.RoomRepository.GetOneAsync(RoomId);
			return new ResponseBuilder<RoomDTO>().AddData(RoomInfo).Build();
		}

		[HttpPost]
		public async Task<ActionResult<Response<RoomDTO>>> AddRoom([FromBody] RoomModel body)
		{
			var roomMapper = _mapper.Map<RoomDTO>(body);

			roomMapper.Id = Guid.NewGuid().ToString();

			_unitOfWork.RoomRepository.AddOne(roomMapper);

			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted) return BadRequest();

			var res = new ResponseBuilder<RoomDTO>().AddData(roomMapper).Build();



			return res;

		}

		[HttpPut("{roomId}")]
		public async Task<ActionResult<Response<RoomDTO>>> EditInfoRoom(string roomId, [FromBody] RoomModel body)
		{
			var roomMapper = _mapper.Map<RoomDTO>(body);
			roomMapper.Id = roomId;

			_unitOfWork.RoomRepository.UpdatingOne(roomMapper);

			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted) return BadRequest();

			var res = new ResponseBuilder<RoomDTO>().AddData(roomMapper).Build();

			return res;
		}

		[HttpDelete("{roomId}")]
		public async Task<ActionResult<Response<string>>> RemoveRoom(string roomId)
		{
			_unitOfWork.RoomRepository.DeletingOne(roomId);
			var isCompleted = await _unitOfWork.Complete();
			if (!isCompleted) return BadRequest();
			var res = new ResponseBuilder<string>().AddData("deleted").Build();

			return res;
		}

	}
}