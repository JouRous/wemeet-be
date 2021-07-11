using System;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
	public class GetUserSettingQuery : IRequest<SettingDTO>
	{
		public GetUserSettingQuery(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; set; }
	}
}