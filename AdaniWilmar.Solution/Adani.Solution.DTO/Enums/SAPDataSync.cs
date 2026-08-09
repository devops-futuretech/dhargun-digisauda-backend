using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum SAPDataSyncSAPToAPP
    {
        [Description("Broker")] Broker = 1,
        [Description("Customer")] Customer = 2,
        [Description("Credit Limit")] CreditLimit = 3,
        [Description("Customer Ledger")] CustomerLedger = 4,
        [Description("Depot")] Depot = 5,
        [Description("Direct Trade Ticket TP")] DirectTradeTicketTP = 6,
        [Description("Direct Trade Ticket SF")] DirectTradeTicketSF = 7,
        [Description("Direct Sauda")] DirectSauda = 8,
        [Description("DO Update")] DOUpdate = 9,
        [Description("DO Delete")] DODelete = 10,
        [Description("Invoice")] Invoice = 11,
        [Description("Invoice Cancel & Return")] InvoiceCancelReturn = 12,
        [Description("Invoie Payment Status Update")] InvoiePaymentStatusUpdate = 13,
        [Description("Lifting Request Enquiry Number Update")] LiftingRequest = 14,
        [Description("ShipToParty")] ShipToParty = 15,
        [Description("Sku")] Sku = 16,
        [Description("Sauda Limit")] SaudaLimit = 17,
        [Description("Sauda HBC & SPF Number Update")] SaudaHBCSPFNumberUpdate = 18,
        [Description("Sauda Approval")] SaudaApproval = 19,
        [Description("Sauda Amendment")] SaudaAmendment = 20,
        [Description("Trade Ticket Number")] TradeTicketNumber = 21,
        [Description("Sauda Conversion")] SaudaConversion = 22,
        [Description("Sauda Extension")] SaudaExtension = 23,
        [Description("Cheque Inventory Report")] ChequeInventoryReport = 24
        
    }

    public enum SAPDataSyncAPPToSAP
    {
        //[Description("Lifting Request Enquiry")] LiftingRequest = 1,
        //[Description("Sauda Limit")] SaudaLimit = 2,
        [Description("Sauda HBC")] SaudaHBC = 1,        
        //[Description("Trade Ticket")] TradeTicket = 4,
        //[Description("Trade Ticket SF")] DirectTradeTicket = 5,
        //[Description("Sauda Conversion")] SaudaConversion = 6,
        //[Description("Sauda Extension")] SaudaExtension = 7,
        [Description("Sauda SPF")] SaudaSPF = 2,
        [Description("Sauda Loose")] SaudaLoose = 3
    }

    public enum SAPDataSyncAPPToSAPWithoutTT
    {
        [Description("Sauda HBC")] SaudaHBC = 1,
        [Description("Sauda SPF")] SaudaSPF = 2,
        [Description("Sauda Loose")] SaudaLoose = 3
    }
}
