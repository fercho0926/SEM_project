namespace SEM_project.Models.Entities
{
    public class EmployeeToComputer
    {
        public Guid EmployeeToComputerId { get; set; }

        public Guid ComputerId { get; set; }

        public Guid EmployeeId { get; set; }
    }
}