using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Interfaces;
using API.Models;
using API.Types;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class MeetingController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public MeetingController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<Response<IEnumerable<MeetingDTO>>>> GetAlls(
			[FromQuery] PaginationParams paginationParams, string filter = "", string sort = "-created_at")
		{
			var result = await _unitOfWork.MeetingRepository.GetAllByPaginationAsync(paginationParams, filter, sort);

			var response = new ResponseBuilder<IEnumerable<MeetingDTO>>()
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
		[HttpGet]
		[Route("waiting")]
		public async Task<ActionResult<Response<IEnumerable<MeetingDTO>>>> GetWaitingMeeting(
			[FromQuery] PaginationParams paginationParams, string filter = "", string sort = "-created_at")
		{
			var result = await _unitOfWork.MeetingRepository.GetWaitMeetingByPaginationAsync(paginationParams, filter, sort);

			var response = new ResponseBuilder<IEnumerable<MeetingDTO>>()
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

		[HttpGet("{MeetingId}")]
		public ActionResult<Response<MeetingDTO>> GetMeetingInfo(int MeetingId)
		{
			var MeetingInfo = _unitOfWork.MeetingRepository.GetOneAsync(MeetingId);
			return new ResponseBuilder<MeetingDTO>().AddData(MeetingInfo).Build();
		}

		[HttpPost]
		public async Task<ActionResult<Response<MeetingDTO>>> AddMeeting([FromBody] MeetingModel body)
		{
			var MeetingMapper = _mapper.Map<MeetingDTO>(body);

			_unitOfWork.MeetingRepository.AddOne(MeetingMapper);

			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted) return BadRequest();

			var res = new ResponseBuilder<MeetingDTO>().AddData(_mapper.Map<MeetingDTO>(MeetingMapper)).Build();

			return res;

		}

		[HttpPut]
		[Route("{MeetingId}")]
		public async Task<ActionResult<Response<MeetingDTO>>> EditInfoMeeting([FromRoute] int MeetingId, [FromBody] MeetingModel body)
		{
			var MeetingMapper = _mapper.Map<MeetingDTO>(body);
			MeetingMapper.Id = MeetingId;

			_unitOfWork.MeetingRepository.UpdatingOne(MeetingMapper);

			var isCompleted = await _unitOfWork.Complete();

			if (!isCompleted) return BadRequest();



			var res = new ResponseBuilder<MeetingDTO>().AddData(MeetingMapper).Build();

			return Accepted(res);
		}

		[HttpDelete("{MeetingId}")]
		public async Task<ActionResult<Response<string>>> RemoveMeeting(int MeetingId)
		{
			var Meeting = _unitOfWork.MeetingRepository.GetOneAsync(MeetingId);
			_unitOfWork.MeetingRepository.DeletingOne(MeetingId);
			var isCompleted = await _unitOfWork.Complete();
			if (!isCompleted) return BadRequest();

			var res = new ResponseBuilder<string>().AddData("deleted").Build();

			return res;
		}
	}
}