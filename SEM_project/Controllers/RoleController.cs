﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SEM_project.Data;
using SEM_project.Models;
using SEM_project.Models.Entities;
using System.Security.Claims;

namespace SEM_project.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
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

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest(); // Return a bad request response if the ID is not provided.
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound(); // Return a not found response if the role doesn't exist.
            }

            // Create a RoleViewModel and set its properties
            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
                // Initialize other properties as needed
            };

            // Check if the role has the "Admin" claim
            var adminClaim =
                (await _roleManager.GetClaimsAsync(role)).FirstOrDefault(c =>
                    c.Type == "Permission" && c.Value == "Admin");

            // Check if the role has the "Computer" claim
            var computerClaim =
                (await _roleManager.GetClaimsAsync(role)).FirstOrDefault(c =>
                    c.Type == "Permission" && c.Value == "Computer");

            // Check if the role has the "Licence" claim
            var licenseClaim =
                (await _roleManager.GetClaimsAsync(role)).FirstOrDefault(c =>
                    c.Type == "Permission" && c.Value == "Licence");

            // Check if the role has the "Software" claim
            var softwareClaim =
                (await _roleManager.GetClaimsAsync(role)).FirstOrDefault(c =>
                    c.Type == "Permission" && c.Value == "Software");

            // Set the permission properties based on the claims
            roleViewModel.HasAdminPermissions = adminClaim != null;
            roleViewModel.HasComputerPermissions = computerClaim != null;
            roleViewModel.HasLicensesPermissions = licenseClaim != null;
            roleViewModel.HasSoftwarePermissions = softwareClaim != null;

            return View(roleViewModel);
        }


        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            // Create a new IdentityRole and set its properties based on the view model
            var role = new IdentityRole
            {
                Name = roleViewModel.Name,
                // Set other properties based on roleViewModel.HasAdminPermissions, etc.
            };

            // Use the role instance to create the role in the database
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                var claimsToAdd = new Dictionary<string, bool>
                {
                    { "Admin", roleViewModel.HasAdminPermissions },
                    { "Computer", roleViewModel.HasComputerPermissions },
                    { "Licence", roleViewModel.HasLicensesPermissions },
                    { "Software", roleViewModel.HasSoftwarePermissions }
                };

                foreach (var (permission, hasPermission) in claimsToAdd)
                {
                    if (hasPermission)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            // Find the role by its ID
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound(); // Return a not found response if the role doesn't exist.
            }

            // Map the role properties to the RoleViewModel
            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                // Map other role properties as needed
            };

            // Check for claims and set permissions
            roleViewModel.HasAdminPermissions = await HasClaimAsync(role, "Admin");
            roleViewModel.HasComputerPermissions = await HasClaimAsync(role, "Computer");
            roleViewModel.HasLicensesPermissions = await HasClaimAsync(role, "Licence");
            roleViewModel.HasSoftwarePermissions = await HasClaimAsync(role, "Software");

            return View(roleViewModel);
        }

        private async Task<bool> HasClaimAsync(IdentityRole role, string claimValue)
        {
            var claim = (await _roleManager.GetClaimsAsync(role))
                .FirstOrDefault(c => c.Type == "Permission" && c.Value == claimValue);

            return claim != null;
        }


        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel roleViewModel)
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
                // Remove existing claims associated with the role
                var existingClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in existingClaims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                // Add new claims based on roleViewModel permissions
                var claimsToAdd = new Dictionary<string, bool>
                {
                    { "Admin", roleViewModel.HasAdminPermissions },
                    { "Computer", roleViewModel.HasComputerPermissions },
                    { "Licence", roleViewModel.HasLicensesPermissions },
                    { "Software", roleViewModel.HasSoftwarePermissions }
                };

                foreach (var (permission, hasPermission) in claimsToAdd)
                {
                    if (hasPermission)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    }
                }

                return RedirectToAction("Index"); // Redirect to the role list on successful update.
            }

            // If the update fails, add model errors and return the view to show validation errors.
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }


            // If ModelState is not valid, return to the Edit view with validation errors.
            return View(roleViewModel);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return BadRequest(); // Return a bad request response if the ID is not provided.
            }

            var role = await _roleManager.FindByIdAsync(id);

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

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return BadRequest(); // Return a bad request response if the ID is not provided.
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound(); // Return a not found response if the role doesn't exist.
            }

            // Delete the role
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index"); // Redirect to the role list on successful deletion.
            }

            // If the deletion fails, add model errors and return the view to show validation errors.
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Delete", new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
                // Add more properties as needed
            });
        }
    }
}