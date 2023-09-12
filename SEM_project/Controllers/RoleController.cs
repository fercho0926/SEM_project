using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SEM_project.Models;

namespace SEM_project.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        // List roles
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            return View(roles);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest(); // Return a bad request response if the ID is not provided.
            }


            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);


            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            if (roleViewModel == null)
            {
                return NotFound(); // Return a not found response if the role doesn't exist.
            }


            return View(roleViewModel);
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(role);
        }

        // GET: RoleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(roleViewModel.Id);

                if (role == null)
                {
                    return NotFound(); // Return a not found response if the role doesn't exist.
                }

                role.Name = roleViewModel.Name;
                // Update other role properties as needed

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); // Redirect to the role list on successful update.
                }

                // If the update fails, add model errors and return the view to show validation errors.
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If ModelState is not valid, return to the Edit view with validation errors.
            return View(roleViewModel);
        }

        // GET: RoleController/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest(); // Return a bad request response if the ID is not provided.
            }

            var role = _roleManager.FindByIdAsync(id)
                .Result; // Assuming _roleManager is an instance of RoleManager<IdentityRole>.

            if (role == null)
            {
                return NotFound(); // Return a not found response if the role doesn't exist.
            }

            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
                // Add more properties as needed
            };

            return View(roleViewModel);
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(); // Return a bad request response if the ID is not provided.
                }

                var role = _roleManager.FindByIdAsync(id);

                if (role == null)
                {
                    return NotFound(); // Return a not found response if the role doesn't exist.
                }

                //// Delete the role using the RoleManager
                //var result =  _roleManager.DeleteAsync(role);

                //if (result.Succeeded)
                //{
                //    return RedirectToAction("Index"); // Redirect to the role list on successful deletion.
                //}
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}