using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using Domain.Entities;
using Application.Features.Commands;
using MediatR;
using System;
using Application.Features.Queries;
using Domain.Models;
using API.Extensions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Domain.Enums;

namespace API.Controllers
{
    public class MeetingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MeetingController(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<MeetingDTO>>>> GetAlls(
            [FromQuery] Dictionary<string, int> page,
            [FromQuery] Dictionary<string, string> filter,
            [FromQuery] Dictionary<string, string> sort)
        {
            var meetingQuery = QueryBuilder<MeetingFilterModel>
                        .Build(page, filter, sort);
            var query = new GetAllMeetingQuery(meetingQuery);

            var result = await _mediator.Send(query);

            var paginationDTO = new PaginationDTO
            {
                CurrentPage = result.CurrentPage,
                PerPage = result.PerPage,
                Total = result.Total,
                Count = result.Count,
                TotalPages = result.TotalPages
            };

            var response = new ResponseWithPaginationBuilder<IEnumerable<MeetingDTO>>()
                                .AddData(result.Items)
                                .AddPagination(paginationDTO)
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
        public async Task<ActionResult> AddMeeting([FromForm] CreateMeetingCommand command)
        {
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
            [FromForm] UpdateMeetingCommand command)
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

        [HttpGet("handling/{meetingId}/{status}")]
        public async Task<ActionResult> HandlingMeeting(Guid meetingId, StatusMeeting status)
        {
            var command = new HandlingMeetingCommand(meetingId, status);
            await _mediator.Send(command);

            return Ok(new ResponseBuilder<Unit>().AddMessage("Meeting has been processed").Build());
        }
    }
}