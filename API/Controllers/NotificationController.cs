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
	public class NotificationController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public NotificationController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult> GetAll(
			[FromQuery] PaginationParams paginationParams)
		{
			var result = await _unitOfWork.NotificationRepository.GetMessagesPagiantionAsync(paginationParams);

			var response = new ResponseBuilder<IEnumerable<NotificationMessageDTO>>()
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

			return Accepted(response);
		}

		[HttpGet]
		[Route("{notifyId}")]
		public async Task<ActionResult> MarkRead([FromRoute] string notifyId)
		{
			_unitOfWork.NotificationRepository.MarkReadNotification(notifyId);
			var isDone = await _unitOfWork.Complete();
			string result = "marked read";
			if (!isDone) result = "error";

			var res = new ResponseBuilder<string>()
									.AddData(result)
									.Build();
			return Accepted(res);
		}


		[HttpGet]
		[Route("unread")]
		public async Task<ActionResult> GetUnReadNotify([FromQuery] PaginationParams paginationParams)
		{
			var result = await _unitOfWork.NotificationRepository.GetMessagesUnreadPaginationAsync(paginationParams);
			var response = new ResponseBuilder<IEnumerable<NotificationMessageDTO>>()
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

			return Accepted(response);
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
