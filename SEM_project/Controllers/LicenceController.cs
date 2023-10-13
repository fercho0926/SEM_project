using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEM_project.Data;
using SEM_project.Models.Entities;
using SEM_project.Utils;

namespace SEM_project.Controllers
{
    public class LicenceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LicenceController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(_context.Licence.ToList());
        }



        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("LicenceName, LicenceStatus, Version, IsAssigned")]
            Licence licence)
        {

            ModelState.Remove("Licences");

            if (ModelState.IsValid)
            {

                _context.Add(licence);
                await _context.SaveChangesAsync();
                // Set a success message in TempData
                TempData["AlertMessage"] = "Licencia Creada Correctamente";

                // Redirect to the Index action if the model is valid
                return RedirectToAction(nameof(Index));
            }

            // Set an error message in TempData if model validation fails
            TempData["ErrorMessage"] = "No se creó el Equipo";

            // Redirect to the Index action in both success and error cases
            return RedirectToAction(nameof(Index));
        }



        public ActionResult Details(Guid id)
        {
            var details = _context.Licence.Find(id);

            //var history = _context.ComputerHistory.Where(x => x.ComputerId == id).OrderByDescending(x => x.date)
            //    .ToList();


            //details.ComputerHistory = history;
            return View(details);
        }

        // GET: ComputerController/Edit/5
        public async Task<IActionResult> Edit(Guid? id)

        {

            var licence = await _context.Licence.FindAsync(id);

            if (licence == null)
            {
                return NotFound();
            }

            return View(licence);



        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Licence licence)
        {


            if (id != licence.LicenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    return NotFound();

                }

                TempData["AlertMessage"] =
                    "Se ha realizado la actualización de la información."; // You can use TempData to show success messages.
                return RedirectToAction(nameof(Index)); // Redirect to the employee list view.
            }

            return View();
        }


    }
}
