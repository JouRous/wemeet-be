using System.Linq;
using API.DTO;
using API.Entities;
using API.Models;
using AutoMapper;

namespace API.Utils
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<AppUser, UserDTO>()
				.ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.Team).ToList()))
				.ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));
			CreateMap<UserActionModel, AppUser>();

			CreateMap<Team, TeamDTO>()
				.ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.User).ToList()));
			CreateMap<TeamModel, Team>();

			CreateMap<Building, BuildingDTO>();
			CreateMap<BuildingModel, Building>();
			CreateMap<BuildingModel, BuildingDTO>();

			CreateMap<Room, RoomDTO>();
			CreateMap<RoomModel, Room>();
			CreateMap<RoomModel, RoomDTO>();

			CreateMap<Notification, NotificationMessageDTO>();

			CreateMap<Meeting, MeetingDTO>()
				.ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
				.ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
				.ForMember(dest => dest.UserInMeeting, opt => opt.MapFrom(src => src.UsersInMeeting))
				.ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team))
				.ForMember(dest => dest.ConflictWith, opt => opt.MapFrom(src => src.ConflictWith));
			CreateMap<MeetingModel, MeetingDTO>();
			CreateMap<Meeting, Meeting>();

		}
	}
}
