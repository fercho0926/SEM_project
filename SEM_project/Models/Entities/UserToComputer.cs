namespace SEM_project.Models.Entities
{
    public class UserToComputer
    {
        public Guid UserToComputerId { get; set; }

        public Guid ComputerId { get; set; }

        public int Id { get; set; } // id user
    }
}