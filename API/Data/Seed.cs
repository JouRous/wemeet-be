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

namespace API.Data
{
  class SeedUserModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
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

      var roles = new List<AppRole>
      {
        new AppRole{Name = "Staff"},
        new AppRole{Name = "Lead"},
        new AppRole{Name = "Admin"}
      };

      foreach (var role in roles)
      {
        await roleManager.CreateAsync(role);
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
          UnsignedName = Utils.Utils.RemoveAccentedString(user.Fullname)
        };

        if (!string.IsNullOrEmpty(user.Password))
        {
          password = user.Password;
        }

        await userManager.CreateAsync(appUser, password);
        await userManager.AddToRoleAsync(appUser, "Staff");
      }

      var admin = new AppUser
      {
        Fullname = "Admin",
        Nickname = "Admin",
        UserName = "admin",
        Email = "admin"
      };

      await userManager.CreateAsync(admin, "123123");
      await userManager.AddToRolesAsync(admin, new[] { "Admin", "Lead" });
    }
  }
}