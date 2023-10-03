using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models.Entities
{
    public class Computer
    {
        public Guid ComputerId { get; set; }
        [Display(Name = "Placa")] public string? Serial { get; set; }
        [Display(Name = "Referencia")] public string? Reference { get; set; }
        [Display(Name = "Procesador")] public string? Processer { get; set; }
        public string? Ram { get; set; }
        public string? HardDisk { get; set; }
        [Display(Name = "Sistema Operativo")] public string? OperativeSystem { get; set; }
        [Display(Name = "Tipo Equipo")] public string? Model { get; set; }
        public string? InstaledApplications { get; set; }
        public string? Licences { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; }


        // Foreign key to relate a computer to an employee
        public Guid? EmployeeId { get; set; }

        //public Employee? Employee { get; set; }
        public IEnumerable<ComputerHistory>? ComputerHistory { get; set; }
    }
}