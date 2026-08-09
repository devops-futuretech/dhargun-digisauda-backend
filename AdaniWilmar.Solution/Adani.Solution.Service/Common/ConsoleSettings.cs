using System;
using System.Configuration;
using System.IO;

namespace Adani.Solution.Service.Common
{
    public static class ConsoleSettings
    {
        public static Uri ApiUrl => new Uri(ConfigurationManager.AppSettings["apiurl"]);
        public const string WebApiUrlPostVerifyToken = "api/authenticate/verify/console";
        public const string WebApiUrlPostValidateUser = "/api/authorize/user/console";
        public const string WebApiUrlCustomerPostSyncData = "api/sap/customer/save";
        public const string WebApiUrlPostSyncStockData = "api/integrate/getsapstockdata";
        public const string WebApiUrlTradeTicketGetSyncData = "api/sap/tradeticket/list";
        public const string WebApiUrlTradeTicketUpdateSyncData = "api/sap/tradeticket/update";
        public const string WebApiUrlSaudaGetSyncData = "api/sap/sauda/list";
        public const string WebApiUrlSaudaUpdateSyncData = "api/sap/sauda/update";
        public const string WebApiUrlSaudaApprovalGetSyncData = "api/sap/sauda/approval/list";
        public const string WebApiUrlStateCityDistrictPostSyncData = "api/sap/statecitydistrict/save";
        public const string WebApiUrlLiftingRequestSaveSyncData = "api/sap/liftingrequest/save";
        public const string WebApiUrlLiftingRequestGetSyncData = "api/sap/liftingrequest/list";
        public const string WebApiUrlInvoicePostSyncData = "api/sap/invoice/save";
        public const string WebApiUrlSaudaLimitSaveSyncData = "api/sap/saudalimit/save";
        public const string WebApiUrlSaudaLimitGetSyncData = "api/sap/saudalimit/list";
        public const string WebApiUrlSkuSaveSyncData = "api/sap/sku/save";
        public const string WebApiUrlCreditMasterSaveSyncData = "api/sap/credit/save";
        public const string WebApiUrlDepotMasterSaveSyncData = "api/sap/depot/save";
        public const string WebApiUrlCustomerLedgerPostSyncData = "api/sap/customerledger/save";
        public const string WebApiUrlDODeleteSyncData = "api/sap/do/delete";
        public const string WebApiUrlDOUpdateSyncData = "api/sap/do/update";
        public const string WebApiUrlSaudaAmendmentSyncData = "api/sap/sauda/amendment";
        public const string WebApiUrlSaudaCreationSyncData = "api/sap/sauda/create";
        public const string WebApiUrlTradeTicketCreateSyncData = "api/sap/tradeticket/create";
        public const string WebApiUrlInvoiceStatusChange = "api/sap/invoice/paymentstatus/update";
        public const string WebApiUrlLiftingRequestInquiryNumberUpdateSyncData = "api/sap/liftingrequest/enquirynumber/update";
        public const string WebApiUrlGetLiftingRequestEnquiryNumberSyncData = "api/sap/getliftingrequest/enquirynumber";
        //email subject
        public const string PendingContractReportSubject = "Pending Contract Report";
        public const string TruckIndentReportSubject = "Truck Indent Report";
        public const string SaudaRelease = "Sauda Release";



