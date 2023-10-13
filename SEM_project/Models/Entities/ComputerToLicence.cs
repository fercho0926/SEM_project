using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models.Entities
{
    public class ComputerToLicence
    {
        public Guid ComputerToLicenceId { get; set; }
        [Required] public Guid ComputerId { get; set; }
        [Required] public Guid LicenceId { get; set; }
        public Computer Computer { get; set; }
        public Licence Licence { get; set; }

    }
}
