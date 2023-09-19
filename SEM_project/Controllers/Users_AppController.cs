#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEM_project.Data;
using SEM_project.Models;
using SEM_project.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using SEM_project.Migrations;

namespace SEM_project.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class Users_AppController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        public string ReturnUrl { get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;


        public Users_AppController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        // GET: Users_App

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users_App.ToListAsync());
        }

        // GET: Users_App/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users_App = await _context.Users_App
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users_App == null)
            {
                return NotFound();
            }

            return View(users_App);
        }

        // GET: Users_App/Create
        public IActionResult Create()
        {
            var roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();

            var user = new Users_App
            {
                role = roles,
                AspNetUserId = "@"
            };

            return View(user);
        }


        // POST: Users_App/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "LastName,Identification,City,Neighborhood,Address,phone,AspNetUserId,Id,Name, Password,SelectedRoleId")]
            Users_App users_App)
        {
            ModelState.Remove("role"); // Remove the existing validation state if it exists
            ModelState.Remove("RoleName"); // Remove the existing validation state if it exists

            if (ModelState.IsValid)
            {
                var user = CreateUser();


                await _userStore.SetUserNameAsync(user, users_App.AspNetUserId, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, users_App.AspNetUserId, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, users_App.Password);

                if (result.Succeeded)
                {
                    users_App.IsActive = true;

                    var rolename = await _roleManager.FindByIdAsync(users_App.SelectedRoleId);

                    users_App.RoleName = rolename.Name;
                    users_App.Password = "hiddenData1*";
                    _context.Add(users_App);
                    await _context.SaveChangesAsync();

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


                    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    await _userManager.ConfirmEmailAsync(user, code);

                    var resultUserToRol = await AssignRoleToUser(userId, roleId: users_App.SelectedRoleId);


                    TempData["AlertMessage"] = "Usuario Creado Correctamente";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["ErrorMessage"] = "No se creo el usuario";
            //return RedirectToPage("./ForgotPassword");
            return RedirectToAction(nameof(Index));


            //return View(users_App);
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


        public async Task<bool> AssignRoleToUser(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user == null || role == null) return false;
            // Check if the user is already in the role
            var isInRole = await _userManager.IsInRoleAsync(user, role.Name);

            if (!isInRole)
            {
                // Add the user to the role
                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return false;
        }

        private async Task<bool> EditRoleToUser(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false; // User not found
            }

            // Get the roles the user currently has
            var userRoles = await _userManager.GetRolesAsync(user);

            // Calculate roles to be added and removed
            var roleToAdd = roleId;
            //var rolesToRemove = userRoles.Except(new List<string> { roleToAdd });

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Add the new role

            //await _userManager.AddToRoleAsync(user, roleToAdd);
            await AssignRoleToUser(userId, roleId);

            // Remove old roles


            return true; // Role updated successfully
        }


        // GET: Users_App/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users_App = await _context.Users_App.FindAsync(id);
            if (users_App == null)
            {
                return NotFound();
            }


            var user = await _userManager.FindByEmailAsync(users_App.AspNetUserId);
            var rolesselect = await _userManager.GetRolesAsync(user);


            var roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();

            users_App.role = roles;

            return View(users_App);
        }

        // POST: Users_App/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "LastName,Identification,City, AspNetUserId,Neighborhood,Address,phone,Id,Name,SelectedRoleId,RoleName")]
            Users_App users_App)
        {
            ModelState.Remove("role"); // Remove the existing validation state if it exists
            ModelState.Remove("Password"); // Remove the existing validation state if it exists
            ModelState.Remove("RoleName"); // Remove the existing validation state if it exists

            try
            {
                if (ModelState.IsValid)
                {
                    if (id != users_App.Id)
                    {
                        return NotFound();
                    }

                    // Ensure that you have a valid user
                    var user = await _userManager.FindByEmailAsync(users_App.AspNetUserId);
                    if (user == null)
                    {
                        return NotFound();
                    }


                    var roleName = await _roleManager.FindByIdAsync(users_App.SelectedRoleId);

                    users_App.RoleName = roleName.Name;

                    users_App.Password = "hiddenData1*";
                    _context.Update(users_App);
                    await _context.SaveChangesAsync();


                    // Update user role
                    await EditRoleToUser(user.Id, users_App.SelectedRoleId);

                    TempData["AlertMessage"] = "Se ha realizado la actualización de la información.";
                    //return RedirectToAction("Index", "Home");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                // Log the exception for debugging purposes
                // Handle the exception as needed (e.g., return an error view)
            }

            return View(users_App);
        }


        // GET: Users_App/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users_App = await _context.Users_App
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users_App == null)
            {
                return NotFound();
            }

            return View(users_App);
        }

        // POST: Users_App/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users_App = await _context.Users_App.FindAsync(id);

            users_App.IsActive = false;


            _context.Users_App.Update(users_App);
            await _context.SaveChangesAsync();

            TempData["AlertMessage"] = "Se ha realizado la Inactivacion del usuario";
            //return RedirectToAction("Index", "Home");
            return RedirectToAction(nameof(Index));
        }

        private bool Users_AppExists(int id)
        {
            return _context.Users_App.Any(e => e.Id == id);
        }

        public async Task<IActionResult> EditByMail(string mail)
        {
            if (User.Identity?.Name != mail)
            {
                return NotFound();
            }

            if (mail == null)
            {
                return NotFound();
            }

            var users_App = _context.Users_App.FirstOrDefault(x => x.AspNetUserId == mail);
            if (users_App == null)
            {
                return NotFound();
            }

            return View(users_App);
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}