        public const string Emailtemplate = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF - 8'><meta http-equiv='X - UA - Compatible' content='IE = edge'><meta name='viewport' content='width=device-width,initial-scale=1'></head><body style='margin:0;padding:0' dir='ltr' bgcolor='#ffffff'><table border='0' cellspacing='0' cellpadding='0' align='center' id='m_-7626415423304311386email_table' style='border-collapse:collapse'><tbody><tr><td id='email-temp-container' style='font-family:Poppins,Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;background:#fff'><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td><table class='emlogo' id='emlogo1' border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse;text-align:center;width:100%'><tbody><tr><td style='line-height:0;width:600px;max-width:600px;padding:0 0 15px 0'><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td style='background:transparent linear-gradient(270deg,#e9322b 0,#fbad18 100%) 0 0 no-repeat padding-box;width:100%;height:4px'></td></tr><tr style='background:#fff9ef;background:transparent linear-gradient(270deg,rgba(233,50,43,.07) 0,rgba(251,173,24,.07) 100%) 0 0 no-repeat padding-box'><td style='width:100%;text-align:center;height:120px'><img height='60' src='https://sauda.adaniwilmar.in:8080/images/logo1.png' style='border:0' class='CToWUd' data-bit='iit'></td></tr></tbody></table></td></tr></tbody></table></td></tr><tr><td><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse;margin:0 auto 0 auto'><tbody><tr><td><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse;margin:0 auto 0 auto;width:95%'><tbody><tr><td><table border='0' width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td><table border='0' cellspacing='0' cellpadding='0' style='border-collapse:collapse'><tbody><tr><td>##MailContent##</td></tr><tr><td height='20' style='line-height:20px'>&nbsp;</td></tr><tr><td><p style='margin:10px 0 10px 0;color:#050b13;font-weight:700;font-size:16px'>Regards,</p><p style='margin:10px 0 10px 0;color:#050b13;font-size:16px'>Adani Groups</p></td></tr></tbody></table></td></tr></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr><tr><td><table border='0' cellspacing='0' cellpadding='0' style='border-collapse:collapse;margin:0 auto 0 auto;width:95%'><tbody><tr><td height='20'>&nbsp;</td></tr><tr><td style='width:100%;text-align:center;height:70px;border-top:1px solid rgba(112,112,112,.3)'><div style='color:#050b13;font-size:12px;margin:0 auto 5px auto'>Adani groups Limited 2022. All rights reserved.<br></div></td></tr></tbody></table></td></tr></tbody></table><style></style></body></html>";
        //public const string Emailtemplate = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
        //            "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta name=\"viewport\" content=\"width=device-width\" /><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" +
        //            "<title>Emami</title>" +
        //            "<meta name=\"description\" content=\"Emami\" />" +
        //            "<style>* {margin: 0;padding: 0;font-family: \"Helvetica Neue\", \"Helvetica\", Helvetica, Arial, sans-serif;box-sizing: border-box;font-size: 14px;} img {max-width: 100%;} body {-webkit-font-smoothing: antialiased;-webkit-text-size-adjust: none;width: 100% !important;height: 100%;line-height: 1.6;} table td { vertical-align: top; } body { background-color: #f6f6f6; } .body-wrap { background-color: #f6f6f6; width: 100%; } .container { display: table !important; max-width: 600px !important; margin: 0 auto !important; clear: both !important; } .content { max-width: 600px; margin: 0 auto; display: table; padding: 0; background: #fff; border-right: 1px solid #e9e9e9; border-left: 1px solid #e9e9e9; border-bottom: 1px solid #e9e9e9; border-top: 5px solid #32bd6a; border-radius: 3px; } .main { padding: 10px; } .content-wrap { padding: 20px; } .content-block { padding: 0 0 10px; width: 50%; }</style></head>" +
        //            "<body><table class=\"body-wrap\" style=\"border-spacing: 0;padding:10px\"> <tr><td></td><td class=\"container\" width=\"600\"><div class=\"content\"><table width=\"100%\" style=\"border-spacing: 0\">" +
        //            "<tr><td><img src=\"logo\"/></td></tr></table> <table width=\"100%\" style=\"padding: 0 20px;background: #ffffff;border-spacing: 0\"> <tr><td class=\"\" style=\"padding-top: 20px; padding-bottom: 20px;padding-left: 20px; width: 100%;\">" +
        //            "##MailContent##" +
        //            "<p style=\"margin:5px 0 45px; font-size: 16px; color: #444444;\">Regards,<br /><strong>Emami</strong><br />" +
        //            "<a href=\"www.emamiltd.in\">www.emamiltd.in</a><br /></p></td></tr></table></div></td><td></td></tr></table>" +
        //            "<p style=\"color:#777;text-align:center;font-size: 12px;\">&copy; 2018 Emami. All Rights Reserved.</p></body></html>";

        public const string ReplaceMainContent = "##MailContent##";
        public const string LogoName = "logo2.png";
        public const string PdfExtention = ".PDF";
        public const string CsvExtention = ".CSV";

        public const string PartnerFunctionBroker = "AG";
        public const string PartnerFunctionCustomer = "RE";

