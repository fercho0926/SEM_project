﻿using SEM_project.Utils;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Identificacion")]
        public string? Identification { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime DateBirth { get; set; }

        //Contact Information
        [Required, Display(Name = "Contraseña")] public EnumCountries EnumCountries { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Ciudad")]
        public string? City { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Barrio")]
        public string? Neighborhood { get; set; }

        [Required]
        [MaxLength(80)]
        [Display(Name = "Direccion")]
        public string? Address { get; set; }

        [DataType(DataType.PhoneNumber), StringLength(25)]
        [Display(Name = "Telefono")]
        public string? phone { get; set; }

        //Connect with aspnetUsers Table
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? AspNetUserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Contrasena")]
        [DataType(DataType.Password)]

        public string? Password { get; set; }




    }
}