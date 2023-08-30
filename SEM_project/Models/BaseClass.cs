using System.ComponentModel.DataAnnotations;

namespace SEM_project.Models
{
    public class BaseClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string? Name { get; set; }

    }
}
