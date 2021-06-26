using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.Types;

namespace API.Data
{
  class SeedUserModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string Role { get; set; }
  }

  public class Seed
  {
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
      if (await userManager.Users.AnyAsync())
      {
        return;
      }

      var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
      var users = JsonSerializer.Deserialize<List<SeedUserModel>>(userData);

      if (users == null)
      {
        return;
      }

      foreach (var user in users)
      {
        var password = "123123";
        var appUser = new AppUser
        {
          Email = user.Email.ToLower(),
          UserName = user.Email.ToLower(),
          Nickname = user.Email.Split("@")[0],
          Fullname = user.Fullname,
          isFirstLogin = true,
          UnsignedName = Utils.Utils.RemoveAccentedString(user.Fullname),
          Role = UserRoles.STAFF
        };

        if (!string.IsNullOrEmpty(user.Password))
        {
          password = user.Password;
        }

        if (!string.IsNullOrEmpty(user.Role))
        {
          appUser.Role = user.Role;
        }

        await userManager.CreateAsync(appUser, password);
      }

      var admin = new AppUser
      {
        Fullname = "Admin",
        Nickname = "Admin",
        UserName = "admin",
        Email = "admin",
        Role = UserRoles.ADMIN
      };

      await userManager.CreateAsync(admin, "123123");
    }
  }
}