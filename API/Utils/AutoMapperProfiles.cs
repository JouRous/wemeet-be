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
			CreateMap<RegisterModel, AppUser>();

			CreateMap<Team, TeamDTO>();
			CreateMap<TeamModel, Team>();

			CreateMap<Building, BuildingDTO>();
			CreateMap<NewBuildingModel, Building>();
			CreateMap<Room, RoomDTO>();
			CreateMap<NewRoomModel, Room>();

		}
	}
}