        public const string CsvDelimiter = "#";
        public const char SapDelimiter = '#';
        public const string NewLineDelimiter = "\r\n";
        public const string DateFormat = "dd.MM.yyyy";
        public const string TimeFormat = "HH:mm";
        public const string SAPDateFormat = "yyyyMMdd";
        public const string KeyType = "WebKey";
        public static string WebKey => ConfigurationManager.AppSettings["WebKey"];
        public static string EncryptionKey => ConfigurationManager.AppSettings["EncryptionKey"];
        public static string VectorKey => ConfigurationManager.AppSettings["VectorKey"];
        public static bool IsFullDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsFullDataSync"]);

        public static string SftpServerPath => ConfigurationManager.AppSettings["SftpServerPath"];
        public static string SftpServerName => ConfigurationManager.AppSettings["SftpServerName"];
        public static string SftpUser => ConfigurationManager.AppSettings["SftpUser"];
        public static string SshPrivateKeyPath => ConfigurationManager.AppSettings["SshPrivateKeyPath"];


        public static bool IsCustomerDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsCustomerDataSync"]);
        public static bool IsBrokerDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsBrokerDataSync"]);
        public static bool IsGetTradeTicketDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsGetTradeTicketDataSync"]);
        public static bool IsCreateTradeTicketDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsCreateTradeTicketDataSync"]);
        public static bool IsUpdateTradeTicketDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateTradeTicketDataSync"]);
        public static bool IsGetSaudaDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsGetSaudaDataSync"]);
        public static bool IsUpdateSaudaDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateSaudaDataSync"]);
        public static bool IsUpdateSaudaApprovalDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateSaudaApprovalDataSync"]);
        public static bool IsStateCityDistrictDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsStateCityDistrictDataSync"]);
        public static bool IsSaveLiftingRequestDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveLiftingRequestDataSync"]);
        public static bool IsGetLiftingRequestDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsGetLiftingRequestDataSync"]);
        public static bool IsSaveInvoiceDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveInvoiceDataSync"]);
        public static bool IsGetSaudaLimitDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsGetSaudaLimitDataSync"]);
        public static bool IsSaveSaudaLimitDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveSaudaLimitDataSync"]);
        public static bool IsSaveSkuDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveSkuDataSync"]);
        public static bool IsSaveCreditMasterDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveCreditMasterDataSync"]);
        public static bool IsSaveDepotMasterDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveDepotMasterDataSync"]);
        public static bool IsMoveFileToArchive => Convert.ToBoolean(ConfigurationManager.AppSettings["IsMoveFileToArchive"]);
        public static bool IsMoveSAPFileToProcessed => Convert.ToBoolean(ConfigurationManager.AppSettings["IsMoveSAPFileToProcessed"]);
        public static bool IsMoveSAPFileToArchive => Convert.ToBoolean(ConfigurationManager.AppSettings["IsMoveSAPFileToArchive"]);
        public static bool IsMoveInboundFileFailedToNewFolder => Convert.ToBoolean(ConfigurationManager.AppSettings["MoveInboundFileFailedToNewFolder"]);
        public static bool IsMoveOutBoundFileFailedToNewFolder => Convert.ToBoolean(ConfigurationManager.AppSettings["MoveOutBoundFileFailedToNewFolder"]);
        public static bool IsSaveCustomerLedgerDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveCustomerLedgerDataSync"]);
        public static string CsvFilePath => ConfigurationManager.AppSettings["CsvFilePath"];
        public static string PdfFilePath => ConfigurationManager.AppSettings["PdfFilePath"];
        public static string LogoPath => ConfigurationManager.AppSettings["LogoPath"];
        public static bool IsDeleteDODataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsDeleteDODataSync"]);
        public static bool IsUpdateDODataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateDODataSync"]);
        public static bool IsCancelAndReturnInvoiceDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsCancelAndReturnInvoiceDataSync"]);
        public static bool IsSaudaAmendmentDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaudaAmendmentDataSync"]);
        public static bool IsSaudaCreationDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaudaCreationDataSync"]);
        public static bool IsUpdateInvoicePaymentStatusDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateInvoicePaymentStatusDataSync"]);
        public static bool IsUpdateLiftingRequestInquiryNumberDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateLiftingRequestInquiryNumberDataSync"]);
        public static bool IsGetLiftingRequestEnquiryNumberDataSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsGetLiftingRequestEnquiryNumberDataSync"]);
        public static string PasswordChangedDaysCount => ConfigurationManager.AppSettings["PasswordChangedDaysCount"];

