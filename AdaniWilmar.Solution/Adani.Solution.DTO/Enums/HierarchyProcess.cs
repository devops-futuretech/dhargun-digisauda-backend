using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum HierarchyProcess
    {
        [Description("Organization")] Organization = 1,
        [Description("Sales")] Sales = 2,
        [Description("Speciality Fat")] SpecialityFat = 3,
        [Description("Complaint Management System")] ComplaintManagementSystem = 4
    }
}
