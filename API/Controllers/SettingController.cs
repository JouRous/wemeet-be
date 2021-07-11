using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Application.Services;
using Domain.Interfaces;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using System;
using MediatR;
using Application.Features.Commands;
using Application.Features.Queries;
using Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Domain.Entities;

namespace API.Controllers
{
	public class SettingController : BaseApiController
	{
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;
		private readonly IUserSettingRepository _setting;

		public SettingController(IMediator mediator, IMapper mapper, IUserSettingRepository setting)
		{
			_mapper = mapper;
			_mediator = mediator;
			_setting = setting;
		}

		[HttpGet]
		public async Task<ActionResult> GetUserSetting()
		{
			var token = await HttpContext.GetTokenAsync("access_token");
			var handler = new JwtSecurityTokenHandler();
			var userId = handler.ReadJwtToken(token).Claims
					.Where(c => c.Type.Equals("UserId"))
					.Select(c => c.Value)
					.SingleOrDefault();
			var config = await _setting.GetSetting(userId);
			if (config == null)
				return Ok(new ResponseBuilder<uint>().AddMessage("Error: No data not found !").Build());
			return Ok(config);
		}

		[HttpPost]
		public async Task<ActionResult> SetConfig([FromBody] SettingDTO settingDTO)
		{
			var token = await HttpContext.GetTokenAsync("access_token");
			var handler = new JwtSecurityTokenHandler();
			var userId = handler.ReadJwtToken(token).Claims
					.Where(c => c.Type.Equals("UserId"))
					.Select(c => c.Value)
					.SingleOrDefault();
			int x = Int32.Parse(userId);

			if (settingDTO.StartFormatTime > 24 || settingDTO.EndFormatTime > 24 || settingDTO.StartFormatTime < 0 || settingDTO.EndFormatTime < 0)
				return BadRequest(new ResponseBuilder<uint>().AddHttpStatus(404, false).AddMessage("err: Setting is Wrong !").Build());

			await _setting.ConfigSettingAsync(settingDTO, x);

			return Ok(new ResponseBuilder<uint>().AddMessage("Setting has beed configged").Build());
		}

	}
}