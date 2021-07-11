using System;
using System.Collections.Generic;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Commands
{
	public class ConfigSettingCommand : IRequest<Guid>
	{
		public virtual int StartFormatTime { get; set; }
		public virtual int EndFormatTime { get; set; }
		public virtual long NotifyBeforeMeeting { get; set; }
		public virtual ICollection<Guid> User_Id { get; set; }
	}
}