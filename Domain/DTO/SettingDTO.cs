using System;
namespace Domain.DTO
{
	public class SettingDTO
	{
		public Guid? Id { get; set; }
		public int? StartFormatTime { get; set; } = 0;
		public int? EndFormatTime { get; set; } = 24;
		public long? NotifyBeforeMeeting { get; set; } = 0;
		public UserDTO User { get; set; }
	}
}