        public static readonly int BulkInsertRecordCount = Convert.ToInt32(ConfigurationManager.AppSettings["BulkInsertRecordCount"]);
        public static bool IsInboundDirectSyncToSapAllowed => Convert.ToBoolean(ConfigurationManager.AppSettings["IsInboundDirectSyncToSapAllowed"]);
        public static int CustomerLedgerDaysCount = Convert.ToInt32(ConfigurationManager.AppSettings["CustomerLedgerDaysCount"]);

        //State Details
        public static string StateExePath => ConfigurationManager.AppSettings["StateExePath"];

        public static string TraditionalProcessExePath => ConfigurationManager.AppSettings["TraditionalProcessExePath"];
        public static string ReverseAuctionExePath => ConfigurationManager.AppSettings["ReverseAuctionExePath"];
        public static string VolumeCapacityRemainderNotification => ConfigurationManager.AppSettings["VolumeCapacityRemainderNotification"];
        public static string HanaPassword => ConfigurationManager.AppSettings["HanaPassword"];
        public static string HanaUsername => ConfigurationManager.AppSettings["HanaUsername"];
        public static string DarwinboxPassword => ConfigurationManager.AppSettings["DarwinboxPassword"];
        public static string DarwinboxUsername => ConfigurationManager.AppSettings["DarwinboxUsername"];
        public static string SaudaCreationHanaApiUrl => ConfigurationManager.AppSettings["SaudaCreationHanaApiUrl"];
        public static string SaudaExtensionHanaApiUrl => ConfigurationManager.AppSettings["SaudaExtensionHanaApiUrl"];
        public static string SaudaConversionHanaApiUrl => ConfigurationManager.AppSettings["SaudaConversionHanaApiUrl"];
        public static string SaudaApprovalHanaApiUrl => ConfigurationManager.AppSettings["SaudaApprovalHanaApiUrl"];
        public static string LiftingInquiryHanaApiUrl => ConfigurationManager.AppSettings["LiftingInquiryHanaApiUrl"];
        public static string CustomerStatement => ConfigurationManager.AppSettings["CustomerStatement"];
        public static string ApiPhysicalPath => ConfigurationManager.AppSettings["ApiPhysicalPath"];
        public static string SapPhysicalPath => ConfigurationManager.AppSettings["SapPhysicalPath"];
        public static string SaudaReleaseApiUrl => ConfigurationManager.AppSettings["SaudaReleaseApiUrl"];
        public static string SaudaConversionApiUrl => ConfigurationManager.AppSettings["SaudaConversionApiUrl"];
        public static string SaudaExtensionApiUrl => ConfigurationManager.AppSettings["SaudaExtensionApiUrl"];
        public static string OpenContractApiUrl => ConfigurationManager.AppSettings["OpenContractApiUrl"];
        public static string CustomerLedgerRequestApiUrl => ConfigurationManager.AppSettings["CustomerLedgerRequestApiUrl"];
        public static string DarwinboxAPIUrl => ConfigurationManager.AppSettings["DarwinboxAPIUrl"];
        public static string DirectBroker => ConfigurationManager.AppSettings["DirectBroker"];
        public static bool UATEmail => Convert.ToBoolean(ConfigurationManager.AppSettings["UATEmail"]);
        public static int BatchCount => Convert.ToInt32(ConfigurationManager.AppSettings["BatchCount"]);
        public static int PendingContractDeleteHours => Convert.ToInt32(ConfigurationManager.AppSettings["PendingContractDeleteHours"]);
        public static bool ContractDateConditionCheck => Convert.ToBoolean(ConfigurationManager.AppSettings["ContractDateConditionCheck"]);
        public static bool SalesOrderDateCheck => Convert.ToBoolean(ConfigurationManager.AppSettings["SalesOrderDateCheck"]);
        public static string SalesOrderDate => ConfigurationManager.AppSettings["SalesOrderDate"];


        public const string ResponseSuccess = "Y77T3XP2B";
        public const string Response = "response";
        public const string ResponseError = "SXVI7XCEU";
        public const string ResponseWebToken = "E6DYES1Q2";
        public const string ResponseMessage = "Message";
        public const string SyncFailed = "Failed";
        public const string SyncSuccess = "Success";

        public const string SkuReplace = "BASESKU";
        public const string CustomerGroupReplace = "BASEGROUP";

