using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DashboardOverallSaudaInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long LoginUserId { get; set; }
        public long BDOId { get; set; }
        public long RoleId { get; set; }
        public bool IsShowDealer { get; set; }
        //public bool IsBulkPack { get; set; }
        public long CurrentFinancialYearId { get; set; }
        public long ZHId { get; set; }
        public long PackGroupId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long DealerId { get; set; }
    }
    public class DashboardSaudaDetailsByDealersInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long DealerId { get; set; }
        public long LoginUserId { get; set; }
        public bool IsShowDealer { get; set; }
        public long PackGroupId { get; set; }
        public bool IsPendingSauda { get; set; }
        //public bool IsBulkPack { get; set; }
    }

    public class StateWiseDashboard
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
