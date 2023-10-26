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
            [Bind("LicenceName,Version, IsAssigned")]
            Licence licence)
        {
            ModelState.Remove("Licences");
            ModelState.Remove("LicenceStatus");

            if (ModelState.IsValid)
            {
                var licenceExist =
                    await _context.Licence.Where(x => x.LicenceName == licence.LicenceName).FirstOrDefaultAsync();
                if (licenceExist != null)
                {
                    TempData["ErrorMessage"] =
                        "Licencia ya existe."; // You can use TempData to show success messages.
                    return RedirectToAction(nameof(Index)); // Redirect to the licence list view.
                }


                licence.IsActive = true;
                _context.Add(licence);
                await _context.SaveChangesAsync();
                // Set a success message in TempData
                TempData["AlertMessage"] = "Licencia Creada Correctamente";

                // Redirect to the Index action if the model is valid
                return RedirectToAction(nameof(Index));
            }


            // Set an error message in TempData if model validation fails
            TempData["ErrorMessage"] = "No se creó licencia";

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


        // GET: Users_App/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var license = await _context.Licence
                .FindAsync(id);
            if (license == null)
            {
                return NotFound();
            }

            return View(license);
        }


        // POST: Users_App/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var licence = await _context.Licence.FindAsync(id);


            licence.IsActive = false;

            _context.Licence.Update(licence);
            await _context.SaveChangesAsync();


            TempData["AlertMessage"] = "Se ha realizado la Inactivación de la licencia";
            return RedirectToAction(nameof(Index));
        }
    }
}