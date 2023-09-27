namespace SEM_project.Models
{
    public class Logs : BaseClass
    {
        public DateTime Date { get; set; }
        public string? User { get; set; }
        public string? Action { get; set; }
        public string? Notes { get; set; }
    }
}