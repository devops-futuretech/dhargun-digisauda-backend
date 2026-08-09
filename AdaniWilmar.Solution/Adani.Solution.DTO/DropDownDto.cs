using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class DropDownDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MobileNumber { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public long SerialNo { get; set; }
        public long? OilTypeId { get; set; }
        public long? PackGroupId { get; set; }
    }

    public class DataRangeDto
    {
        public long FromValue { get; set; }
        public long ToValue { get; set; }
    }

    public class SkuOutputDto
    {
        public long ParentId { get; set; }
        public long SkuId { get; set; }
        public string Name { get; set; }
        public long PackGroupId { get; set; }
        public string PackGroupName { get; set; }
        public string Code { get; set; }
    }
}
