using SEM_project.Utils;
using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models.Entities
{
    public class Employee
    {
        
        public Guid EmployeeId { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string? EmployeeName { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Identificación")]
        public string? IDNumber { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Cargo")] public EnumPosition EnumPosition { get; set; }
        [Display(Name = "Tipo de Vinculación")] public EnumAffiliation EnumAffiliation { get; set; }
        [Display(Name = "Sede")] public EnumLocation EnumLocation { get; set; }
        [Display(Name = "Piso")] public EnumFloor EnumFloor { get; set; }
        [Display(Name = "Subsecretaría")] public EnumSubdepartment EnumSubdepartment { get; set; }
        [Display(Name = "Grupo de trabajo")] public EnumWorkGroup EnumWorkGroup { get; set; }
       
        [Display(Name = "Placa")][MaxLength(10)] public string? AssignedEquipmentPlate { get; set; }
        public string? PhonePlate { get; set; }
        public string? PhoneModel { get; set; }
        public string? PhoneSerial { get; set; }
        public string? PhoneExtension { get; set; }
        public string? Observations { get; set; }
        public bool PersonalEquipment { get; set; }

        public ICollection<Computer>? Computers { get; set; }
    }
}