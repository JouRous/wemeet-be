using System.Linq;
using API.DTO;
using API.Entities;
using API.Models;
using AutoMapper;

namespace API.Services
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<AppUser, UserDTO>()
        .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.AppUserTeams.Select(x => x.Team).ToList()))
        .ForAllMembers(options => options.Condition((src, dest, srcMembers) => srcMembers != null));
      CreateMap<RegisterModel, AppUser>();
    }
  }
}