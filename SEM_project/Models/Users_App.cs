using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEM_project.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEM_project.Models
{
    public class Users_App : BaseClass
    {
        //Personal Information
        [Required, StringLength(60), RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Apellidos")]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Identificación")]
        public string? Identification { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime DateBirth { get; set; }

        //Contact Information
        [Required, Display(Name = "Contraseña")]
        public EnumCountries EnumCountries { get; set; }

        [MaxLength(50)]
        [Display(Name = "Ciudad")]
        public string? City { get; set; }

        [MaxLength(50)]
        [Display(Name = "Barrio")]
        public string? Neighborhood { get; set; }

        [Required]
        [MaxLength(80)]
        [Display(Name = "Dirección")]
        public string? Address { get; set; }

        [DataType(DataType.PhoneNumber), StringLength(25)]
        [Display(Name = "Teléfono")]
        public string? phone { get; set; }

        //Connect with aspnetUsers Table
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Tiene que ser un email correcto")]
        [DataType(DataType.EmailAddress)]
        public string? AspNetUserId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "mínimo 8 caracteres", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W).+$",
            ErrorMessage =
                "Contraseña debe tener un digito ('0'-'9'), una mayuscula ('A'-'Z'), y un caracter especial.")]
        [Display(Name = "Contrasena")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool IsActive { get; set; } = true;

        [NotMapped] // Exclude this property from Entity Framework mapping
        [Display(Name = "Selecciona el rol para el usuario")]
        public string? SelectedRoleId { get; set; }


        [Display(Name = "Rol")] public string? RoleName { get; set; }


        [NotMapped] // Exclude this property from Entity Framework mapping
        public IEnumerable<SelectListItem>? role { get; set; }
    }
}