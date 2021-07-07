using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using Domain.Entities;
using Application.Features.Commands;
using MediatR;
using System;
using Application.Features.Queries;

namespace API.Controllers
{
    public class MeetingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public MeetingController(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<MeetingDTO>>>> GetAlls(
            [FromQuery] PaginationParams paginationParams, string filter = "", string sort = "-created_at")
        {
            var result = await _unitOfWork.MeetingRepository.GetAllByPaginationAsync(paginationParams, filter, sort);

            var response = new ResponseWithPaginationBuilder<IEnumerable<MeetingDTO>>()
                                                    .AddData(result.Items)
                                                    .AddPagination(new PaginationDTO
                                                    {
                                                        CurrentPage = result.CurrentPage,
                                                        PerPage = result.PerPage,
                                                        Total = result.Total,
                                                        Count = result.Count,
                                                        TotalPages = result.TotalPages
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

            var response = new ResponseWithPaginationBuilder<IEnumerable<MeetingDTO>>()
                                                    .AddData(result.Items)
                                                    .AddPagination(new PaginationDTO
                                                    {
                                                        CurrentPage = result.CurrentPage,
                                                        PerPage = result.PerPage,
                                                        Total = result.Total,
                                                        Count = result.Count,
                                                        TotalPages = result.TotalPages
                                                    })
                                                    .Build();

            return response;
        }

        [HttpGet("{MeetingId}")]
        public async Task<ActionResult<Response<MeetingDTO>>> GetMeetingInfoAsync(Guid meetingId)
        {
            var query = new GetMeetingQuery(meetingId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddMeeting([FromBody] CreateMeetingCommand command)
        {
            var meeting = _mapper.Map<Meeting>(command);

            var result = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                        .AddData(result)
                        .AddMessage("Meeting has been created")
                        .Build();

            return Ok(response);
        }

        [HttpPut]
        [Route("{MeetingId}")]
        public async Task<ActionResult> EditInfoMeeting(
            [FromRoute] Guid meetingId,
            [FromBody] UpdateMeetingCommand command)
        {
            command.Id = meetingId;

            var res = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                                .AddData(res)
                                .AddMessage("Meeting has been updated")
                                .Build();

            return Ok(response);
        }

        [HttpDelete("{MeetingId}")]
        public async Task<ActionResult> RemoveMeeting(Guid meetingId)
        {
            var command = new DeleteMeetingCommand(meetingId);

            var result = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                            .AddData(result)
                            .AddMessage("Meeting has been removed")
                            .Build();

            return Ok(response);
        }
    }
}