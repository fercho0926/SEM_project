using Microsoft.AspNetCore.Mvc;
using SEM_project.Data;
using Microsoft.AspNetCore.Identity;
using SEM_project.Services;

namespace SEM_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMailService mailService;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> SignInManager, ApplicationDbContext context, IMailService mailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = SignInManager;
            _context = context;
            this.mailService = mailService;
        }

        public IActionResult Index()
        {
            string? UserLogged = User.Identity?.Name.ToString();

            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed == null)
            {
                return RedirectToAction("Login", "Account", new { @mail = UserLogged });
            }

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            var activeComputers = _context.Computer.Where(x => x.IsActive== true && x.Unsubscribed == false);
            ViewBag.CountComputers = activeComputers.Count();


            var activeEmployees = _context.Employee.Where(x => x.IsActive);
            ViewBag.ActiveEmployees = activeEmployees.Count();

            var activeLicenses = _context.Licence.Where(x => x.IsActive);
            ViewBag.ActiveLicenses = activeLicenses.Count();

            var unassignedComputers = _context.Computer.Where(x => x.IsAssigned == false && x.IsActive==true && x.Unsubscribed == false);
            ViewBag.UnassignedComputers = unassignedComputers.Count();

            var UnsubscribedComputers = _context.Computer.Where(x => x.Unsubscribed);
            ViewBag.UnsubscribedComputers = UnsubscribedComputers.Count();

            var assignedComputers = _context.Computer.Where(x => x.IsAssigned);
            ViewBag.assignedComputers = assignedComputers.Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Working()
        {
            return View();
        }

        public IActionResult Support()
        {
            string UserLogged = User.Identity?.Name.ToString();
            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            return View();
        }

        public IActionResult Transfer()
        {
            string UserLogged = User.Identity?.Name.ToString();
            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            return View();
        }

        public IActionResult WebSite()
        {
            string UserLogged = User.Identity?.Name.ToString();
            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            return View();
        }

        public IActionResult Credential()
        {
            string UserLogged = User.Identity?.Name.ToString();
            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            return View();
        }

        public IActionResult News()
        {
            string UserLogged = User.Identity?.Name.ToString();
            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            return View();
        }

        public IActionResult Crypto()
        {
            string UserLogged = User.Identity?.Name.ToString();
            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            return View();
        }

        public IActionResult Bancolombia()
        {
            string UserLogged = User.Identity?.Name.ToString();
            var completed = _context.Users_App.FirstOrDefault(m => m.AspNetUserId == UserLogged);

            if (completed.Identification == "")
            {
                return RedirectToAction("EditByMail", "Users_App", new { @mail = UserLogged });
            }

            return View();
        }


        //public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        //{
        //    try
        //    {
        //        await mailService.SendEmailAsync(request);
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


        //public IActionResult Refer()
        //{
        //    return View();
        //}


        public IActionResult FinalRegister()
        {
            return RedirectToPage("~/Users_App/Index");
        }
    }
}