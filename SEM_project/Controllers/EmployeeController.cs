using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEM_project.Data;
using SEM_project.Models.Entities;


namespace SEM_project.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;


        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComputerController
        public async Task<IActionResult> Index()
        {
            return View(_context.Employee.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            ModelState.Remove("Computers");


            if (ModelState.IsValid)
            {
                // You can add additional validation logic here if needed.

                employee.EmployeeId = Guid.NewGuid(); // Generate a new unique ID for the employee.


                var employeeExist = await _context.Employee.Where(x => x.IDNumber == employee.IDNumber)
                    .FirstOrDefaultAsync();

                if (employeeExist != null)
                {
                    TempData["ErrorMessage"] =
                        "Empleado ya existe."; // You can use TempData to show success messages.
                    return RedirectToAction(nameof(Index)); // Redirect to the employee list view.
                }


                // Add the employee to the database.
                _context.Add(employee);
                await _context.SaveChangesAsync();

                TempData["AlertMessage"] =
                    "Empleado Creado correctamente."; // You can use TempData to show success messages.
                return RedirectToAction(nameof(Index)); // Redirect to the employee list view.
            }

            return View(employee);
        }


        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the employee by ID.
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the employee in the database.
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["AlertMessage"] =
                    "Se ha realizado la actualización de la información."; // You can use TempData to show success messages.
                return RedirectToAction(nameof(Index)); // Redirect to the employee list view.
            }

            return View(employee);
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _context.Employee.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}