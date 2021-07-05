using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Domain.DTO;
using API.Types;
using API.Utils;
using Domain.Interfaces;


namespace API.Services
{
    public class NotificationService : Hub
    {


        public async Task CreateNotify(NotificationMessageDTO msg)
        {
            try
            {
                var res = new ResponseBuilder<NotificationMessageDTO>()
                                .AddData(msg)
                                .Build();

                await Clients.All.SendAsync("ReceiveNotify", msg);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}