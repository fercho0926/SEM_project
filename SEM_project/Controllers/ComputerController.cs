using Microsoft.AspNetCore.Mvc;
using SEM_project.Data;
using SEM_project.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEM_project.Utils;
using System.Drawing;
using SEM_project.Models;
using System.Text;
using System.Globalization;

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

            var history = _context.ComputerHistory.Where(x => x.ComputerId == id).OrderBy(x => x.date)
                .ToList();

            var licences = _context.ComputerToLicence.Where(x => x.ComputerId == id).Include(x => x.Licence).ToList();

            var allLicences = _context.Licence.Where(x => x.IsActive).ToList();

            ViewBag.LicenceList =
                new SelectList(allLicences, "LicenceId",
                    "LicenceName");

            details.ComputerHistory = history;
            details.ComputerToLicence = licences;
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


            var allEmployees = _context.Employee.Where(x => x.IsActive).ToList();
            ViewBag.EmployeeList =
                new SelectList(allEmployees, "EmployeeId",
                    "EmployeeName"); // Replace "UserId" and "UserName" with your actual property names.

            return View(activity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignLicence(ComputerToLicence licence)
        {
            ModelState.Remove("licence.Computer");
            ModelState.Remove("licence.Licence");


            var licenceExist = await _context.ComputerToLicence
                .Where(x => x.LicenceId == licence.LicenceId && x.ComputerId == licence.ComputerId)
                .AnyAsync();

            if (licenceExist)
            {
                TempData["ErrorMessage"] = "Ya tiene asignada esta licencia";
                return RedirectToAction("Details", "Computer", new { id = licence.ComputerId });
            }

            if (ModelState.IsValid)
            {
                await _context.AddAsync(licence);
                await _context.SaveChangesAsync();

                var licenceName = await _context.Licence.FindAsync(licence.LicenceId);

                var licenceWithDetail = $"Instalación {licenceName.LicenceName}";

                await AddComputerHistory(licence.ComputerId, (int)EnumAction.Instalación_Software_Licencia,
                   licenceWithDetail);

                TempData["AlertMessage"] = "Licencia Asignada Correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "No se puede asignar";
            }

            // If ModelState is not valid, handle the validation errors or return a view
            return RedirectToAction("Details", "Computer", new { id = licence.ComputerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deleteAssignedUser(
            [Bind(" ComputerId, Owner, Action, Details,EmployeeId")]
            ComputerHistory computerHistory)
        {
            TempData["AlertMessage"] = "Actividad Creada Correctamente";

            // Redirect to the Index action if the model is valid
            return RedirectToAction(nameof(Index));
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
            ModelState.Remove("EmployeeId");

            if (ModelState.IsValid)
            {
                var userAuth = HttpContext.User;

                computerHistory.date = DateTime.Now;
                computerHistory.Performer = userAuth.Identity.Name;


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


                    if (computerHistory.EmployeeId == Guid.Empty)
                    {
                        computer.IsAssigned = false;
                        computerHistory.Owner = String.Empty;
                        computerHistory.Details = "Se Elimina Asignación de usuario";
                    }
                    else
                    {
                        computer.IsAssigned = true;
                        computer.EmployeeId = computerHistory.EmployeeId;
                    }


                    _context.Update(computer);

                    await _context.SaveChangesAsync();
                }

                if (computerHistory.Action == 10) //DAR DE BAJA
                {
                    var computer = _context.Computer.FirstOrDefault(x => x.ComputerId == computerHistory.ComputerId);
                    computer.IsAssigned = false;
                    computer.EmployeeId = null;
                    computer.Unsubscribed = true;
                    computerHistory.Owner = String.Empty;


                    var emploToComputer =
                        _context.EmployeeToComputer.FirstOrDefault(x => x.ComputerId == computerHistory.ComputerId);

                    _context.Remove(emploToComputer);
                }

                _context.Add(computerHistory);
                await _context.SaveChangesAsync();


                TempData["AlertMessage"] = "Actividad Creada Correctamente";

                // Redirect to the Index action if the model is valid

                return RedirectToAction("Details", "Computer", new { id = computerHistory.ComputerId });

                //return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Informacion Incorrecta";
            return RedirectToAction(nameof(Index));
        }


    
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ComputerId, Serial, Reference, Processer, Ram, HardDisk, OperativeSystem, Model, EnumLocation, LocationFloor, Value")]
            Computer computer)
        {
            // Remove the existing validation state for these properties if it exists
            ModelState.Remove("ComputerHistory");
            ModelState.Remove("InstaledApplications");
            ModelState.Remove("Licences");
            ModelState.Remove("Latitud");
            ModelState.Remove("Longitud");
            ModelState.Remove("LocationName");




            CultureInfo culture = new CultureInfo("es-ES"); 
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

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
                var name = "";
                var latitud = 0f;
                var longitud = 0f;

               switch (computer.EnumLocation) {
                    case EnumLocation.NA:
                        name = "NA";
                            break;
                    case EnumLocation.Bulevar:
                        name = "Bulevar";
                        latitud = 6.24537173186433f;
                        longitud = -75.5711242481222f;
                        break;
                    case EnumLocation.Carre:
                        name = "Carre";
                        latitud = 6.24597f;
                        longitud = -75.57139f;
                        break;
                    case EnumLocation.ViveroSf:
                        name = "ViveroSf";
                        latitud = 6.2092220071554705f;
                        longitud = -75.57759124759058f;
                        break;
                }


                var floor = "";
                switch (computer.LocationFloor)
                {
                    case "0":
                        floor = "Primero";
                        break;
                    case "1":
                        floor = "Segundo";
                        break;
                    case "2":
                        floor = "Tercero";
                        break;
                    case "3":
                        floor = "Cuarto";
                        break; 
                    case "4":
                        floor = "Quinto";
                        break;
                    case "5":
                        floor = "Quinto";
                        break;
                    case "6":
                        floor = "NA";
                        break;
                }

                computer.LocationName = name;
                computer.LocationFloor = floor;
                computer.Latitud = latitud;
                computer.Longitud = longitud;




                //var employee = _context.Employee.Find(computer.EmployeeId);
                computer.ComputerHistory = new List<ComputerHistory>();
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
                    Details = "Creación inicial"
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
            var computer = await _context.Computer.FindAsync(id);

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
            ModelState.Remove("LocationName");

            var computerExist = await _context.Computer.Where(x => x.Serial == computer.Serial && x.ComputerId != id)
                .FirstOrDefaultAsync();

            if (computerExist != null)
            {
                TempData["ErrorMessage"] =
                    "No se puede actualizar, el serial ya existe.";
                return RedirectToAction(nameof(Index));
            }

            if (id != computer.ComputerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldData = await _context.Computer.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ComputerId == computer.ComputerId);

                    if (oldData != null)
                    {
                        // Create a list to store the changes
                        var changes = new List<string>();

                        // Use reflection to compare property values
                        foreach (var property in typeof(Computer).GetProperties())
                        {
                            var oldValue = property.GetValue(oldData);
                            var newValue = property.GetValue(computer);

                            if (!object.Equals(oldValue, newValue))
                            {
                                switch (property.Name)
                                {
                                    case "Processer":
                                        changes.Add($" Procesador: {oldValue} -> {newValue} ,  ");

                                        break;

                                    case "Ram":
                                        changes.Add($" Memoria Ram: {oldValue} -> {newValue} ,  ");

                                        break;
                                    case "HardDisk":
                                        changes.Add($" Disco Duro: {oldValue} -> {newValue} ,  ");

                                        break;
                                    case "OperativeSystem":
                                        changes.Add($" Sistema Operativo: {oldValue} -> {newValue} ,  ");

                                        break;
                                    case "Model":
                                        changes.Add($" Tipo de Equipo: {oldValue} -> {newValue} ,  ");

                                        break;
                                    default:
                                        changes.Add($"{property.Name}: {oldValue} -> {newValue} ,  ");

                                        break;
                                }
                            }
                        }

                        var changesString = string.Join(Environment.NewLine, changes);


                        var name = "";
                        var latitud = 0f;
                        var longitud = 0f;

                        switch (computer.EnumLocation)
                        {
                            case EnumLocation.NA:
                                name = "NA";
                                break;
                            case EnumLocation.Bulevar:
                                name = "Bulevar";
                                latitud = 6.24537173186433f;
                                longitud = -75.5711242481222f;
                                break;
                            case EnumLocation.Carre:
                                name = "Carre";
                                latitud = 6.24597f;
                                longitud = -75.57139f;
                                break;
                            case EnumLocation.ViveroSf:
                                name = "ViveroSf";
                                latitud = 6.2092220071554705f;
                                longitud = -75.57759124759058f;
                                break;
                        }
                        computer.LocationName = name;
                        computer.Latitud = latitud;
                        computer.Longitud = longitud;



                        _context.Update(computer);
                        await _context.SaveChangesAsync();


                        await AddComputerHistory(id, (int)EnumAction.Actualización_De_Información, changesString);


                        TempData["AlertMessage"] = "Se ha realizado la actualización de la información.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
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

            var computer = await _context.Computer
                .FirstOrDefaultAsync(m => m.ComputerId == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }


        public async Task<RedirectToActionResult> DeleteLicencePerComputer(Guid id)
        {
            var computerToLicence = await _context.ComputerToLicence.FindAsync(id);
            _context.Remove(computerToLicence);

            var license = await _context.Licence.FindAsync(computerToLicence.LicenceId);



            var licenceWithDetail = $"Eliminación Licencia {license.LicenceName}";

            await AddComputerHistory(computerToLicence.ComputerId, (int)EnumAction.Eliminacion_Software_Licencia,
                licenceWithDetail);
            
            await _context.SaveChangesAsync();

            TempData["AlertMessage"] = "Se elimina licencia Correctamente";

            return RedirectToAction("Details", "Computer", new { id = computerToLicence.ComputerId });
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
            _context.SaveChangesAsync();

            await AddComputerHistory(id, (int)EnumAction.Inactivar_Equipo, "");

            TempData["AlertMessage"] = "Se ha realizado la Inactivación del Equipo";
            //return RedirectToAction("Index", "Home");
            return RedirectToAction(nameof(Index));
        }

        private async Task AddComputerHistory(Guid computerId, int action, string details)
        {
            var lasOwner = await _context.ComputerHistory.Where(x => x.ComputerId == computerId).OrderBy(c => c.date)
                .Select(x => x.Owner).LastOrDefaultAsync();


            var userAuth = HttpContext.User;

            var newComputerHistory = new ComputerHistory()
            {
                ComputerId = computerId,
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