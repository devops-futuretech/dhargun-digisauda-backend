using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum SaudaFunctionTypes
    {
        [Description("SaudaNumberUpdate")] SaudaNumberUpdate = 1,
        //[Description("SaudaExtensionUpdate")] SaudaExtensionUpdate = 2,
        [Description("SalesOrder")] SalesOrder = 3,
        [Description("SalesOrderDeliveryNoUpdate ")] SalesOrderDeliveryNoUpdate = 5,
        [Description("SalesOrderInvoicNoUpdate ")] SalesOrderInvoicNoUpdate = 6,
        [Description("SaudaChange ")] SaudaChange = 2,
    }
}
