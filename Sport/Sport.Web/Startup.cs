namespace Sport.Web
{
    using Sport.Web.Hubs;
    using Data;
    using Domain;
    using Profiles;
    using Services;
    using Services.Implementation;
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using AutoMapper;
    using Microsoft.AspNetCore.SignalR;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SportDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
             {
                 options.Password.RequireUppercase = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireDigit = false;
             })
                .AddDefaultUI()
                .AddEntityFrameworkStores<SportDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<ITournamentService, TournamentService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IMatchService, MatchService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
                cfg.AllowNullDestinationValues = true;
                cfg.AllowNullCollections = true;
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();

            services.AddMvc(option => option.EnableEndpointRouting = true)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);           
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDatabaseMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Error/Index?code={0}");
                app.UseExceptionHandler("/Home/Error/");
            }
            // app.UseMiddleware<ErrorHandlingMiddleware>();          
           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<SportHub>("/sportHub", options =>
                {
                    // 30Kb message buffer
                    options.ApplicationMaxBufferSize = 30 * 1024;
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
