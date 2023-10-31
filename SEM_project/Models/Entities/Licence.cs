using SEM_project.Utils;
using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models.Entities
{
    public class Licence
    {
        public Guid LicenceId { get; set; }
        [Display(Name = "Nombre")] public string LicenceName { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Versión")] public string Version { get; set; }
        public IEnumerable<ComputerToLicence>? ComputerToLicences { get; set; }


    }
}
