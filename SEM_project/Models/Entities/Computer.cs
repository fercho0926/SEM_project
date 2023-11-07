using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models.Entities
{
    public class Computer
    {
        public Guid ComputerId { get; set; }

        [Display(Name = "Placa")] public string? Serial { get; set; }

        [Required]
        [Display(Name = "Referencia")]
        public string? Reference { get; set; }

        [Display(Name = "Procesador")] public string? Processer { get; set; }
        [Display(Name = "Memoria Ram")] public string? Ram { get; set; }
        [Display(Name = "Disco Duro")] public string? HardDisk { get; set; }
        [Display(Name = "Sistema Operativo")] public string? OperativeSystem { get; set; }

        [Display(Name = "Tipo de Equipo")] public string? Model { get; set; }

        //public string? InstaledApplications { get; set; }
        //public string? Licences { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Asignado")] public bool IsAssigned { get; set; }
        [Display(Name = "Se da de Baja")] public bool Unsubscribed { get; set; }

        // Foreign key to relate a computer to an employee
        public Guid? EmployeeId { get; set; }

        //public Employee? Employee { get; set; }
        public IEnumerable<ComputerHistory>? ComputerHistory { get; set; }
        public IEnumerable<ComputerToLicence>? ComputerToLicence { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string LocationName { get; set; }
        public string LocationFloor { get; set; }
        public float Value { get; set; }

    }
}