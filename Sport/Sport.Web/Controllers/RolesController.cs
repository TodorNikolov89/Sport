﻿namespace Sport.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Sport.Web.Infrastructure;

    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    [Route("roles")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
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



    }
}