using Microsoft.AspNetCore.Mvc;
using SEM_project.Data;
using SEM_project.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEM_project.Utils;

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


            var allEmployees = _context.Employee.ToList();
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
                computer.InstaledApplications = "m";
                computer.ComputerHistory = new List<ComputerHistory>();
                computer.Licences = "null";
                computer.IsActive = true;
                _context.Add(computer);
                await _context.SaveChangesAsync();

                var userAuth = HttpContext.User;


                var newComputerHistory = new ComputerHistory()
                {
                    ComputerId = computer.ComputerId,
                    date = DateTime.Now,
                    Owner = "",
                    Action = (int)EnumAction.Creacion_Inicial,
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