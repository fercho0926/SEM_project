namespace SEM_project.Models.Entities
{
    public class ComputerHistory
    {
        public Guid ComputerHistoryId { get; set; }
        public Guid ComputerId { get; set; }
        public DateTime date { get; set; }
        public string Owner { get; set; }
        public string Action { get; set; }
        public string Performer { get; set; }
        public string Details { get; set; }
    }
}