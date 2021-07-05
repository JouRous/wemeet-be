using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Types;
using System.IO;
using System.Reflection;
using Application.Utils;

namespace Infrastructure.Data
{
    class SeedUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
    }
    class SeedTeamModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int l_id { get; set; }
    }

    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppDbContext context)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var userData = await System.IO.File.ReadAllTextAsync(path + @"/Data/UserSeedData.json");
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
                    isFirstLogin = false,
                    UnsignedName = StringHelper.RemoveAccentedString(user.Fullname),
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

            var teamData = await System.IO.File.ReadAllTextAsync("Data/TeamSeedData.json");
            var teams = JsonSerializer.Deserialize<List<SeedTeamModel>>(teamData);

            foreach (var team in teams)
            {
                var leader = await userManager.Users.FirstOrDefaultAsync(u => u.Id == team.l_id);
                var _team = new Team
                {
                    Name = team.Name,
                    Description = team.Description,
                    Leader = leader,
                    // LeaderId = leader.Id,
                    AppUserTeams = new List<AppUserTeam>()
                };
                await context.Teams.AddAsync(_team);
                await context.SaveChangesAsync();
                // _team.AppUserTeams.Add(new AppUserTeam
                // {
                //     TeamId = _team.Id,
                //     AppUserId = _team.LeaderId
                // });
                await context.SaveChangesAsync();
            }

        }
    }
}