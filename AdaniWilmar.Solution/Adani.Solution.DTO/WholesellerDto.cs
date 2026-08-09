using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class WholesellerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class WholesellerSecondarySaleslistDto
    {
        public DateTime? VisitDate { get; set; }
        public List<WholesellerSecondarySalesDto> WholesellerSecondarySales { get; set; }
        public WholesellerSecondarySaleslistDto()
        {
            WholesellerSecondarySales = new List<WholesellerSecondarySalesDto>();
        }
    }
    public class WholesellerSecondarySalesDto
    {
        public long WholesellerId { get; set; }
        public string Name { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public DateTime? VisitDate { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class WholesellerSecondarySalesInputDto
    {
        public long WholesellerId { get; set; }
        public DateTime VisitDate { get; set; }
    }
    public class WholesellerSecondarySalesDetailOutputDto
    {
        public long WholesellerId { get; set; }
        public string Wholeseller { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
    public class SecondarySalesInputDto
    {
        public long EmployeeId { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
