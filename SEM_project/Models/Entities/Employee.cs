using SEM_project.Utils;

namespace SEM_project.Models.Entities
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string IDNumber { get; set; }
        public DateTime Date { get; set; }
        public EnumPosition EnumPosition { get; set; }
        public EnumAffiliation EnumAffiliation { get; set; }
        public EnumLocation EnumLocation { get; set; }
        public EnumFloor EnumFloor { get; set; }
        public EnumSubdepartment EnumSubdepartment { get; set; }
        public EnumWorkGroup EnumWorkGroup { get; set; }
        public string AssignedEquipmentPlate { get; set; }
        public string PhonePlate { get; set; }
        public string PhoneModel { get; set; }
        public string PhoneSerial { get; set; }
        public string PhoneExtension { get; set; }
        public string Observations { get; set; }
        public bool PersonalEquipment { get; set; }

        public ICollection<Computer> Computers { get; set; }
    }
}