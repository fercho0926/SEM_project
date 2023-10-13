using System.ComponentModel.DataAnnotations;

namespace SEM_project.Utils
{
    public enum EnumSubdepartment
    {
        [Display(Name = "Administrativa y Financiera")]
        Administrativa_Financiera,
        [Display(Name = "Planeación Educativa")]
        Planeacion_Educativa,
        [Display(Name = "Prestación del servicio Educativo")]
        Prestación_del_Servicio_Educativo,
        NA
    }
}