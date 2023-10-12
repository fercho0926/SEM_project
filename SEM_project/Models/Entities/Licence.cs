using SEM_project.Utils;
using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models.Entities
{
    public class Licence
    {
        public Guid LicenceId { get; set; }
        public string LicenceName { get; set; }

        public bool IsActive { get; set; }

        public string Version { get; set; }



        public IEnumerable<ComputerToLicence>? ComputerToLicences { get; set; }


    }
}