        public const string NewFolder = "/New/";
        public const string ModifiedFolder = "/Modified/";
        public const string ArchiveFolder = "/Archive/";
        public const string FailedFolder = "/Failed/";
        public const string ProcessedFolder = "/Processed/";
        public const string InboundFolder = "/Inbound";
        public const string OutboundFolder = "/Outbound";
        public const string CustomerFolder = "Customer";
        public const string BrokerFolder = "Broker";
        public const string TradeTicketFolder = "TradeTiket";
        public const string SaudaFolder = "SaudaCreation";
        public const string SaudaHBCFolder = "HBC";
        public const string SaudaSPFFolder = "SPF";
        public const string SaudaApproval = "SaudaApproval";
        public const string InvoiceFolder = "Invoice";
        public const string LiftingRequestFolder = "LiftingRequest";
        public const string SaudaLimitFolder = "SaudaLimit";
        public const string SKUMasterFolder = "SKUMaster";
        public const string CreditMasterFolder = "CreditMaster";
        public const string DepotMasterFolder = "DepotMaster";
        public const string StateFolder = "State";
        public const string CustomerLedgerFolder = "CustomerLedger";
        public const string InvoicePdfFolder = "InvoicePdf";
        public const string CustomerLedgerPdfFolder = "CustomerLedgerPdf";
        public const string DODeleteFolder = "DODelete";
        public const string DOUpdateFolder = "DOUpdate";
        public const string InvoiceCancelAndReturnFolder = "InvoiceCancelAndReturn";
        public const string SaudaAmendmentFolder = "SaudaAmendment";
        public const string DirectSaudaFolder = "DirectSauda";
        public const string DirectTradeTiketFolder = "DirectTradeTiket";
        public const string DirectTradeTiketSFFolder = "DirectTradeTiket_sf";
        public const string InvoicePaymentStatus = "InvoicePaymentStatus";
        public const string LiftingInquiry = "SalesOrder";
        public const string SpecialityFatTradeTicketFolder = "TradeTiketSF";
        public const string SpecialityFatTradeTicketSubject = "SF TradeTiket";
        public const string SpecialityFatTradeTicketNewCsv = "TRADE_SF";
        public const string SpecialityFatTradeTicketModifiedCsv = "TRADE_SF_M";
        public const string ShipToParty = "Shiptoparty";
        public const string PendingContract = "Pending_contract";
        public const string SalesReport = "SALES_REG";
        public const string SaudaConversion_ValidationMsg = "SaudaConversion_ValidationMsg";
        public const string SaudaLooseOilFolder = "CreateSaudaLooseOil";
        public const string SaudaReleaseFolder = "SaudaRelease";



        public static string AllInboundFolders = string.Concat(CustomerFolder, ",", BrokerFolder, ",", TradeTicketFolder, ",", SaudaFolder, ",",
            InvoiceFolder, ",", LiftingRequestFolder, ",", SaudaLimitFolder, ",", SKUMasterFolder, ",", CreditMasterFolder, ",", DepotMasterFolder, ",",
            SaudaAmendmentFolder, ",", DODeleteFolder, ",", DOUpdateFolder, ",", InvoiceCancelAndReturnFolder, ",", DirectSaudaFolder, ",", 
            DirectTradeTiketFolder, ",", InvoicePaymentStatus, ",", LiftingInquiry,",", PendingContract,",", SalesReport,",", ShipToParty,",",CustomerLedgerFolder,",", SpecialityFatTradeTicketFolder,
            ",", DirectTradeTiketSFFolder);

        public static string AllOutBoundFolders = string.Concat(LiftingRequestFolder, ",", SaudaApproval, ",", SaudaFolder, ",", SaudaLimitFolder, ",", TradeTicketFolder, ",", LiftingInquiry);

        public static string ErrorInboundFolders = string.Concat(CustomerFolder, ",", BrokerFolder, ",", TradeTicketFolder, ",", SaudaFolder, "/", SaudaHBCFolder, ",", SaudaFolder, "/", SaudaSPFFolder, ",",
            InvoiceFolder, ",", LiftingRequestFolder, ",", SaudaLimitFolder, ",", SKUMasterFolder, ",", CreditMasterFolder, ",", DepotMasterFolder, ",",
            SaudaAmendmentFolder, ",", DODeleteFolder, ",", DOUpdateFolder, ",", InvoiceCancelAndReturnFolder, ",", DirectSaudaFolder, ",", CustomerLedgerFolder, ",", DirectTradeTiketFolder, ",", InvoicePaymentStatus, ",", LiftingInquiry,",", DirectTradeTiketSFFolder);

