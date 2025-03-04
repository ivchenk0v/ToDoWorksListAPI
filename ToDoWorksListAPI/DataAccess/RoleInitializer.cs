using Microsoft.AspNetCore.Identity;
using ToDoWorksListAPI.Models;

namespace ToDoWorksListAPI.DataAccess
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            List<User> users =
            [
                new() { UserName = "admin", Email = "admin@gmail.com" },
                new() { UserName = "user", Email = "user@gmail.com" }
            ];

            List<string> roles = new List<string>{ "admin", "admin" };

            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            foreach (var user in users)
            {
                if (await userManager.FindByNameAsync(user.UserName) == null)
                {
                    IdentityResult result = await userManager.CreateAsync(user, user.UserName);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, user.UserName);
                    }
                }
            }

        }
    }
}
