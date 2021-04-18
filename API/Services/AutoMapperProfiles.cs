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
      CreateMap<AppUser, UserDTO>();
      CreateMap<RegisterModel, AppUser>();
    }
  }
}