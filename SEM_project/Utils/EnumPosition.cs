using System.ComponentModel.DataAnnotations;

namespace SEM_project.Utils
{
    public enum EnumPosition
    {
        [Display(Name = "Auxiliar Administrativo")]
        administrativo,
        [Display(Name = "Profesional")]
        profesional,
        [Display(Name = "No definido")]
        No_definido,
        Asesor,
        Abogado,
        Analista,Servidores,
        Analista_Telecomunicaciones,
        AsesorDespacho,
        Coordinador,
        DirectorTecnico,
        LiderDeProyecto,
        LiderDePrograma,
        LiderZonal,
        GuardaDeSeguridad,
        ProfesionalAltoRendimiento,
        ProfesionalEspecializado,
        ProfesionalUniversitario,
        ProfesionalDeGestion,
        SecretarioDespacho,
        SecretariaO,
        SubsecretarioPlaneacionEducativa,
        SubsecretarioAdministrativoYFinanciero,
        SubsecretarioPrestacionServicioEducativo,
        TecnicoAdministrativo,
        TecnicoAsistencial,
        TecnicoOperativo
    }
}