        public static string ErrorOutBoundFolders = string.Concat(LiftingRequestFolder, ",", SaudaApproval, ",", SaudaFolder, "/", SaudaHBCFolder, ",", SaudaFolder, "/", SaudaSPFFolder, ",", SaudaLimitFolder, ",", TradeTicketFolder, ",", LiftingInquiry);

        //SAP file move       
        public const string SAPFileMove = "MoveFiles";


        //Subjects
        public const string SAPToAppDataSyncEmailSubject = "SAP to APP Data Sync";
        public const string AppToSapDataSyncEmailSubject = "APP to SAP Data Sync";
        public const string TradeTicketNumberUpdateSubject = "Trade Tiket Number Update";
        public const string TradeTicketSubject = "Trade Tiket";
        public const string TradeTicketCreateSubject = "Trade Tiket Create";
        public const string SaudaHBCCreationSubject = "Sauda Creation HBC";
        public const string SaudaSPFRasoiCreationSubject = "Sauda Creation SPF and Rasoi";
        public const string SaudaNumberUpdateSubject = "Sauda Number Update";
        public const string SaudaApprovalSubject = "Sauda Approval";
        public const string LiftingRequestSubject = "Lifting Request";
        public const string LiftingRequestDeliveryOrderNumberUpdateSubject = "Lifting Request Delivery Order Number";
        public const string SaudaLimitSubject = "Sauda Limit";
        public const string SkuSubject = "Sku Master";
        public const string CreditMasterSubject = "Credit Master";
        public const string DepotMasterSubject = "Depot Master";
        public const string DODeleteSubject = "DO Delete";
        public const string DOUpdateSubject = "DO Update";
        public const string SaudaAmendmentSubject = "Sauda Amendment";
        public const string SaudaCreationSubject = "Sauda Creation";
        public const string SaudaCreationSubjectAppToSap = "Sauda Creation App To Sap";
        public const string InvoicePaymentStatusSubject = "Invoice Payment Status";
        public const string LiftingRequestInquiryNumberUpdateSubject = "Sales Order Number Update";
        public const string SaudaConversionSubject = "Sauda Conversion";
        public const string SaudaExtensionionSubject = "Sauda Extension";
        public const string SaudaLooseCreationSubject = "Sauda Creation Loose";
        public const string SaudaApprovalConfirmationSubject = "Sauda Approval Confirmation";       
        public const string LooseSaudaNumberUpdateSubject = "Loose Oil Sauda Number Update";
        public const string SaudaReleaseSubject = "Sauda Release";      
        public const string SalesReportSubject = "Sales Report";       
        public const string SaudaExtensionSubject = "Sauda Extension";
        public const string ChequeInventoryReportSubject = "Cheque Inventory Report";
        public const string PricingSubject = "Pricing";
        public const string EmployeeRequestActiveUsersSubject = "Employee Request Active Users";
        public const string SaudaUpdateSubject = "Sauda Update";

        //Csv name
        public const string BrokerCsv = "BROK_MAS_";
        public const string CustomerCsv = "CUST_MAS_";
        public const string TradeTicketNewCsv = "TRADE_TKT_";
        public const string TradeTicketModifiedCsv = "TRADE_TKTM_";
        public const string SaudaHBCCreationCsv = "SAUDA_CRH_";
        public const string SaudaSPFCreationCsv = "SAUDA_CRS_";
        public const string SaudaApprovalCsv = "SAUDA_APPR_";
        public const string LiftingRequestCsv = "DELIVERY_";
        public const string InvoicePdf = "DSA_HBCINVOICE_";
        public const string SaudaLimitCsv = "SAUDA_LIMS_";
        public const string DirectTradeTicketCsv = "DIR_TRADE_";
        public const string DeportCsv = "DEPOT_MAS_";
        public const string TradeTicketNumberCsv = "TRADE_TKTN_";
        public const string SaudaNumberCsv = "SAUDA_NR_";
        public const string InvoiceCsv = "INV_DETAIL_";
        public const string SaudaLimitAddCsv = "SAUDA_LIMA_";
        public const string SkuCsv = "SKU_MAS_";
        public const string CreditMasterCsv = "CREDIT_MAS_";
        public const string CustomerLedgerCsv = "CUST_LEDGR_";
        public const string DODeleteCsv = "DO_DEL_";
        public const string DOUpdateCsv = "DO_AMEND_";
        public const string SaudaAmeCsv = "SAUDA_AME_";
        public const string DirectSaudaCsv = "DIR_SAUDA_";
        public const string InvoiceStatusCsv = "INV_STAT_";
        public const string InquiryrCsv = "INQUIRY_";
        public const string SaudaConversion = "SaudaConversion";
        public const string SaudaConversionCsv = "SAUDA_CONV_";
        public const string SaudaExtension = "SaudaExtension";
        public const string SaudaExtensionCsv = "SAUDA_VAL_";
        public const string ChequeStatus = "ChequeStatus";
        public const string SaudaLooseCreationCsv = "SAUDA_CRL_";
        public const string EmployeeRequestActiveUsersCsv = "User_Active_";


