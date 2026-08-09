using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum SeederDataType
    {
        [Description("Contract Type")] ContractType = 1,
        [Description("Inco Terms")] IncoTerms = 2,
        [Description("Delivery Priority")] DeliveryPriority = 3,
        [Description("Picking Point")] PickingPoint = 4        
    }
}
