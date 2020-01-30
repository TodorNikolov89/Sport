namespace Sport.Web.Controllers
{
    using Data;
    using Domain;
    using Infrastructure;
    using ViewModels.Identity;
    using ViewModels.Role;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;  
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    [Route("identity")]
    public class IdentityController : Controller
    {
        private readonly SportDbContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityController(SportDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction(nameof(Roles), new { Id = userId });
        }



        [Route(nameof(All))]
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
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToList();


            return View(users);
        }

        [Route(nameof(Roles) + "/{id}")]
        public async Task<IActionResult> Roles(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await this.userManager.GetRolesAsync(user);

            return View(new UserWithRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles
            });

        }

        [Route(nameof(Create))]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Route(nameof(Create))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            ViewData["roles"] = roleManager.Roles.ToList();
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.userManager.CreateAsync(new User
            {
                Email = model.Email,
                UserName = model.Email,

            }, model.Password);


            if (result.Succeeded)
            {
                this.TempData["SuccessMessage"] = $"User with e-mail {model.Email} created";
                return RedirectToAction(nameof(All));
            }
            else
            {
                AddModelErrors(result);
                return View(model);
            }
        }

        [Route(nameof(ChangePassword) + "/{id}")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            return View(new IdentityChangePasswordViewModel
            {
                Email = user.Email
            });
        }


        [HttpPost]
        [Route(nameof(ChangePassword) + "/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, IdentityChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await this.userManager.FindByIdAsync(id);
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var result = await this.userManager.ResetPasswordAsync(user, token, model.Password);


            if (result.Succeeded)
            {
                this.TempData["SuccessMessage"] = $"Password changed for user {user.Email}";
                return RedirectToAction(nameof(All));
            }
            else
            {
                AddModelErrors(result);
                return View(model);
            }
        }

        [Route(nameof(Delete) + "/{id}")]
        public IActionResult Delete(string id)
        {
            return View("Delete", id);
        }


        [Route(nameof(Destroy) + "/{id}")]
        public async Task<IActionResult> Destroy(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await userManager.DeleteAsync(user);

            return RedirectToAction(nameof(All));
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}