        //Notification 
        public static readonly string FromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
        public static readonly string AWSEmailAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        public static readonly string AWSEmailSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        public static readonly string BucketName = ConfigurationManager.AppSettings["Bucketname"];
        public static readonly string applicationARNForPushNotification = ConfigurationManager.AppSettings["applicationARNForPushNotification"];
        public const string EmailSendSuccessfully = "Email Send Successfully";

        //Smtp Credentials
        public static string SmtpHostServerName = ConfigurationManager.AppSettings["SftpSmtpHostServerName"];
        public static string SmtpNetworkCredentialUserName = ConfigurationManager.AppSettings["SftpSmtpNetworkCredentialUserName"];
        public static string SmtpNetworkCredentialPassword = ConfigurationManager.AppSettings["SftpSmtpNetworkCredentialPassword"];
        public static string SmtpFromMailAddress = ConfigurationManager.AppSettings["SftpSmtpFromMailAddress"];
        public static bool SmtpEnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SftpSmtpEnableSsl"]);
        public static readonly bool AwsEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSEmail"].ToString());
        public static int SmtpNetworkCredentialPort = Convert.ToInt32(ConfigurationManager.AppSettings["SftpSmtpNetworkCredentialPort"]);
        public const string EmailSendError = "Email Send Error";
        public static string ApiPath = ConfigurationManager.AppSettings["ApiUrl"];

        public static string SapDataSyncEmailIds = ConfigurationManager.AppSettings["SapDataSyncEmailIds"];
        public static string SapDataSyncMobileNumbers = ConfigurationManager.AppSettings["SapDataSyncMobileNumbers"];
        public static bool SapDataSyncIsToMailId = Convert.ToBoolean(ConfigurationManager.AppSettings["SapDataSyncIsToMailId"]);
        public static bool IsEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEmail"]);
        public static bool IsSMS = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSMS"]);
        public const string ToEmail = "awlsauda@adaniwilmar.in,awlsauda@gmail.com,vaishnavi.v@adaniwilmar.in";
        public const string DataSyncEmail = "DataSyncEmail";
        public const string DataSyncSms = "DataSyncSMS";
        public const string DataSyncValue = "[DATA_RESULT]";

        public const string FromDisplayName = "No Reply";
        public const string Exception = "Internal Server Error";
        public const string SyncErrorMessage = "While Error occured {0} data sync,Please check failed details.";
        public const string SyncSuccessMessage = "Successfully {0} data synced";

        public static string ReplaceString(this string s, string[] separators, string newVal)
        {
            string[] temp;

            temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(newVal, temp);
        }

