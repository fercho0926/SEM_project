using SEM_project.Utils;
using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models.Entities
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }

        [Required] [Display(Name = "Nombre")] public string? EmployeeName { get; set; }

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
       
        [Display(Name = "campo_pendiente latitud")][MaxLength(10)] public string? AssignedEquipmentPlate { get; set; }
        [Display(Name = "Placa de teléfono")][MaxLength(9)] public string? PhonePlate { get; set; }
        [Display(Name = "Modelo Teléfono")] public string? PhoneModel { get; set; }
        [Display(Name = "campo_pendiente longitud")] public string? PhoneSerial { get; set; }
        [Display(Name = "Extensión")] public string? PhoneExtension { get; set; }
        [Display(Name = "Observaciones")] public string? Observations { get; set; }
        [Display(Name = "Equipo personal")] public bool PersonalEquipment { get; set; }
        public bool IsActive { get; set; }


        public ICollection<Computer>? Computers { get; set; }
    }
}