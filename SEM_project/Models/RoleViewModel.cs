namespace SEM_project.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool HasAdminPermissions { get; set; }
        public bool HasComputerPermissions { get; set; }
        public bool HasSoftwarePermissions { get; set; }
        public bool HasLicensesPermissions { get; set; }

        public List<ClaimViewModel> Claims { get; set; }
    }
}