        public static string InboundDirectoryFilePath(string syncFolder, bool IsFaild = false)
        {
            var directoryPath = string.Empty;
            if (IsFaild)
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, InboundFolder, FailedFolder, "/");
            }
            else
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, InboundFolder, ProcessedFolder, DateTime.Now.ToString("yyyyMMdd"), "/");
            }
            return directoryPath;
        }

        public static string OutboundDirectoryPath(string syncFolder, bool IsModified = false)
        {
            var directoryPath = string.Empty;
            if (!IsModified)
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, OutboundFolder, NewFolder);
            }
            else
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, OutboundFolder, ModifiedFolder);
            }
            return directoryPath;
        }

        public static string InboundDirectoryPath(string syncFolder, bool IsModified = false)
        {
            var directoryPath = string.Empty;
            if (!IsModified)
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, InboundFolder, NewFolder);
            }
            else
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, InboundFolder, ModifiedFolder);
            }
            return directoryPath;
        }
        public static string ProcessedOrArchiveDirectoryFilePath(string syncFolder, bool IsProcessed = false, bool IsOutBound = false)
        {
            var directoryPath = string.Empty;
            var folderInboundOrOutBound = IsOutBound ? OutboundFolder : InboundFolder;
            if (IsProcessed)
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, folderInboundOrOutBound, ProcessedFolder);
            }
            else
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, folderInboundOrOutBound, ArchiveFolder);
            }
            return directoryPath;
        }

        public static string FailedDirectoryFilePath(string syncFolder, bool IsOutBound = false, bool IsFailed = false)
        {
            var directoryPath = string.Empty;
            var folderInboundOrOutBound = IsOutBound ? OutboundFolder : InboundFolder;
            if (IsFailed)
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, folderInboundOrOutBound, FailedFolder);
            }
            else
            {
                directoryPath = string.Concat(SftpServerPath, syncFolder, folderInboundOrOutBound, NewFolder);
            }

            return directoryPath;
        }

        public static string FilePathCreation(string fileName)
        {
            var filePath = string.Concat(fileName, DateTime.Now.ToString("yyyyMMdd_HHmmss"), CsvExtention);
            return filePath;
        }

        public static string ErrorFilePathCreation(string syncFolder)
        {
            var fileName = string.Empty;
            switch (syncFolder)
            {
                case DirectTradeTiketFolder:
                    fileName = TradeTicketNewCsv;
                    break;
                case DirectSaudaFolder:
                    fileName = SaudaHBCCreationCsv;
                    break;
                default:
                    break;
            }
            var filePath = string.Concat(fileName, DateTime.Now.ToString("yyyyMMdd_HHmmss"), CsvExtention);
            return filePath;
        }

        public static string SystemPath(string fileName, string folderName = "", bool IsInvoicePdf = false)
        {
            var filePath = string.Concat(CsvFilePath, @"\Upload\", fileName).Replace("\\bin\\Debug\\Adani.Solution.Console.exe", "").ToString();
            if (IsInvoicePdf)
            {
                if (folderName == InvoicePdf)
                {
                    folderName = "InvoicePdf";
                }

                filePath = string.Concat(PdfFilePath, @"\Upload\" + folderName + @"\", fileName).Replace("\\bin\\Debug\\Adani.Solution.Console.exe", "").ToString();
            }
            return filePath;
        }

        public static decimal StringToDecimalTryParse(string decimalValue)
        {
            decimal.TryParse(decimalValue, out decimal kilometro);
            return kilometro;
        }

        public static DateTime? DateTimeNullableTryParse(string stringDate)
        {
            DateTime? date;
            try
            {
                if (!string.IsNullOrEmpty(stringDate) && stringDate != "00000000")
                {
                    date = DateTime.ParseExact(stringDate, "yyyyMMdd", null);
                }
                else
                {
                    date = DateTime.MinValue;
                }
            }
            catch
            {
                date = DateTime.MinValue;
            }

            return date;
        }

        public static DateTime DateTimeTryParse(string stringDate)
        {
            DateTime date = DateTime.ParseExact(stringDate, "yyyyMMdd", null);
            return date;
        }

        public static DateTime? DateTimeTryParseNullable(string stringDate)
        {
            DateTime? date;
            try
            {
                if (!string.IsNullOrEmpty(stringDate) && stringDate != "00000000")
                {
                    date = DateTime.ParseExact(stringDate, "yyyyMMdd", null);
                }
                else
                {
                    date = null;
                }
            }
            catch
            {
                date = null;
            }

            return date;            
        }

        public static DateTime AddBusinessDays(DateTime saudaValidToDate, long ExtendedDays)
        {

            for (int i = 1; i <= ExtendedDays; i++)
            {
                saudaValidToDate = saudaValidToDate.AddDays(Constants.NumberOfDaysAddedToGetNextDate);
                if (saudaValidToDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    saudaValidToDate = saudaValidToDate.AddDays(Constants.NumberOfDaysAddedToGetNextDate);
                }
            }
            return saudaValidToDate;
        }

        public static bool IsPositive(this decimal number)
        {
            return number > 0;
        }
        public static bool IsNegative(this decimal number)
        {
            return number < 0;
        }

    }
}
