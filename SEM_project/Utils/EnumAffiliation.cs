using System.ComponentModel.DataAnnotations;

namespace SEM_project.Utils
{
    public enum EnumAffiliation
    {
        [Display(Name = "Carrera Administrativa")]
        Carrera_Administrativa,
        Provisional,
        Contratista,
        [Display(Name = "Libre remoción y nombramiento")]
        Libre_Remocion_Y_Nombramiento,
        NA
    }
}