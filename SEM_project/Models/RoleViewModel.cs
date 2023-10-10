using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models
{
    public class RoleViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Nombre")] public string? Name { get; set; }

        public bool HasAdminPermissions { get; set; }
        public bool HasComputerPermissions { get; set; }
        public bool HasSoftwarePermissions { get; set; }
        public bool HasLicensesPermissions { get; set; }

        public bool HasEmployeePermissions { get; set; }

        public List<ClaimViewModel>? Claims { get; set; }
    }
}