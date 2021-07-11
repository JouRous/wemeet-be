using System;

namespace Domain.Entities
{
	public class UserSetting : Bases
	{
		public virtual int StartFormatTime { get; set; }
		public virtual int EndFormatTime { get; set; }
		public virtual long NotifyBeforeMeeting { get; set; }
		public virtual AppUser User { get; set; }
	}
}