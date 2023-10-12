using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEM_project.Data;
using SEM_project.Models.Entities;
using SEM_project.Utils;


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

                employee.IsActive = true;
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



            var employeeExist = await _context.Employee.Where(x => x.IDNumber == employee.IDNumber && x.EmployeeId != id).FirstOrDefaultAsync();

            if (employeeExist != null)
            {
                TempData["ErrorMessage"] =
              "No se puede actualizar, el empleado ya existe."; // You can use TempData to show success messages.
                return RedirectToAction(nameof(Index)); // Redirect to the employee list view.
            }



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

            var assignedComputers = _context.EmployeeToComputer.Where(x => x.EmployeeId == id).ToList();

            var listComputers = new List<Computer>();

            foreach (var assignedItem in assignedComputers)
            {
                var result = _context.Computer.FirstOrDefault(x => x.ComputerId == assignedItem.ComputerId);
                listComputers.Add(result);
            }

            employee.Computers = listComputers;

            if (employee == null)
            {
                return NotFound();
            }


            var allComputers
                = _context.Computer.Where(x => x.IsActive && x.IsAssigned == false).ToList();


            ViewBag.ComputerList =
                new SelectList(allComputers, "ComputerId",
                    "Serial");


            //var allEmployees = _context.Employee.ToList();
            //ViewBag.EmployeeList =
            //    new SelectList(allEmployees, "EmployeeId",
            //        "EmployeeName"); // Replace "UserId" and "UserName" with your actual property names.


            return View(employee);
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }


        [HttpPost]
        public IActionResult AddUserToComputer(Guid employeeId, Guid selectedOption)
        {
            var newUserToComputer = new EmployeeToComputer
            {
                EmployeeId = employeeId,
                ComputerId = selectedOption
            };

            _context.Add(newUserToComputer);


            var editComputer = _context.Computer.Where(c => c.ComputerId == selectedOption).First();

            editComputer.IsAssigned = true;
            editComputer.EmployeeId = employeeId;
            _context.Update(editComputer);


            var userAuth = HttpContext.User;
            var employee = _context.Employee.Where(x => x.EmployeeId == employeeId).First();

            var newComputerHistory = new ComputerHistory()
            {
                ComputerId = selectedOption,
                date = DateTime.Now,
                Owner = employee.EmployeeName, // GET THE NAME
                Action = (int)EnumAction.Asignación_Equipo,
                Performer = userAuth.Identity.Name,
                Details = "",
                EmployeeId = employeeId
            };
            _context.Add(newComputerHistory);


            _context.SaveChanges();

            // Your logic here...
            // You can use the employeeId and selectedOption values for processing.

            // For demonstration purposes, you can return a JSON response.
            var result = new
            {
                EmployeeId = employeeId,
                SelectedOption = selectedOption,
            };

            TempData["AlertMessage"] = "Se asigna Equipo a Funcionario";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Users_App/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var employee = await _context.Employee.FindAsync(id);

            if (!employee.IsActive)
            {
                TempData["ErrorMessage"] = "El Funcionario ya se encuentra inactivo";
                return RedirectToAction(nameof(Index));
            }

            employee.IsActive = false;

            _context.Employee.Update(employee);
            await _context.SaveChangesAsync();

            TempData["AlertMessage"] = "Se ha realizado la Inactivación del Funcionario";
            //return RedirectToAction("Index", "Home");
            return RedirectToAction(nameof(Index));
        }

    }
}