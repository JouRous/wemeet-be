using System;
using System.IO;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Application.Utils;
using MediatR;
using Application.Features.Commands;
using Application.Features.Queries;
using Application.Exceptions;

namespace API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginCommand command)
        {

            var result = new AuthModel();

            try
            {
                result = await _mediator.Send(command);
            }
            catch (ApplicationException exception)
            {
                var exceptionRes = new ResponseBuilder<Unit>()
                                    .AddMessage(exception.Message)
                                    .AddHttpStatus(401, false)
                                    .Build();

                return Unauthorized(exceptionRes);
            }

            var response = new ResponseBuilder<AuthModel>()
                                .AddData(result)
                                .AddMessage("Login success")
                                .Build();

            return Ok(response);

        }

        [HttpPost("forget-request")]
        public async Task<ActionResult> ForgetRequest(ForgetPasswordRequest request)
        {

            try
            {
                await _mediator.Send(request);
            }
            catch (NotFoundException exception)
            {
                var excRes = new ResponseBuilder<Unit>()
                                    .AddMessage(exception.Message)
                                    .AddHttpStatus(400, false)
                                    .Build();
                return NotFound(excRes);
            }

            return Ok(new ResponseBuilder<Unit>()
                        .AddMessage("Request success")
                        .Build());
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);

            return Accepted(new ResponseBuilder<Unit>().AddMessage("Password has been reset").Build());
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> GetProfile()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var query = new GetProfileQuery(token);
            var result = await _mediator.Send(query);

            var response = new ResponseBuilder<UserDTO>()
                                .AddData(result)
                                .Build();

            return Ok(response);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            command.Token = token;

            await _mediator.Send(command);

            return Accepted(new ResponseBuilder<Unit>().AddMessage("Password has changed").Build());
        }
    }
}
