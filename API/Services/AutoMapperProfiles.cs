using API.DTO;
using API.Entities;
using AutoMapper;

namespace API.Services
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<AppUser, UserDTO>();
      CreateMap<RegisterDTO, AppUser>();
    }
  }
}