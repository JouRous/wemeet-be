using Domain.Entities;
using API.Types;
using Domain.DTO;
using Domain.Models;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INotificationRepo
    {
        void AddOne(Notification message);
        void MarkReadNotification(string Id);
        Task<Pagination<NotificationMessageDTO>> GetMessagesPagiantionAsync(PaginationParams paginationQuery);
        Task<Pagination<NotificationMessageDTO>> GetMessagesUnreadPaginationAsync(PaginationParams paginationQuery);
    }
}