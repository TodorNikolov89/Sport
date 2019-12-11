namespace Sport.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Sport.Data;
    using Sport.Domain;
    using Sport.ViewModels.Identity;
    using Sport.Web.Infrastructure;
    using System.Linq;

    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class IdentityController : Controller
    {
        private readonly SportDbContext db;
        private readonly UserManager<User> userManager;

        public IdentityController(SportDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public IActionResult All()
        {
            var users = this.db
                .Users
                .OrderBy(u => u.Email)
                .Select(u => new ListUserViewModel
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email,
                })
                .ToList();

            return View(users);
        }
    }
}