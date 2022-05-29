using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Quiz_Application.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Services.Utilities.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<Candidate> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            
            var username = configuration["Data:AdminUser:username"];
            var email = configuration["Data:AdminUser:email"];
            var password = configuration["Data:AdminUser:password"];
            var role = configuration["Data:AdminUser:role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));

                var user = new Candidate()
                {
                    UserName = username,
                    Email = email,
                    Name = "admin",
                    Candidate_ID = "admin",
                    EmailConfirmed = true
                };


                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
