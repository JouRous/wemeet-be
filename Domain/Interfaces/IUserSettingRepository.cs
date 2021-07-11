using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;

namespace Domain.Interfaces
{
	public interface IUserSettingRepository
	{
		Task<SettingDTO> GetSetting(string UserId);
		Task ConfigSettingAsync(SettingDTO config, int userId);
	}
}