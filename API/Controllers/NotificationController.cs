using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;

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


        [HttpPost]
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

    }
}
