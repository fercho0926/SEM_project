namespace SEM_project.Models.Entities
{
    public class Computer
    {
        public Guid ComputerId { get; set; }
        public string Serial { get; set; }
        public string Reference { get; set; }
        public string Processer { get; set; }
        public string Ram { get; set; }
        public string HardDisk { get; set; }
        public string OperativeSystem { get; set; }
        public string Model { get; set; }
        public string InstaledApplications { get; set; }
        public string Licences { get; set; }
        public IEnumerable<ComputerHistory> ComputerHistory { get; set; }
    }
}