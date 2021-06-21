using API.Entities;
using API.Types;
using API.DTO;
using API.Models;
using System;
using System.Threading.Tasks;

namespace API.Interfaces
{
	public interface INotificationRepo
	{
		void AddOne(Notification message);
		void MarkReadNotification(string Id);
		Task<Pagination<NotificationMessageDTO>> GetMessagesPagiantionAsync(PaginationParams paginationQuery);
		Task<Pagination<NotificationMessageDTO>> GetMessagesUnreadPaginationAsync(PaginationParams paginationQuery);
	}
}