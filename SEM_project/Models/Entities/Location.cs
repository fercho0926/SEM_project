using Microsoft.Build.Evaluation;

namespace SEM_project.Models.Entities
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
