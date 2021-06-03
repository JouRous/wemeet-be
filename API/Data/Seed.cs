using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class Seed
  {
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
      if (await userManager.Users.AnyAsync())
      {
        return;
      }

      var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
      var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

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
        user.Email = user.Email.ToLower();
        user.UserName = user.Email.ToLower();

        await userManager.CreateAsync(user, "123123");
        await userManager.AddToRoleAsync(user, "Staff");
      }

      var admin = new AppUser
      {
        UserName = "admin",
        Email = "admin"
      };

      await userManager.CreateAsync(admin, "123123");
      await userManager.AddToRolesAsync(admin, new[] { "Admin", "Lead" });
    }
  }
}