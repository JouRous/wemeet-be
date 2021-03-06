using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITeamRepository TeamRepository { get; }
        IBuildingRepository BuildingRepository { get; }
        IRoomRepository RoomRepository { get; }
        INotificationRepo NotificationRepository { get; }
        IMeetingRepo MeetingRepository { get; }
        Task<bool> Complete();
    }
}