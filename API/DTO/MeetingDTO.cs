using System;
using System.Collections.Generic;

using API.Enums;
using API.Entities;
using API.Models;

namespace API.DTO
{
	public class MeetingDTO
	{
		public int? Id { get; set; } = null;
		public DateTime? CreatedAt { get; set; } = null;
		public string Name { get; set; } = null;
		public string Description { get; set; } = null;
		public string Note { get; set; } = null;
		public UserDTO Creator { get; set; } = null;
		public ICollection<UserDTO> UserInMeeting { get; set; } = null;
		public TeamDTO Team { get; set; } = null;
		public RoomDTO Room { get; set; } = null;
		public StatusMeeting? Status { get; set; } = null;
		public DateTime? StartTime { get; set; } = null;
		public DateTime? EndTime { get; set; } = null;
		public PriorityMeeting? Priority { get; set; } = null;
		public string Target { get; set; } = null;
		public MethodMeeting? Method { get; set; } = null;
		public MeetingDTO ConflictWith { get; set; } = null;
	}
}