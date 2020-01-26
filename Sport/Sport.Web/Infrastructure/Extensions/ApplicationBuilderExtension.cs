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
                var roles = new string[] {GlobalConstants.SuperAdministrator, GlobalConstants.PlayerRole, GlobalConstants.UmpireRole, GlobalConstants.AdministratorRole };

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

                         var superAdminRoleName = GlobalConstants.SuperAdministrator;
                         var playerRoleName = GlobalConstants.PlayerRole;
                         var umpireRoleName = GlobalConstants.UmpireRole;


                         var roleExists = await roleManager.RoleExistsAsync(superAdminRoleName);

                         if (!roleExists)
                         {
                             await roleManager.CreateAsync(new IdentityRole
                             {
                                 Name = superAdminRoleName
                             });
                         }



                         var superAdmin = await userManager.FindByNameAsync(superAdminRoleName);

                         if (superAdmin == null)
                         {
                             superAdmin = new User
                             {
                                 Email = "admin@abv.bg",
                                 UserName = "admin@abv.bg",
                                 FirstName = "Todor",
                                 LastName = "Nikolov"
                             };

                             
                             await userManager.CreateAsync(superAdmin, "admin12");

                             await userManager.AddToRoleAsync(superAdmin, playerRoleName);
                             await userManager.AddToRoleAsync(superAdmin, superAdminRoleName);
                             await userManager.AddToRoleAsync(superAdmin, umpireRoleName);
                         }
                     })
                     .Wait();
            }
            return app;
        }
    }
}
