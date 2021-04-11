using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class Seed
  {
    public static async Task SeedUsers(AppDbContext context)
    {
      if (await context.Users.AnyAsync())
      {
        return;
      }

      var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
      var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

      if (users == null)
      {
        return;
      }

      foreach (var user in users)
      {
        using var hmac = new HMACSHA512();

        user.Username = user.Username.ToLower();
        user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("123123"));
        user.PasswordSalt = hmac.Key;

        await context.AddAsync(user);
      }

      await context.SaveChangesAsync();
    }
  }
}