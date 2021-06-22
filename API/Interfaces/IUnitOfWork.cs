using System.Threading.Tasks;

namespace API.Interfaces
{
	public interface IUnitOfWork
	{
		IUserRepository USerRepository { get; }
		ITeamRepository TeamRepository { get; }
		IBuildingRepository BuildingRepository { get; }
		IRoomRepository RoomRepository { get; }
		INotificationRepo NotificationRepository { get; }
		Task<bool> Complete();
		bool HasChanges();
	}
}