using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using API.DTO;
using API.Types;
using API.Utils;
using API.Interfaces;


namespace API.Services
{
	public class NotificationService : Hub
	{
		private async Task PushNotification(Response<NotificationMessageDTO> msg)
		{
			await Clients.All.SendAsync("ReceiveNotify", msg);
		}

		public async Task CreateNotify(NotificationMessageDTO msg)
		{
			try
			{
				var res = new ResponseBuilder<NotificationMessageDTO>()
								.AddData(msg)
								.Build();

				await PushNotification(res);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}