using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SEM_project.Data;
using SEM_project.Models;
using System.Text;
using SEM_project.Models.Entities;

namespace SEM_project.Controllers
{
    public class ComputerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComputerController
        public async Task<IActionResult> Index()
        {
            return View(_context.Computer.ToList());
        }

        // GET: ComputerController/Details/5
        public ActionResult Details(Guid id)
        {
            var details = _context.Computer.Find(id);

            var history = _context.ComputerHistory.Where(x => x.ComputerId == id).ToList();
            details.ComputerHistory = history;
            return View(details);
        }

        // GET: ComputerController/Create
        public ActionResult Create()
        {
            return View();
        }


        public ActionResult CreateActivity(Guid id)
        {
            var currentOwner = _context.ComputerHistory
                .Where(x => x.ComputerId == id)
                .OrderByDescending(x => x.date) // Assuming the date field is named 'Date'
                .FirstOrDefault();
            var activity = new ComputerHistory()
            {
                ComputerId = id,
                Owner = currentOwner.Owner
            };

            return View(activity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateActivity(
            [Bind(" ComputerId, Owner, Action, Details")]
            ComputerHistory computerHistory)
        {
            // Remove the existing validation state for these properties if it exists
            ModelState.Remove("ComputerHistoryId");
            ModelState.Remove("Date");
            ModelState.Remove("Performer");

            if (ModelState.IsValid)
            {
                var userAuth = HttpContext.User;

                computerHistory.date = DateTime.Now;
                computerHistory.Performer = userAuth.Identity.Name;
                _context.Add(computerHistory);
                await _context.SaveChangesAsync();

                TempData["AlertMessage"] = "Actividad Creada Correctamente";

                // Redirect to the Index action if the model is valid
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Informacion Incorrecta";
            return RedirectToAction(nameof(Index));
        }


        //// POST: ComputerController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // POST: Users_App/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ComputerId, Serial, Reference, Processer, Ram, HardDisk, OperativeSystem, Model")]
            Computer computer)
        {
            // Remove the existing validation state for these properties if it exists
            ModelState.Remove("ComputerHistory");
            ModelState.Remove("InstaledApplications");
            ModelState.Remove("Licences");

            if (ModelState.IsValid)
            {
                computer.InstaledApplications = "m";
                computer.ComputerHistory = new List<ComputerHistory>();
                computer.Licences = "null";
                _context.Add(computer);
                await _context.SaveChangesAsync();

                var userAuth = HttpContext.User;


                var newComputerHistory = new ComputerHistory()
                {
                    ComputerId = computer.ComputerId,
                    date = DateTime.Now,
                    Owner = "sin propietario",
                    Action = "Creacion Inicial equipo",
                    Performer = userAuth.Identity.Name,
                    Details = "sin novedad"
                };
                _context.Add(newComputerHistory);
                await _context.SaveChangesAsync();


                // Set a success message in TempData
                TempData["AlertMessage"] = "Equipo Creado Correctamente";

                // Redirect to the Index action if the model is valid
                return RedirectToAction(nameof(Index));
            }

            // Set an error message in TempData if model validation fails
            TempData["ErrorMessage"] = "No se creó el Equipo";

            // Redirect to the Index action in both success and error cases
            return RedirectToAction(nameof(Index));
        }


        // GET: ComputerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ComputerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ComputerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ComputerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}