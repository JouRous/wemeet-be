using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
	public class ExceptionHandler
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandler> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

				var response = _env.IsDevelopment()
					? new HttpException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
					: new HttpException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());

				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var json = JsonSerializer.Serialize(response, options);

				await context.Response.WriteAsync(json);
			}
		}
	}
}