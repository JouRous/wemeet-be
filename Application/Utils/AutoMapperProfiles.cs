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
            CreateMap<AppUser, UserBaseDTO>().ReverseMap();
            CreateMap<AppUser, AdminUserDTO>().ReverseMap();
            CreateMap<AppUser, UserDTO>()
              .ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));
            CreateMap<CreateUserCommand, AppUser>().ReverseMap();
            CreateMap<UpdateUserCommand, AppUser>().ReverseMap();

            CreateMap<AppUser, UserWithTeamDTO>()
              .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.Team)))
              .ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));

            CreateMap<AppUser, UserWithTeamUsersDTO>()
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.Team)))
            .ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));

            CreateMap<Team, TeamBaseDTO>().ReverseMap();
            CreateMap<Team, TeamDTO>()
              .ForMember(dest => dest.Leader, opt => opt.MapFrom(src => src.Leader));
            CreateMap<Team, TeamWithUserDTO>()
              .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.User)))
              .ForMember(dest => dest.Leader, opt => opt.MapFrom(src => src.Leader));
            CreateMap<CreateTeamCommand, Team>().ReverseMap();
            CreateMap<UpdateTeamCommand, Team>().ReverseMap();

            CreateMap<Building, BuildingDTO>();
            CreateMap<Building, BuildingBaseDTO>().ReverseMap();
            CreateMap<CreateBuildingCommand, Building>().ReverseMap();
            CreateMap<UpdatebuildingCommand, Building>().ReverseMap();

            CreateMap<Room, RoomDTO>()
            .ForMember(room => room.Building, opt => opt.MapFrom(src => src.Building));
            CreateMap<Room, RoomBaseDTO>().ReverseMap();
            CreateMap<CreateRoomCommand, Room>().ReverseMap();
            CreateMap<UpdateRoomCommand, Room>().ReverseMap();

            CreateMap<Notification, NotificationMessageDTO>();

            CreateMap<Tag, TagDTO>().ReverseMap();
            CreateMap<FileEntity, FileDTO>().ReverseMap();

            CreateMap<Meeting, MeetingDTO>()
              .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
              .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.MeetingTags.Select(x => x.Tag)))
              .ForMember(dest => dest.UserInMeeting,
                  opt => opt.MapFrom(src => src.ParticipantMeetings.Select(x => x.Participant)))
              .ForMember(dest => dest.Files, opt => opt.MapFrom(
                  src => src.MeetingFiles.Select(x => x.FileEntity)
              ))
              .ForMember(dest => dest.Teams, opt => opt.MapFrom(
                src => src.MeetingTeams.Select(x => x.Team)
              ))
              .ForMember(dest => dest.Creator, opt => opt.MapFrom(
                src => src.MeetingTeams.Select(x => x.Team).First().Leader
              ))
              .ReverseMap();
            CreateMap<Meeting, MeetingBaseDTO>()
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(
                src => src.MeetingTeams.Select(x => x.Team)
              ))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(
                src => src.MeetingTeams.Select(x => x.Team).First().Leader
              ))
            .ReverseMap();
            CreateMap<CreateMeetingCommand, Meeting>().ReverseMap();
            CreateMap<UpdateMeetingCommand, Meeting>().ReverseMap();
        }
    }
}
