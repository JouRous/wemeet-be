using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using Application.Features.Commands;
using MediatR;
using System;
using Application.Features.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Domain.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class MeetingController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMeetingRepo _meetingRepo;

        public MeetingController(IMediator mediator, IWebHostEnvironment hostEnvironment, IMeetingRepo meetingRepo)
        {
            _mediator = mediator;
            _meetingRepo = meetingRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<MeetingDTO>>>> GetAlls(
            [FromQuery] Dictionary<string, int> page,
            [FromQuery] Dictionary<string, string> filter,
            [FromQuery] Dictionary<string, string> sort)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var role = handler.ReadJwtToken(token).Claims
                .Where(c => c.Type.Equals("role"))
                .Select(c => c.Value)
                .SingleOrDefault();
            var userId = handler.ReadJwtToken(token).Claims
                .Where(c => c.Type.Equals("UserId"))
                .Select(c => c.Value)
                .SingleOrDefault();

            var meetingQuery = QueryBuilder<MeetingFilterModel>
                        .Build(page, filter, sort);
            meetingQuery.filter.Role = role;
            meetingQuery.filter.me = Guid.Parse(userId);

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

        [HttpGet("waiting")]
        public async Task<ActionResult> GetWaitingMeeting(
            [FromQuery] Dictionary<string, int> page,
            [FromQuery] Dictionary<string, string> filter,
            [FromQuery] Dictionary<string, string> sort
        )
        {
            var meetingQuery = QueryBuilder<MeetingFilterModel>
                       .Build(page, filter, sort);
            var query = new GetWatingMeetingQuery(meetingQuery);

            var result = await _mediator.Send(query);

            var paginationDTO = new PaginationDTO
            {
                CurrentPage = result.CurrentPage,
                PerPage = result.PerPage,
                Total = result.Total,
                Count = result.Count,
                TotalPages = result.TotalPages
            };

            var response = new ResponseWithPaginationBuilder<IEnumerable<MeetingBaseDTO>>()
                                .AddData(result.Items)
                                .AddPagination(paginationDTO)
                                .Build();

            return Ok(response);
        }

        [HttpGet("get-by-team/{teamId}")]
        public async Task<ActionResult> GetMeetingByTeam(Guid teamId,
            [FromQuery] Dictionary<string, int> page,
            [FromQuery] Dictionary<string, string> filter,
            [FromQuery] Dictionary<string, string> sort)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var role = handler.ReadJwtToken(token).Claims
                .Where(c => c.Type.Equals("role"))
                .Select(c => c.Value)
                .SingleOrDefault();

            var meetingQuery = QueryBuilder<MeetingFilterModel>.Build(page, filter, sort);
            meetingQuery.filter.Role = role;
            var query = new GetMeetingByTeamQuery(teamId, meetingQuery);

            var result = await _mediator.Send(query);

            var paginationDTO = new PaginationDTO
            {
                CurrentPage = result.CurrentPage,
                PerPage = result.PerPage,
                Total = result.Total,
                Count = result.Count,
                TotalPages = result.TotalPages
            };
            return Ok(new ResponseWithPaginationBuilder<IEnumerable<MeetingDTO>>()
                            .AddData(result.Items)
                            .AddPagination(paginationDTO)
                            .Build());
        }

        [HttpGet("{MeetingId}")]
        public async Task<ActionResult<Response<MeetingDTO>>> GetMeetingInfoAsync(Guid meetingId)
        {
            var query = new GetMeetingQuery(meetingId);

            var result = await _mediator.Send(query);

            var response = new ResponseBuilder<MeetingDTO>()
                                .AddData(result)
                                .Build();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddMeeting([FromForm] CreateMeetingCommand command)
        {
            var result = Guid.Empty;

            try
            {
                result = await _mediator.Send(command);
            }
            catch (CreateMeetingException ex)
            {
                var exResponse = new ResponseBuilder<IEnumerable<MeetingBase>>()
                                    .AddData(ex.conflictMeetings)
                                    .AddHttpStatus(409, false)
                                    .AddMessage("Cant create meeting")
                                    .Build();
                return Conflict(exResponse);
            }

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

        [HttpGet("week/{roomId}/{monday}")]
        public async Task<ActionResult> GetCalendarByWeek(Guid roomId, DateTime monday)
        {
            var query = new GetCalendarByWeekQuery(roomId, monday);
            var result = await _mediator.Send(query);

            return Ok(new ResponseBuilder<IEnumerable<object>>()
                            .AddData(result)
                            .Build());
        }

        [HttpGet("month/{roomId}/{firstDay}")]
        public async Task<ActionResult> GetCalendaByMonth(Guid roomId, DateTime firstDay)
        {
            var query = new GetCalendaByMonthQuery(roomId, firstDay);
            var result = await _mediator.Send(query);

            var response = new ResponseBuilder<IEnumerable<object>>()
                            .AddData(result)
                            .Build();

            return Ok(response);
        }
    }
}