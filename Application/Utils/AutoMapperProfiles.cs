using System.Linq;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using AutoMapper;
using Application.Features.Commands;

namespace Application.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDTO>()
              .ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));

            CreateMap<AppUser, UserWithTeamDTO>()
              .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.Team)))
              .ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));

            CreateMap<AppUser, UserWithTeamUsersDTO>()
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.Team)))
            .ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));

            CreateMap<UserActionModel, AppUser>();

            CreateMap<Team, TeamDTO>()
              .ForMember(dest => dest.Leader, opt => opt.MapFrom(src => src.Leader));
            CreateMap<Team, TeamWithUserDTO>()
              .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.User)))
              .ForMember(dest => dest.Leader, opt => opt.MapFrom(src => src.Leader));
            CreateMap<TeamModel, Team>();

            CreateMap<Building, BuildingDTO>();
            CreateMap<Building, BuildingBaseDTO>().ReverseMap();
            CreateMap<CreateBuildingCommand, Building>().ReverseMap();
            CreateMap<UpdatebuildingCommand, Building>().ReverseMap();

            CreateMap<Room, RoomDTO>()
            .ForMember(room => room.Building, opt => opt.MapFrom(src => src.Building));
            CreateMap<RoomModel, Room>();
            CreateMap<RoomModel, RoomDTO>();
            CreateMap<CreateRoomCommand, Room>().ReverseMap();
            CreateMap<UpdateRoomCommand, Room>().ReverseMap();

            CreateMap<Notification, NotificationMessageDTO>();

            CreateMap<Meeting, MeetingDTO>()
              .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
              .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
              .ForMember(dest => dest.ConflictWith, opt => opt.MapFrom(src => src.ConflictWith));

            CreateMap<MeetingModel, MeetingDTO>();
            CreateMap<Meeting, Meeting>();
            // CreateMap<Meeting, MeetingDTO>().ReverseMap();
            CreateMap<CreateMeetingCommand, Meeting>().ReverseMap();
            CreateMap<UpdateMeetingCommand, Meeting>().ReverseMap();

            CreateMap<Tag, TagDTO>().ReverseMap();
        }
    }
}
