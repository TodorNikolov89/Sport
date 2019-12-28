namespace Sport.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Sport.Domain;
    using Sport.ViewModels.Role;
    using Sport.Web.Infrastructure;

    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    [Route("roles")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        [Route(nameof(All))]
        public IActionResult All()
        {
            var roles = roleManager.Roles.ToList();

            return View(roles);
        }


        [Route(nameof(Create))]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction(nameof(All));
        }

        [Route(nameof(Delete) + "/{id}")]
        public IActionResult Delete(string id)
        {
            return this.View(nameof(Delete), id);
        }


        [Route(nameof(Destroy) + "/{id}")]
        public async Task<IActionResult> Destroy(string id)
        {
            var currentRole = await roleManager.FindByIdAsync(id);

            if (currentRole == null)
            {
                return NotFound();
            }

            await roleManager.DeleteAsync(currentRole);
            return RedirectToAction(nameof(All));
        }

    }
}