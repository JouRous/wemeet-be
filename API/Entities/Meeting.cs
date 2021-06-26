using System;
using System.Collections.Generic;
using API.Enums;

namespace API.Entities
{
	public class Meeting : Bases
	{
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual string Note { get; set; }
		public virtual AppUser Creator { get; set; }
		public virtual ICollection<AppUser> UsersInMeeting { get; set; } = new List<AppUser>();
		public virtual Team Team { get; set; }
		public virtual Room Room { get; set; }
		public virtual StatusMeeting Status { get; set; }
		public virtual DateTime StartTime { get; set; }
		public virtual DateTime EndTime { get; set; }
		public virtual PriorityMeeting Priority { get; set; }
		public virtual string Target { get; set; }
		public virtual MethodMeeting Method { get; set; } = MethodMeeting.Offline;
		public virtual Meeting ConflictWith { get; set; }
	}
}