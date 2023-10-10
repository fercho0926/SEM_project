using Microsoft.AspNetCore.Mvc;
using SEM_project.Data;
using SEM_project.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEM_project.Utils;
using System.Drawing;

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

            var history = _context.ComputerHistory.Where(x => x.ComputerId == id).OrderByDescending(x => x.date)
                .ToList();


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


            var allEmployees = _context.Employee.Where(x=>x.IsActive).ToList();
            ViewBag.EmployeeList =
                new SelectList(allEmployees, "EmployeeId",
                    "EmployeeName"); // Replace "UserId" and "UserName" with your actual property names.

            return View(activity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateActivity(
            [Bind(" ComputerId, Owner, Action, Details,EmployeeId")]
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


                if (computerHistory.Action == 9) //CAMBIO DE FUCNIONARIO
                {
                    var currentEmployeeToComputer =
                        _context.EmployeeToComputer.FirstOrDefault(x => x.ComputerId == computerHistory.ComputerId);

                    if (currentEmployeeToComputer != null)
                    {
                        _context.Remove(currentEmployeeToComputer);
                    }


                    var employeeTocomputer = new EmployeeToComputer()
                    {
                        ComputerId = computerHistory.ComputerId,
                        EmployeeId = computerHistory.EmployeeId
                    };
                    _context.Add(employeeTocomputer);


                    var computer = _context.Computer.FirstOrDefault(x => x.ComputerId == computerHistory.ComputerId);
                    computer.IsAssigned = true;
                    computer.EmployeeId = computerHistory.EmployeeId;

                    _context.Update(computer);

                    await _context.SaveChangesAsync();
                }

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
                var computerExist =
                    await _context.Computer.Where(x => x.Serial == computer.Serial).FirstOrDefaultAsync();


                if (computerExist != null)
                {
                    TempData["ErrorMessage"] =
                        "Equipo ya existe."; // You can use TempData to show success messages.
                    return RedirectToAction(nameof(Index)); // Redirect to the employee list view.
                }


                var employee = _context.Employee.Find(computer.EmployeeId);
                //computer.InstaledApplications = "m";
                computer.ComputerHistory = new List<ComputerHistory>();
                //computer.Licences = "null";
                computer.IsActive = true;
                _context.Add(computer);
                await _context.SaveChangesAsync();

                var userAuth = HttpContext.User;


                var newComputerHistory = new ComputerHistory()
                {
                    ComputerId = computer.ComputerId,
                    date = DateTime.Now,
                    Owner = "",
                    Action = (int)EnumAction.Creación_Inicial,
                    Performer = userAuth.Identity.Name,
                    Details = ""
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
                    public async Task<IActionResult> Edit(Guid? id)

        {

            var computer =  await  _context.Computer.FindAsync(id);

            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);


        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Computer computer)
        {


            var computerExist = await _context.Computer.Where(x=> x.Serial == computer.Serial && x.ComputerId != id).FirstOrDefaultAsync();

            if (computerExist != null)
            {
                TempData["ErrorMessage"] =
              "No se puede actualizar, el serial ya existe."; // You can use TempData to show success messages.
                return RedirectToAction(nameof(Index)); // Redirect to the employee list view.
            }

            if (id != computer.ComputerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the employee in the database.
                    _context.Update(computer);
                    await _context.SaveChangesAsync();
                    await AddComputerHistory(id, computer, (int)EnumAction.Actualizacion_De_Informacion, "Se actualiza la informacion del equipo");
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

            var users_App = await _context.Computer
                .FirstOrDefaultAsync(m => m.ComputerId == id);
            if (users_App == null)
            {
                return NotFound();
            }

            return View(users_App);
        }

        // POST: Users_App/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var computer = await _context.Computer.FindAsync(id);

            if (!computer.IsActive)
            {
                TempData["ErrorMessage"] = "El equipo ya se encuentra inactivo";
                return RedirectToAction(nameof(Index));
            }

            computer.IsActive = false;

            _context.Computer.Update(computer);

            await AddComputerHistory(id, computer, (int)EnumAction.Inactivar_Equipo,"");

            TempData["AlertMessage"] = "Se ha realizado la Inactivación del Equipo";
            //return RedirectToAction("Index", "Home");
            return RedirectToAction(nameof(Index));
        }

        private async Task AddComputerHistory(Guid id, Computer? computer, int action, string details)
        {
            var lasOwner = await _context.ComputerHistory.Where(x => x.ComputerId == id).OrderBy(c => c.date).Select(x => x.Owner).LastOrDefaultAsync();


            var userAuth = HttpContext.User;

            var newComputerHistory = new ComputerHistory()
            {
                ComputerId = computer.ComputerId,
                date = DateTime.Now,
                Owner = lasOwner,
                Action = action,
                Performer = userAuth.Identity.Name,
                Details = details
            };
            _context.Add(newComputerHistory);
            await _context.SaveChangesAsync();
        }
    }
}