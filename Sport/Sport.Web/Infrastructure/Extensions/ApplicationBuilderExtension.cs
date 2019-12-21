namespace Sport.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Sport.Data;
    using Sport.Domain;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtension
    {
        //Administrator access
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<SportDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roles = new string[] { GlobalConstants.PlayerRole, GlobalConstants.UmpireRole, GlobalConstants.AdministratorRole };

                Task
                     .Run(async () =>
                     {
                         foreach (var role in roles)
                         {
                             var roleExist = await roleManager.RoleExistsAsync(role);

                             if (!roleExist)
                             {
                                 await roleManager.CreateAsync(new IdentityRole(role));
                             }
                         }

                         var roleName = GlobalConstants.AdministratorRole;

                         var roleExists = await roleManager.RoleExistsAsync(roleName);

                         if (!roleExists)
                         {
                             await roleManager.CreateAsync(new IdentityRole
                             {
                                 Name = roleName
                             });
                         }

                         var adminUser = await userManager.FindByNameAsync(roleName);

                         if (adminUser == null)
                         {
                             adminUser = new User
                             {
                                 Email = "admin@abv.bg",
                                 UserName = "admin@abv.bg",
                                 FirstName = "Todor",
                                 LastName = "Nikolov"
                             };

                             await userManager.CreateAsync(adminUser, "admin12");

                             await userManager.AddToRoleAsync(adminUser, roleName);
                         }
                     })
                     .Wait();
            }
            return app;
        }
    }
}
