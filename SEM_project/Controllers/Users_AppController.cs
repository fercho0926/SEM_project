#nullable disable
using System;
using System.Collections.Generic;
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



        public Users_AppController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // GET: Users_App

        [Authorize(Roles = "Admin")]
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
            return View();
        }

        // POST: Users_App/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "LastName,Identification,DateBirth,EnumCountries,City,Neighborhood,Address,phone,AspNetUserId,Id,Name, Password")]
            Users_App users_App)
        {


            if (ModelState.IsValid)
            {
                users_App.IsActive = true;
                _context.Add(users_App);
                await _context.SaveChangesAsync();

                //var returnUrl ??= Url.Content("~/");

                var user = CreateUser();

                //var password =users_App.EnumCountries.ToString();

                await _userStore.SetUserNameAsync(user, users_App.AspNetUserId, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, users_App.AspNetUserId, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, users_App.Password);

                if (result.Succeeded)
                {

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));



                    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                   await _userManager.ConfirmEmailAsync(user, code);


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction(nameof(Index));

                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(UrlToRegister);
                    }
                }



                return RedirectToAction(nameof(Index));
            }

            return View(users_App);
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

            return View(users_App);
        }

        // POST: Users_App/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "LastName,Identification,DateBirth,EnumCountries,City,Neighborhood,Address,phone,AspNetUserId,Id,Name")]
            Users_App users_App)
        {
            if (id != users_App.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //  var refer = await _context.ReferedByUser.OrderByDescending(x => x.Date).FirstOrDefaultAsync(x =>
                //x.ReferedUserId == users_App.AspNetUserId);

                //  if (refer != null)
                //  {
                //      refer.Accepted = true;
                //      var movement = new ReferedByUserMovement()
                //      {
                //          ReferedByUserId = refer.Id,
                //          Message = "Acepto Invitación",
                //          DateMovement = DateTime.Now,
                //          Status = EnumStatusBalance.PENDIENTE
                //      };
                //      _context.ReferedByUserMovement.Add(movement);

                _context.Update(users_App);
                await _context.SaveChangesAsync();


                //}
                //else
                //{
                //    return NotFound();
                //}


                TempData["AlertMessage"] =
                    $"Se ha realizado la actualizacion de la Informacion";
                return RedirectToAction("Index", "Home");
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