using FlickFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlickFinder.Data
{
    public static class SeedIdentityData
    {
        private const string adminUser = "admin";
        private const string adminPassword = "Secret123$";
        private const string email = "admin@gmail.com";
        private const string adminRole = "Admin";


        public static async void EnsureIdentityDataPopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            //AppIdentityDbContext contextIdentity = app.ApplicationServices
            //  .CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>();

            if (!context.Database.GetMigrations().Any())
            {
                context.Database.Migrate();
            }

            //if (!contextIdentity.Database.GetMigrations().Any())
            //{
            //    context.Database.Migrate();
            //}

            UserManager<AppUser> userManager = app.ApplicationServices
                                 .CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();



            if (await userManager.FindByEmailAsync(email) == null)
            {
                if (await roleManager.FindByNameAsync(adminRole) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                }

                AppUser user = new AppUser
                {
                    UserName = adminUser,
                    Email = email
                };

                IdentityResult result = await userManager.CreateAsync(user, adminPassword);  
                
                if (result.Succeeded) 
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }

            }
        }
    }
}
