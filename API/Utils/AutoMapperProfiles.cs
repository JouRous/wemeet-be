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

		}
	}
}
