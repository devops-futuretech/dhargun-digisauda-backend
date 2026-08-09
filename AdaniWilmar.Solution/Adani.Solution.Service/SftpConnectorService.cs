using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using WinSCP;

namespace Adani.Solution.Service
{
    public interface ISftpConnectorService
    {
        SAPDataResponseDto UploadSFTPFile(List<string> FileList, string destinationPath);
        SAPDataResponseDto GetSFTPFile(string remotePath, string modelType);
        void RemoveSFTPFile(List<string> FileList);
        void CreateDirectorySFTPFile(List<string> FileList, string directoryName);
        void MoveSFTPFile(List<string> FileList, string directoryName);
        void DeleteLocalFile(List<string> FileList);
        void SyncProcessForSucessAndFailed(ResultDto response, string syncFolder, SAPDataResponseDto inputDto, string subject);
        void GetDataAsync(ResultDto response, string syncFolder, string subject, string csvFileName);
    }
    public class SftpConnectorService: ISftpConnectorService
    {
        private const string ServiceName = "Sftp Connector Service";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        private SessionOptions SessionData()
        {
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = ConsoleSettings.SftpServerName,
                SshPrivateKeyPath = ConsoleSettings.SystemPath(ConsoleSettings.SshPrivateKeyPath),
                UserName = ConsoleSettings.SftpUser,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            return sessionOptions;
        }

        #region Download and Upload Files to SFTP

        public SAPDataResponseDto UploadSFTPFile(List<string> FileList, string destinationPath)
        {
            _methodName = "UploadSFTPFile";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sapDataResponseDto = new SAPDataResponseDto();
            try
            {
                // Setup session options                
                var sessionOptions = SessionData();

                using (Session session = new Session())
                {

                    // Connect
                    session.Open(sessionOptions);
                    _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Message: SFTP Session Open Success");
                    if (!session.FileExists(destinationPath))
                    {
                        session.CreateDirectory(destinationPath);
                    }
                    // Download files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    TransferOperationResult transferResult;
                    foreach (var sourcePath in FileList)
                    {
                        transferResult = session.PutFiles(sourcePath, destinationPath, false, transferOptions);
                        // Throw on any error
                        transferResult.Check();
                    }
                }
                sapDataResponseDto.Status = true;
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                sapDataResponseDto.Status = false;
                sapDataResponseDto.Message = message;
            }
            return sapDataResponseDto;
        }



        public List<string> DownloadSFTPFile(string remotePath, List<string> fileNameList, string folderName = "", bool IsInvoicePdf = false)
        {
            _methodName = "DownloadSFTPFile";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var sapDataResponseDto = new SAPDataResponseDto();
            var sourcePath = string.Empty;
            var tempPath = string.Empty;
            var localFiles = new List<string>();
            try
            {
                // Setup session options
                var sessionOptions = SessionData();

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                    _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Message: SFTP Session Open Success");
                    foreach (var fileName in fileNameList)
                    {
                        tempPath = ConsoleSettings.SystemPath(fileName, folderName, IsInvoicePdf);
                        sourcePath = RemotePath.EscapeFileMask(remotePath + "/" + fileName);
                        if (session.FileExists(sourcePath))
                        {
                            session.GetFiles(sourcePath, tempPath).Check();
                        }
                        localFiles.Add(tempPath);
                    }
                }
                sapDataResponseDto.Status = true;
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception: {exception} Path: {tempPath} ";
                _logger.Error(message);
            }
            return localFiles;
        }

        #endregion

        #region Get SAP file and Convert CSV to Model

        public SAPDataResponseDto GetSFTPFile(string remotePath, string modelType)
        {
            _methodName = "GetSFTPFile";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} SAP Service ");
            var sapDataResponseDto = new SAPDataResponseDto();
            var sourcePath = string.Empty;
            var tempPath = string.Empty;
            try
            {
                // Setup session options
                var sessionOptions = SessionData();

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                    _logger.Info($"{ServiceName} Controller-Method {_methodName} Session open: Success");
                    //string remotePath = ConsoleSettings.SftpServerPath + folderPath;
                    // Retrieve a list of files in a remote directory
                    RemoteDirectoryInfo directory = session.ListDirectory(remotePath);
                    var files = directory.Files.Where(e => e.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase));
                    // Iterate the list
                    foreach (RemoteFileInfo fileInfo in files)
                    {
                        // Is it a file with .txt extension?
                        if (!fileInfo.IsDirectory &&
                            fileInfo.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            tempPath = ConsoleSettings.SystemPath(fileInfo.Name);
                            // Download the file to a temporary folder
                            sourcePath = RemotePath.EscapeFileMask(remotePath + "/" + fileInfo.Name);
                            if (session.FileExists(sourcePath))
                            {
                                session.GetFiles(sourcePath, tempPath).Check();
                                sapDataResponseDto = ConvertCsvToModel(modelType, tempPath, sapDataResponseDto, remotePath);
                                sapDataResponseDto.SourceFileName.Add(sourcePath);
                                sapDataResponseDto.LocalFileName.Add(tempPath);
                            }
                        }
                    }
                }
                sapDataResponseDto.Status = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception} SAP Service";
                _logger.Error(message);
                var FileList = new List<string>();
                FileList.Add(sourcePath);
                var directoryPath = ConsoleSettings.InboundDirectoryFilePath(modelType, true);
                MoveSFTPFile(FileList, directoryPath);
                sapDataResponseDto.Status = false;
                sapDataResponseDto.Message = message.ToString();
                sapDataResponseDto.LocalFileName.Add(tempPath);
            }
            return sapDataResponseDto;
        }

        private SAPDataResponseDto ConvertCsvToModel(string modelType, string filePath, SAPDataResponseDto sapDataResponseDto, string remotePath = "")
        {
            _methodName = "ConvertCsvToModel";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var lines = File.ReadAllLines(filePath).Skip(1).Select(x => x.Split(','));
            switch (modelType)
            {
                case ConsoleSettings.CustomerFolder:
                    var customerData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPCustomerDto
                      {
                          Code = x[0],
                          UserCode = x[1],
                          Name = !string.IsNullOrEmpty(x[3]) ? string.Concat(x[2], "-", x[3]) : x[2],
                          City = x[4],
                          Region = x[5],
                          Street = x[6],
                          ADRNR = x[7],
                          GSTN = x[8],
                          District = x[9],
                          DeliveringPlant = x[10],
                          MobileNumber = x[11],
                          Email = x[12],
                          State = x[13],
                          CentralDeletionFlag = x[14],
                          VerticalCode = x[15],
                          FSSAINumber = x[21],
                          RoleId = (int)Role.Dealer,
                          CustomerGroup = UtilityHelper.GetEnumDescription(CustomerGroup.Customer)
                      }).ToList();

                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var customerResponce = (List<SAPCustomerDto>)sapDataResponseDto.Response;
                        customerResponce.AddRange(customerData);
                        sapDataResponseDto.Response = customerResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = customerData;
                    }
                    break;
                case ConsoleSettings.BrokerFolder:

                    var brokerData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPCustomerDto
                      {
                          Code = x[0],
                          UserCode = x[1],
                          Name = !string.IsNullOrEmpty(x[3]) ? string.Concat(x[2], "-", x[3]) : x[2],
                          City = x[4],
                          Region = x[5],
                          Street = x[6],
                          ADRNR = x[7],
                          GSTN = x[8],
                          District = x[9],
                          DeliveringPlant = x[10],
                          MobileNumber = x[11],
                          Email = x[12],
                          State = x[13],
                          CentralDeletionFlag = x[14],
                          VerticalCode = x[15],
                          FSSAINumber = x[21],
                          RoleId = (int)Role.Broker,
                          CustomerGroup = UtilityHelper.GetEnumDescription(CustomerGroup.Broker)
                      }).ToList();

                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var brokerResponce = (List<SAPCustomerDto>)sapDataResponseDto.Response;
                        brokerResponce.AddRange(brokerData);
                        sapDataResponseDto.Response = brokerResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = brokerData;
                    }

                    break;
                case ConsoleSettings.ShipToParty:

                    var ShipToPartyData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPCustomerDto
                      {
                          Code = x[0],
                          UserCode = x[1],
                          Name = !string.IsNullOrEmpty(x[3]) ? string.Concat(x[2], "-", x[3]) : x[2],
                          City = x[4],
                          Region = x[5],
                          Street = x[6],
                          ADRNR = x[7],
                          GSTN = x[8],
                          District = x[9],
                          DeliveringPlant = x[10],
                          MobileNumber = x[11],
                          Email = x[12],
                          State = x[13],
                          CentralDeletionFlag = x[14],
                          VerticalCode = x[15],
                          FSSAINumber = x[21],
                          RoleId = (int)Role.ShipToParty,
                          CustomerGroup = string.Empty
                      }).ToList();

                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var ShipToPartyResponce = (List<SAPCustomerDto>)sapDataResponseDto.Response;
                        ShipToPartyResponce.AddRange(ShipToPartyData);
                        sapDataResponseDto.Response = ShipToPartyResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = ShipToPartyData;
                    }

                    break;
                case ConsoleSettings.TradeTicketFolder:

                    var tradeTicketData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new TradeTicketNumberDto
                      {
                          Id = UtilityHelper.LongTryToParse(x[0]),
                          TradeTicketNumber = x[1],
                          ErrorMessage = x[3]

                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var tradeTicketResponce = (List<TradeTicketNumberDto>)sapDataResponseDto.Response;
                        tradeTicketResponce.AddRange(tradeTicketData);
                        sapDataResponseDto.Response = tradeTicketResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = tradeTicketData;
                    }
                    break;
                case ConsoleSettings.SaudaFolder + "/" + ConsoleSettings.SaudaHBCFolder:

                    var saudaData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SaudaNumberDto
                      {
                          AppId = UtilityHelper.LongTryToParse(x[0]),
                          SaudaNumber = x[1],
                          ErrorMessage = x[4]
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaResponce = (List<SaudaNumberDto>)sapDataResponseDto.Response;
                        saudaResponce.AddRange(saudaData);
                        sapDataResponseDto.Response = saudaResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaData;
                    }
                    break;
                case ConsoleSettings.SaudaFolder + "/" + ConsoleSettings.SaudaSPFFolder:

                    var saudaSPFData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SaudaNumberDto
                      {
                          AppId = UtilityHelper.LongTryToParse(x[0]),
                          SaudaNumber = x[1],
                          ErrorMessage = x[4]

                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaResponce = (List<SaudaNumberDto>)sapDataResponseDto.Response;
                        saudaResponce.AddRange(saudaSPFData);
                        sapDataResponseDto.Response = saudaResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaSPFData;
                    }
                    break;
                case ConsoleSettings.SaudaLooseOilFolder:

                    var saudaLooseSPFData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SaudaNumberDto
                      {
                          AppId = UtilityHelper.LongTryToParse(x[0]),
                          SaudaNumber = x[1],
                          ErrorMessage = x[4]

                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaResponce = (List<SaudaNumberDto>)sapDataResponseDto.Response;
                        saudaResponce.AddRange(saudaLooseSPFData);
                        sapDataResponseDto.Response = saudaResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaLooseSPFData;
                    }
                    break;
                case ConsoleSettings.SaudaReleaseFolder:

                    var saudaReleaseSPFData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SaudaReleaseDto
                      {                         
                          SaudaNumber = x[0],
                          SaudaStatus = x[1]

                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaResponce = (List<SaudaReleaseDto>)sapDataResponseDto.Response;
                        saudaResponce.AddRange(saudaReleaseSPFData);
                        sapDataResponseDto.Response = saudaResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaReleaseSPFData;
                    }
                    break;
                case ConsoleSettings.InvoiceFolder:
                    var invoiceData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPInvoiceDto
                      {
                          BillingDocument = x[0],
                          UserCode = x[1],
                          Plant = x[2],
                          FromWarehouseId = x[3],
                          Mode = x[4],
                          NetValue = ConsoleSettings.StringToDecimalTryParse(x[5]),
                          BillDiscount = x[6],
                          BillDiscountType = x[7],
                          BillDate = ConsoleSettings.DateTimeTryParse(x[8]),
                          InvoiceDueDate = ConsoleSettings.DateTimeNullableTryParse(x[9]),
                          MaterialNumber = x[10],
                          QuantityInCase = ConsoleSettings.StringToDecimalTryParse(x[11]),
                          ActualBilledQuantity = ConsoleSettings.StringToDecimalTryParse(x[12]),
                          Discount = x[13],
                          DiscountType = x[14],
                          Status = x[15],
                          SKUInvoiceTax = ConsoleSettings.StringToDecimalTryParse(x[16]),
                          SalesDocumentType = x[17],
                          UnitPrice = x[18],
                          VechicleId = x[19],
                          DriverName = x[20],
                          DriverNumber = x[21],
                          GstAmount = x[22],
                          VerticalCode = x[23],
                          UOM = x[24],
                          SaudaNumber = x[25],
                          BatchNo = x[26],
                          DoNumber = x[27],
                          PdfUrl = ConsoleSettings.SystemPath(string.Concat(ConsoleSettings.InvoicePdf, x[0], ConsoleSettings.PdfExtention), ConsoleSettings.InvoicePdf, true),
                          PdfFileName = string.Concat(ConsoleSettings.InvoicePdf, x[0], ConsoleSettings.PdfExtention),
                      }).ToList();
                    //var pdfFileNameList = invoiceData.Select(_ => _.PdfFileName).ToList();
                    //DownloadSFTPFile(remotePath, pdfFileNameList, ConsoleSettings.InvoicePdf, true);
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaResponce = (List<SAPInvoiceDto>)sapDataResponseDto.Response;
                        saudaResponce.AddRange(invoiceData);
                        sapDataResponseDto.Response = saudaResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = invoiceData;
                    }
                    break;
                case ConsoleSettings.InvoiceCancelAndReturnFolder:
                    var invoiceCancelAndReturnData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPInvoiceDto
                      {
                          BillingDocument = x[0],
                          UserCode = x[1],
                          Plant = x[2],
                          FromWarehouseId = x[3],
                          Mode = x[4],
                          NetValue = ConsoleSettings.StringToDecimalTryParse(x[5]),
                          BillDiscount = x[6],
                          BillDiscountType = x[7],
                          BillDate = ConsoleSettings.DateTimeTryParse(x[8]),
                          InvoiceDueDate = ConsoleSettings.DateTimeNullableTryParse(x[9]),
                          MaterialNumber = x[10],
                          QuantityInCase = ConsoleSettings.StringToDecimalTryParse(x[11]),
                          ActualBilledQuantity = ConsoleSettings.StringToDecimalTryParse(x[12]),
                          Discount = x[13],
                          DiscountType = x[14],
                          Status = x[15],
                          SKUInvoiceTax = ConsoleSettings.StringToDecimalTryParse(x[16]),
                          SalesDocumentType = x[17],
                          UnitPrice = x[18],
                          VechicleId = x[19],
                          DriverName = x[20],
                          DriverNumber = x[21],
                          GstAmount = x[22],
                          VerticalCode = x[23],
                          UOM = x[24],
                          InvoiceCancelFlag = x[25].ToLower() == "x" ? true : false,
                          SaudaNumber = x[27],
                          BatchNo = x[28],
                          DoNumber = x[29],
                          ReturnFlag=true,
                          PdfUrl = ConsoleSettings.SystemPath(string.Concat(ConsoleSettings.InvoicePdf, x[0], ConsoleSettings.PdfExtention), ConsoleSettings.InvoicePdf, true),
                          PdfFileName = string.Concat(ConsoleSettings.InvoicePdf, x[0], ConsoleSettings.PdfExtention),
                      }).ToList();
                    var invoicePdfFileNameList = invoiceCancelAndReturnData.Where(_ => _.InvoiceCancelFlag == false).Select(_ => _.PdfFileName).ToList();
                    //DownloadSFTPFile(remotePath, invoicePdfFileNameList, ConsoleSettings.InvoicePdf, true);
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaResponce = (List<SAPInvoiceDto>)sapDataResponseDto.Response;
                        saudaResponce.AddRange(invoiceCancelAndReturnData);
                        sapDataResponseDto.Response = saudaResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = invoiceCancelAndReturnData;
                    }
                    break;

                case ConsoleSettings.LiftingRequestFolder:

                    var liftingRequestData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new LiftingRequestDeliveryOrderNumberDto
                      {
                          Id = UtilityHelper.LongTryToParse(x[0]),
                          DeliveryOrderNumber = x[1],
                          SaudaNumber = x[2],
                          ContractQuantity = ConsoleSettings.StringToDecimalTryParse(x[3]),
                          PendingQuantity = ConsoleSettings.StringToDecimalTryParse(x[4]),
                          LiftingQuantity = ConsoleSettings.StringToDecimalTryParse(x[5]),
                          ErrorMessage = x[8]

                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var liftingRequestResponce = (List<LiftingRequestDeliveryOrderNumberDto>)sapDataResponseDto.Response;
                        liftingRequestResponce.AddRange(liftingRequestData);
                        sapDataResponseDto.Response = liftingRequestResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = liftingRequestData;
                    }
                    break;
                case ConsoleSettings.SaudaLimitFolder:

                    var saudaLimitData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPSaudaLimitDto
                      {
                          CustomerCode = x[0],
                          CustomerName = x[1],
                          VerticalCode = x[2],
                          PendCont = ConsoleSettings.StringToDecimalTryParse(x[3]),
                          PendDO = ConsoleSettings.StringToDecimalTryParse(x[4]),
                          PendOBD = ConsoleSettings.StringToDecimalTryParse(x[5])
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaLimitResponce = (List<SAPSaudaLimitDto>)sapDataResponseDto.Response;
                        saudaLimitResponce.AddRange(saudaLimitData);
                        sapDataResponseDto.Response = saudaLimitResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaLimitData;
                    }
                    break;
                case ConsoleSettings.SKUMasterFolder:

                    var skuData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPSkuDto
                      {
                          SkuCode = x[0],
                          MaterialDescription = x[1],
                          ConvertionType = x[2],
                          ConvertionFactor = ConsoleSettings.StringToDecimalTryParse(x[3]),
                          VerticalCode = x[4],
                          SalesDivision = x[5],
                          MaterialGroup1 = x[6],
                          OilTypeCode = x[7],
                          VerticalGroupCode = x[8],
                          PackTypeCode = x[9],
                          MaterialType = x[11],
                          PackGroups = x[12]
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var skuResponce = (List<SAPSkuDto>)sapDataResponseDto.Response;
                        skuResponce.AddRange(skuData);
                        sapDataResponseDto.Response = skuResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = skuData;
                    }
                    break;
                case ConsoleSettings.CreditMasterFolder:

                    var creditMasterData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPCreditMasterDto
                      {
                          CustomerCode = x[0],
                          CCreditArea= x[1],
                          CreditAccountNumber=x[2],
                          RiskCat= x[3],
                          Curr=x[4],
                          CreditLimit = ConsoleSettings.StringToDecimalTryParse(x[5]),
                          CreditExposure = ConsoleSettings.StringToDecimalTryParse(x[6]),
                          SalesValue = ConsoleSettings.StringToDecimalTryParse(x[7]),
                          TotalReceivable = ConsoleSettings.StringToDecimalTryParse(x[8]),
                          SaudaDepC = ConsoleSettings.StringToDecimalTryParse(x[9]),
                          SecDepH = ConsoleSettings.StringToDecimalTryParse(x[10]),
                          BankGuarM = ConsoleSettings.StringToDecimalTryParse(x[11]),
                          AdvanceA= ConsoleSettings.StringToDecimalTryParse(x[12]),
                          DueToday = ConsoleSettings.StringToDecimalTryParse(x[13]),
                          TomorrowsDue = ConsoleSettings.StringToDecimalTryParse(x[14]),
                          Overdue = ConsoleSettings.StringToDecimalTryParse(x[15]),
                          NotDue = ConsoleSettings.StringToDecimalTryParse(x[16]),
                          NextIntRev = x[17],
                          Blocked = x[18],
                          TotalLimit = ConsoleSettings.StringToDecimalTryParse(x[19]),
                          IndividLimit = ConsoleSettings.StringToDecimalTryParse(x[20]),
                          AvailableCreditLimit = ConsoleSettings.StringToDecimalTryParse(x[21]),

                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var creditMasterResponce = (List<SAPCreditMasterDto>)sapDataResponseDto.Response;
                        creditMasterResponce.AddRange(creditMasterData);
                        //creditMasterResponce = (from cm in creditMasterResponce
                        //                        group cm by cm.CustomerCode into groupResult
                        //                        select new SAPCreditMasterDto
                        //                        {
                        //                            CreditLimit = groupResult.Sum(f => f.CreditLimit > 0 ? f.CreditLimit : f.CreditLimit),
                        //                            TotalReceivable = groupResult.Sum(f => f.TotalReceivable > 0 ? f.TotalReceivable : f.TotalReceivable),
                        //                            SpecialLiabil = groupResult.Sum(f => f.SpecialLiabil > 0 ? f.SpecialLiabil : f.SpecialLiabil),
                        //                            SalesValues = groupResult.Sum(f => f.SalesValues > 0 ? f.SalesValues : f.SalesValues),
                        //                            CreditExposure = groupResult.Sum(f => f.CreditExposure > 0 ? f.CreditExposure : f.CreditExposure),
                        //                            CreditLimitPercentage = groupResult.Sum(f => f.CreditLimitPercentage > 0 ? f.CreditLimitPercentage : f.CreditLimitPercentage),
                        //                            CustomerCode = groupResult.Key
                        //                        }).ToList();
                        sapDataResponseDto.Response = creditMasterResponce;
                    }
                    else
                    {
                        //creditMasterData = (from cm in creditMasterData
                        //                    group cm by cm.CustomerCode into groupResult
                        //                    select new SAPCreditMasterDto
                        //                    {
                        //                        CreditLimit = groupResult.Sum(f => f.CreditLimit > 0 ? f.CreditLimit : f.CreditLimit),
                        //                        TotalReceivable = groupResult.Sum(f => f.TotalReceivable > 0 ? f.TotalReceivable : f.TotalReceivable),
                        //                        SpecialLiabil = groupResult.Sum(f => f.SpecialLiabil > 0 ? f.SpecialLiabil : f.SpecialLiabil),
                        //                        SalesValues = groupResult.Sum(f => f.SalesValues > 0 ? f.SalesValues : f.SalesValues),
                        //                        CreditExposure = groupResult.Sum(f => f.CreditExposure > 0 ? f.CreditExposure : f.CreditExposure),
                        //                        CreditLimitPercentage = groupResult.Sum(f => f.CreditLimitPercentage > 0 ? f.CreditLimitPercentage : f.CreditLimitPercentage),
                        //                        CustomerCode = groupResult.Key
                        //                    }).ToList();
                        sapDataResponseDto.Response = creditMasterData;
                    }
                    break;
                case ConsoleSettings.DepotMasterFolder:

                    var depotData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPDepotDto
                      {
                          PlantCode = x[0],
                          Name = !string.IsNullOrEmpty(x[2]) ? string.Concat(x[1], "-", x[2]) : x[1],
                          ADRNR = x[3],
                          Street1 = x[4],
                          Region = x[5],
                          City = x[6],
                          CentralArchiving = x[7],
                          StateName = x[8],
                          Street2 = x[9],
                          TelephoneNumber = x[10],
                          Email = x[11],
                          TaxNumber = x[12],
                          IsPlant = x[13].ToLower() == "x" ? false : true
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var depotResponce = (List<SAPDepotDto>)sapDataResponseDto.Response;
                        depotResponce.AddRange(depotData);
                        sapDataResponseDto.Response = depotResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = depotData;
                    }
                    break;
                case ConsoleSettings.SAPFileMove:
                    var sapFileData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPfileMove
                      {
                          DestinationPath = x[0],
                          SourcePath = x[1],
                          FileName = x[2],
                          IsSuccess = string.IsNullOrEmpty(x[3]) ? false : true
                      }).ToList();

                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var sapFileResponce = (List<SAPfileMove>)sapDataResponseDto.Response;
                        sapFileResponce.AddRange(sapFileData);
                        sapDataResponseDto.Response = sapFileResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = sapFileData;
                    }
                    break;
                case ConsoleSettings.CustomerLedgerFolder:

                    var customerLedgerData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPCustomerLedgerDto
                      {
                          UserCode = x[0],
                          PdfUrl = ConsoleSettings.SystemPath(x[1], ConsoleSettings.CustomerLedgerPdfFolder, true),
                          PdfFileName = x[1],

                      }).ToList();
                    var customerLedgerPdfFileNameList = customerLedgerData.Select(_ => _.PdfFileName).ToList();
                    DownloadSFTPFile(remotePath, customerLedgerPdfFileNameList, ConsoleSettings.CustomerLedgerPdfFolder, true);
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var customerLedgerResponce = (List<SAPCustomerLedgerDto>)sapDataResponseDto.Response;
                        customerLedgerResponce.AddRange(customerLedgerData);
                        sapDataResponseDto.Response = customerLedgerResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = customerLedgerData;
                    }
                    break;
                case ConsoleSettings.DODeleteFolder:

                    var doDeleteData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPDoDeleteDto
                      {
                          DONumber = x[0],
                          Status = x[1]
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var doDeleteResponce = (List<SAPDoDeleteDto>)sapDataResponseDto.Response;
                        doDeleteResponce.AddRange(doDeleteData);
                        sapDataResponseDto.Response = doDeleteResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = doDeleteData;
                    }
                    break;
                case ConsoleSettings.DOUpdateFolder:

                    var doUpdateData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPDoUpdateDto
                      {
                          DONumber = x[0],
                          SoldToParty = x[1],
                          ShipToParty = x[2],
                          Payer = x[3],
                          BillToParty = x[4],
                          Vertical = x[5],
                          OrderQuantity = ConsoleSettings.StringToDecimalTryParse(x[6]),
                          Uom = x[7],
                          MaterialNumber = x[8],
                          SaudaNumber = x[9],
                          Enquiry=x[10],
                          Reason=x[11]
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var doUpdateResponce = (List<SAPDoUpdateDto>)sapDataResponseDto.Response;
                        doUpdateResponce.AddRange(doUpdateData);
                        sapDataResponseDto.Response = doUpdateResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = doUpdateData;
                    }
                    break;
                case ConsoleSettings.SaudaAmendmentFolder:
                    var saudaAmendmentData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPSaudaAmendmentDto
                      {
                          SaudaNumber = x[0],
                          SaudaOrderId = UtilityHelper.LongTryToParse(x[1]),
                          SkuCode = x[2],
                          Quantity = ConsoleSettings.StringToDecimalTryParse(x[3]),
                          DepotCode = x[4],
                          INCO1 = x[5],
                          INCO2 = x[6],
                          ToDate = ConsoleSettings.DateTimeTryParse(x[7]),
                          SoldToParty = x[8],
                          ShipToParty = x[9],
                          Payer = x[10],
                          BillToParty = x[11],
                          //Remarks = x[12],
                          Uom = x[13],
                          Vertical = x[14],
                          BidAmount= ConsoleSettings.StringToDecimalTryParse(x[15]),
                          Rate1 = ConsoleSettings.StringToDecimalTryParse(x[16])
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaAmendmentResponce = (List<SAPSaudaAmendmentDto>)sapDataResponseDto.Response;
                        saudaAmendmentResponce.AddRange(saudaAmendmentData);
                        sapDataResponseDto.Response = saudaAmendmentResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaAmendmentData;
                    }
                    break;
                case ConsoleSettings.DirectSaudaFolder:
                    var saudaCreateData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SaudaViewDto
                      {
                          SaudaNumber = x[0],
                          SOType = x[1],
                          VerticalName = x[2],
                          CustomerPoNumber = x[3],
                          ValidFrom = ConsoleSettings.DateTimeNullableTryParse(x[5]).Value,
                          ValidTo = ConsoleSettings.DateTimeNullableTryParse(x[6]).Value,
                          SoldToParty = x[7],
                          ShipToParty = x[8],
                          Sku = x[9],
                          BidQuantity = ConsoleSettings.StringToDecimalTryParse(x[10]),
                          DocumentDate = ConsoleSettings.DateTimeNullableTryParse(x[11]).Value,
                          CustomerGroup = x[12],
                          //PriceListType = x[13],
                          PriceGroup = x[14],
                          Usage = x[15],
                          INCO1 = x[16],
                          INCO2 = x[17],
                          BillDate = ConsoleSettings.DateTimeNullableTryParse(x[18]).Value,
                          DeliveryPriority = x[19],
                          UserDepotMapping = x[20],
                          PickingPoint = x[21],
                          MaximumNumberDeliveries = UtilityHelper.IntTryToParse(x[22]),
                          TradeTicketNumber = x[23],
                          ConditionType1 = x[24],
                          BidAmount = ConsoleSettings.StringToDecimalTryParse(x[25]),
                          ConditionType2 = x[26],
                          Rate2 = ConsoleSettings.StringToDecimalTryParse(x[27]),
                          CustomerPOType = x[28],
                          Uom = x[29],
                          ConditionType3 = x[30],
                          Rate3 = ConsoleSettings.StringToDecimalTryParse(x[31]),
                          ConditionType4 = x[32],
                          Rate4 = ConsoleSettings.StringToDecimalTryParse(x[33])
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaAmendmentResponce = (List<SaudaViewDto>)sapDataResponseDto.Response;
                        saudaAmendmentResponce.AddRange(saudaCreateData);
                        sapDataResponseDto.Response = saudaAmendmentResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaCreateData;
                    }
                    break;
                case ConsoleSettings.DirectTradeTiketFolder:
                    var tradeTicketCreateData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Where(x => x != null && x.Any())
                      .Select(x => new SAPTradeTicketViewDto
                      {
                          TradeTicketNumber = x[0],
                          ContractType = x[1],
                          BookingType = x[2],
                          MaterialType = x[3],
                          //MATERIAL_TYPE1 = x[4],
                          //MATERIAL_TYPE2 = x[5],
                          //MATERIAL_TYPE3 = x[6],
                          //MATERIAL_TYPE4 = x[7],
                          //MATERIAL_TYPE5 = x[8],
                          //PRICE1 = ConsoleSettings.StringToDecimalTryParse(x[9]),
                          //PRICE2 = ConsoleSettings.StringToDecimalTryParse(x[10]),
                          //PRICE3 = ConsoleSettings.StringToDecimalTryParse(x[11]),
                          //PRICE4 = ConsoleSettings.StringToDecimalTryParse(x[12]),
                          //PRICE5 = ConsoleSettings.StringToDecimalTryParse(x[13]),
                          //PRCOST1 = ConsoleSettings.StringToDecimalTryParse(x[14]),
                          //PRCOST2 = ConsoleSettings.StringToDecimalTryParse(x[15]),
                          //PRCOST3 = ConsoleSettings.StringToDecimalTryParse(x[16]),
                          //PRCOST4 = ConsoleSettings.StringToDecimalTryParse(x[17]),
                          //PRCOST5 = ConsoleSettings.StringToDecimalTryParse(x[18]),
                          //PROPORTION1 = ConsoleSettings.StringToDecimalTryParse(x[19]),
                          //PROPORTION2 = ConsoleSettings.StringToDecimalTryParse(x[20]),
                          //PROPORTION3 = ConsoleSettings.StringToDecimalTryParse(x[21]),
                          //PROPORTION4 = ConsoleSettings.StringToDecimalTryParse(x[22]),
                          //PROPORTION5 = ConsoleSettings.StringToDecimalTryParse(x[23]),
                          ContractQuantity = ConsoleSettings.StringToDecimalTryParse(x[24]),
                          UnitOfMeasure = x[25],
                          PlantOrVendor = x[26],
                          ContractDate = ConsoleSettings.DateTimeTryParse(x[27]),
                          ValidFrom = x[28] != "0" && x[28] != "" ? ConsoleSettings.DateTimeNullableTryParse(x[28]) : (DateTime?)null,
                          ValidTo = x[29] != "0" && x[29] != "" ? ConsoleSettings.DateTimeNullableTryParse(x[29]) : (DateTime?)null,
                          OtherElement = ConsoleSettings.StringToDecimalTryParse(x[30])
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaAmendmentResponce = (List<SAPTradeTicketViewDto>)sapDataResponseDto.Response;
                        saudaAmendmentResponce.AddRange(tradeTicketCreateData);
                        sapDataResponseDto.Response = saudaAmendmentResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = tradeTicketCreateData;
                    }
                    break;
                case ConsoleSettings.DirectTradeTiketSFFolder:
                    var tradeTicketSFCreateData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Where(x => x != null && x.Any())
                      .Select(x => new SAPTradeTicketViewDto
                      {
                          TradeTicketNumber = x[0],
                          ContractType = x[1],
                          BookingType = x[2],
                          MaterialType = x[3],
                          //MATERIAL_TYPE1 = x[4],
                          //MATERIAL_TYPE2 = x[5],
                          //MATERIAL_TYPE3 = x[6],
                          //MATERIAL_TYPE4 = x[7],
                          //MATERIAL_TYPE5 = x[8],
                          //MATERIAL_TYPE6 = x[9],
                          //MATERIAL_TYPE7 = x[10],
                          //MATERIAL_TYPE8 = x[11],
                          //MATERIAL_TYPE9 = x[12],
                          //MATERIAL_TYPE10 = x[13],
                          //PRICE1 = ConsoleSettings.StringToDecimalTryParse(x[14]),
                          //PRICE2 = ConsoleSettings.StringToDecimalTryParse(x[15]),
                          //PRICE3 = ConsoleSettings.StringToDecimalTryParse(x[16]),
                          //PRICE4 = ConsoleSettings.StringToDecimalTryParse(x[17]),
                          //PRICE5 = ConsoleSettings.StringToDecimalTryParse(x[18]),
                          //PRICE6 = ConsoleSettings.StringToDecimalTryParse(x[19]),
                          //PRICE7 = ConsoleSettings.StringToDecimalTryParse(x[20]),
                          //PRICE8 = ConsoleSettings.StringToDecimalTryParse(x[21]),
                          //PRICE9 = ConsoleSettings.StringToDecimalTryParse(x[22]),
                          //PRICE10 = ConsoleSettings.StringToDecimalTryParse(x[23]),
                          //PRCOST1 = ConsoleSettings.StringToDecimalTryParse(x[24]),
                          //PRCOST2 = ConsoleSettings.StringToDecimalTryParse(x[25]),
                          //PRCOST3 = ConsoleSettings.StringToDecimalTryParse(x[26]),
                          //PRCOST4 = ConsoleSettings.StringToDecimalTryParse(x[27]),
                          //PRCOST5 = ConsoleSettings.StringToDecimalTryParse(x[28]),
                          //PRCOST6 = ConsoleSettings.StringToDecimalTryParse(x[29]),
                          //PRCOST7 = ConsoleSettings.StringToDecimalTryParse(x[30]),
                          //PRCOST8 = ConsoleSettings.StringToDecimalTryParse(x[31]),
                          //PRCOST9 = ConsoleSettings.StringToDecimalTryParse(x[32]),
                          //PRCOST10 = ConsoleSettings.StringToDecimalTryParse(x[33]),
                          //PROPORTION1 = ConsoleSettings.StringToDecimalTryParse(x[34]),
                          //PROPORTION2 = ConsoleSettings.StringToDecimalTryParse(x[35]),
                          //PROPORTION3 = ConsoleSettings.StringToDecimalTryParse(x[36]),
                          //PROPORTION4 = ConsoleSettings.StringToDecimalTryParse(x[37]),
                          //PROPORTION5 = ConsoleSettings.StringToDecimalTryParse(x[38]),
                          //PROPORTION6 = ConsoleSettings.StringToDecimalTryParse(x[39]),
                          //PROPORTION7 = ConsoleSettings.StringToDecimalTryParse(x[40]),
                          //PROPORTION8 = ConsoleSettings.StringToDecimalTryParse(x[41]),
                          //PROPORTION9 = ConsoleSettings.StringToDecimalTryParse(x[42]),
                          //PROPORTION10 = ConsoleSettings.StringToDecimalTryParse(x[43]),
                          ContractQuantity = ConsoleSettings.StringToDecimalTryParse(x[44]),
                          UnitOfMeasure = x[45],
                          PlantOrVendor = x[46],
                          ContractDate = ConsoleSettings.DateTimeTryParse(x[47]),
                          ValidFrom = x[48] != "0" && x[48] != "" ? ConsoleSettings.DateTimeNullableTryParse(x[48]) : (DateTime?)null,
                          ValidTo = x[49] != "0" && x[49] != "" ? ConsoleSettings.DateTimeNullableTryParse(x[49]) : (DateTime?)null,
                          OtherElement = ConsoleSettings.StringToDecimalTryParse(x[50])
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaAmendmentResponce = (List<SAPTradeTicketViewDto>)sapDataResponseDto.Response;
                        saudaAmendmentResponce.AddRange(tradeTicketSFCreateData);
                        sapDataResponseDto.Response = saudaAmendmentResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = tradeTicketSFCreateData;
                    }
                    break;               
                case ConsoleSettings.InvoicePaymentStatus:
                    var invoicePaymentData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPInvoiceStatusDto
                      {
                          InvoiceNumber = x[0],
                          PaymentStatus = x[1],
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var invoicePaymentResponce = (List<SAPInvoiceStatusDto>)sapDataResponseDto.Response;
                        invoicePaymentResponce.AddRange(invoicePaymentData);
                        sapDataResponseDto.Response = invoicePaymentResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = invoicePaymentData;
                    }
                    break;

                case ConsoleSettings.LiftingInquiry:
                    var liftingRequestDetailData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new LiftingRequestInquiryNumberDto
                      {
                          LiftingRequestId = Convert.ToInt64(x[0]),
                          EnquiryNumber = x[1],
                          //Status = x[2],
                          Message = x[3]
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var invoicePaymentResponce = (List<LiftingRequestInquiryNumberDto>)sapDataResponseDto.Response;
                        invoicePaymentResponce.AddRange(liftingRequestDetailData);
                        sapDataResponseDto.Response = invoicePaymentResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = liftingRequestDetailData;
                    }
                    break;
                case ConsoleSettings.PendingContract:
                    var pendingContractData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new PendingContractDto
                      {
                          SaudaNumber = x[0],
                          PlantCode = x[1],
                          PlantName = x[2],
                          RecordCreatedDate = x[3] != "0" && x[3] != "" ? ConsoleSettings.DateTimeTryParseNullable(x[3]) : (DateTime?)null,
                          SalesOrganization = x[4],
                          SalesOrgDescription = x[5],
                          ContractValidFrom = x[6] != "0" && x[6] != "" ? ConsoleSettings.DateTimeTryParseNullable(x[6]) : (DateTime?)null ,
                          ContractValidTo = x[7] != "0" && x[7] != "" ? ConsoleSettings.DateTimeTryParseNullable(x[7]) : (DateTime?)null,
                          PONumber = x[8],
                          SaudaDate = x[9] != "0" && x[9] != "" ? ConsoleSettings.DateTimeTryParseNullable(x[9]) : (DateTime?)null,
                          IncoTerms1 = x[10],
                          Tax = x[11],
                          BrokerCode = x[12],
                          Place = x[13],
                          BrokerName = x[14],
                          BrokerCity = x[15],
                          BrokerRegionDescription = x[16],
                          CustomerCode = x[17],
                          CustomerName = x[18],
                          CustomerCity = x[19],
                          CustomerRegionDescription = x[20],
                          CustomerRegionalMarket = x[21],
                          MaterialCode = x[22],
                          CustomerMaterialCode = x[23],
                          MaterialGroup = x[24],
                          MaterialDescription1 = x[25],
                          MaterialDescription2 = x[26],
                          Location = x[27],
                          BasicRate = ConsoleSettings.StringToDecimalTryParse(x[28]),
                          Discount = ConsoleSettings.StringToDecimalTryParse(x[29]),
                          BasicRateAfterDiscount= ConsoleSettings.StringToDecimalTryParse(x[30]),
                          PR00 = x[31],                          
                          ZDC1 = x[32],
                          ZDC2 = x[33],
                          ZPU1 = x[34],
                          ZPU2 = x[35],
                          FRC1 = x[36],
                          FRC2 = x[37],
                          JINSVALUE = x[38],
                          DespatchQty = ConsoleSettings.StringToDecimalTryParse(x[39]),                          
                          PendingQuantityInCase = ConsoleSettings.StringToDecimalTryParse(x[40]),
                          SaudaQuantity = ConsoleSettings.StringToDecimalTryParse(x[41]),
                          UOM = x[42],
                          PendingQuantityInMT = ConsoleSettings.StringToDecimalTryParse(x[43]),
                          ContractType = x[44],
                          PartnerFunction = x[45],
                          Description = x[46],
                          ReleaseStatus= x[47],
                          MaterialGroup1 = x[48],
                          MaterialGroupDescription1 = x[49],
                          MaterialGroup2 = x[50],
                          MaterialGroupDescription2 = x[51],
                          MaterialGroup3 = x[52],
                          MaterialGroupDescription3 = x[53],
                          MaterialGroup4 = x[54],
                          MaterialGroupDescription4 = x[55],
                          MaterialGroup5 = x[56],   
                          MaterialGroupDescription5 = x[57],
                          UsageDescription = x[58],
                          CreatedPerson = x[59],
                          UsegeIndicator = x[60],
                          TermsOfPaymentKey = x[61]                         
                      }).ToList();

                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var customerResponce = (List<PendingContractDto>)sapDataResponseDto.Response;
                        customerResponce.AddRange(pendingContractData);
                        sapDataResponseDto.Response = customerResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = pendingContractData;
                    }
                    break;

                case ConsoleSettings.SalesReport:
                    var salesReportData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SalesRegisterOutputDto
                      {
                          Payer = x[0],
                          PlantCode = x[1],
                          PlantNameOne = x[2],
                          CityPincode = x[3],
                          PlantSAPStateCode = x[4],
                          PlantGSTStateCode = x[5],
                          PlantSAPStateDescription = x[6],
                          DeliveryNo = x[7],
                          BatchItemNo = x[8],
                          MaterialCode = x[9],
                          MaterialPricingGrp = x[10],
                          MaterialPricingTxt = x[11],
                          OilTypeOne = x[12],
                          OilTypeDesc = x[13],
                          Vertical = x[14],
                          VerticalDesc = x[15],
                          Brand = x[16],
                          BrandDesc = x[17],
                          Packtype = x[18],
                          PacktypeDesc = x[19],
                          ItemName = x[20],
                          OilTypeTwo = x[21],
                          SalesOrganization = x[22],
                          DistributionChannel = x[23],
                          OrderType = x[24],
                          BillNumber = x[25],
                          CustGrp = x[26],
                          PriceZone = x[27],
                          SourceLocation = x[28],
                          PriceList = x[29],
                          BillingType = x[30],
                          StorageLocation = x[31],
                          PlantNameTwo = x[32],
                          BatchNumber = x[33],
                          MfgDate = x[34],
                          QuantityinSKU = x[35],
                          UOM = x[36],
                          SalesUOM = x[37],
                          QuantityCase = ConsoleSettings.StringToDecimalTryParse(x[38]),
                          QuantityMT = ConsoleSettings.StringToDecimalTryParse(x[39]),
                          QuantityKG = ConsoleSettings.StringToDecimalTryParse(x[40]),
                          RateperSKU = x[41],
                          ValueinPR00 = x[42],
                          MaterialReturnValue = x[43],
                          DocCurrency = x[44],
                          TradeDiscount = x[45],
                          QuantityDiscount = x[46],
                          SpecialDiscount = x[47],
                          OtherDiscount = x[48],
                          CashDiscount = x[49],
                          FrieghtDiscount = x[50],
                          TaxableAmount = x[51],
                          TotalvalueBeforeGST = x[52],
                          NetReturnValue = x[53],
                          NetTaxAmount = x[54],
                          BillingDate = ConsoleSettings.DateTimeTryParseNullable(x[55]),
                          BillingTime = x[56],
                          SoldtoPartyCountry = x[57],
                          ShipTo = x[58],
                          ShipToPartyDescription = x[59],
                          ShipToPartySAPStateCode = x[60],
                          StateofShipparty = x[61],
                          ShipToPartyGSTStateCode = x[62],
                          ShipToPartyGSTNO = x[63],
                          SoldToParty = x[64],
                          SoldToPartyDescription = x[65],
                          StateofSoldparty = x[66],
                          BillToParty = x[67],
                          BillToPartyDescription = x[68],
                          BillToPartySAPStateCode = x[69],
                          StateofBillparty = x[70],
                          BillToPartyGSTStateCode = x[71],
                          BillToPartyGSTNO = x[72],
                          SalesOrderNo = x[73],
                          PONo = x[74],
                          PODate = x[75],
                          FreightTerms = x[76],
                          Contractnumber = x[77],
                          ContractDate = x[78],
                          VAT = x[79],
                          CST = x[80],
                          INS = x[81],
                          AgriTax = x[82],
                          freight = x[83],
                          freightDiff = x[84],
                          Entrytax = x[85],
                          ExiseDuty = x[86],
                          RoundOff = x[87],
                          Discount = x[88],
                          SAT = x[89],
                          FirstBrokerageRate = x[90],
                          FirstBrokerage = x[91],
                          SecondBrokerName = x[92],
                          SecondBrokerageRate = x[93],
                          SecondBrokerage = x[94],
                          VATSurcharge = x[95],
                          SGST = x[96],
                          CGST = x[97],
                          IGST = x[98],
                          UGST = x[99],
                          SGSTPercentage = x[100],
                          CGSTPercentage = x[101],
                          IGSTPercentage = x[102],
                          UGSTPercentage = x[103],
                          CompCess = x[104],
                          TotalGST = x[105],
                          TotalValueWithGST = x[106],
                          WaybillNo = x[107],
                          Vehicleno = x[108],
                          Transporter = x[109],
                          TransporterName = x[110],
                          FrieghtFRC3 = x[111],
                          Insuranceandpackingcharges = UtilityHelper.IntTryToParse(x[112]),
                          PrimaryDiscount = x[113],
                          ContractDescriptionColumn = x[114],
                          Transportationzone = x[115],
                          TzoneDescription = x[116],
                          ShipTOPartyZone = x[117],
                          BillTOPartyZone = x[118],
                          ContractValidfrom = ConsoleSettings.DateTimeTryParseNullable(x[119]),
                          ContractValidto = ConsoleSettings.DateTimeTryParseNullable(x[120]),
                          LiquidationDisc = x[121],
                          TTNumber = x[122],
                          MRPValue = x[123],
                          FromDate = ConsoleSettings.DateTimeTryParseNullable(x[124]),
                          ToDate = ConsoleSettings.DateTimeTryParseNullable(x[125])
                      }).ToList();

                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var customerResponce = (List<SalesRegisterOutputDto>)sapDataResponseDto.Response;
                        customerResponce.AddRange(salesReportData);
                        sapDataResponseDto.Response = customerResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = salesReportData;
                    }
                    break;
                case ConsoleSettings.SaudaConversion:
                    var saudaConversionData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPSaudaConversionDto
                      {
                          SaudaConversionSkusId= UtilityHelper.LongTryToParse(x[0]),
                          SaudaNumber = x[1],                          
                          SkuCode = x[2],
                          Quantity = ConsoleSettings.StringToDecimalTryParse(x[3]),
                          BaseRate = ConsoleSettings.StringToDecimalTryParse(x[4]),
                          Status = x[6] == "S" ? true : false,
                          SaudaType = x[7] == "X" ? false : true,
                          Remarks = x[8],
                          TradeTicketNumber = x[9]
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaConversionResponce = (List<SAPSaudaConversionDto>)sapDataResponseDto.Response;
                        saudaConversionResponce.AddRange(saudaConversionData);
                        sapDataResponseDto.Response = saudaConversionResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaConversionData;
                    }
                    break;
                case ConsoleSettings.SaudaExtension:
                    var saudaExtensionData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPSaudaExtension
                      {                         
                          SaudaNumber = x[0],
                          Status = x[1] == "S" ? true : false,                        
                          Remarks = x[2]
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaExtensionResponce = (List<SAPSaudaExtension>)sapDataResponseDto.Response;
                        saudaExtensionResponce.AddRange(saudaExtensionData);
                        sapDataResponseDto.Response = saudaExtensionResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = saudaExtensionData;
                    }
                    break;
                case ConsoleSettings.ChequeStatus:
                    var chequeStatusData = File.ReadAllLines(filePath)
                      .Skip(1)
                      .Select(x => x.Split(ConsoleSettings.SapDelimiter))
                      .Select(x => new SAPChequeStatusDto
                      {
                          ControllingArea = x[0],
                          DealerCode = x[1],
                          DealerName = x[2],
                          ChequeNo = x[3],
                          NameOfBank = x[4],
                          BranchName = x[5] 
                      }).ToList();
                    if (sapDataResponseDto != null && sapDataResponseDto.Response != null && !string.IsNullOrEmpty(sapDataResponseDto.Response.ToString()))
                    {
                        var saudaConversionResponce = (List<SAPChequeStatusDto>)sapDataResponseDto.Response;
                        saudaConversionResponce.AddRange(chequeStatusData);
                        sapDataResponseDto.Response = saudaConversionResponce;
                    }
                    else
                    {
                        sapDataResponseDto.Response = chequeStatusData;
                    }
                    break;
                default:
                    break;
            }

            return sapDataResponseDto;
        }

        #endregion

        #region Move Remove and Delete Local and SFTP Files
        /// <summary>
        /// Method to delete file
        /// </summary>       
        /// <returns></returns>
        public void DeleteLocalFile(List<string> FileList = null)
        {
            _methodName = "DeleteLocalFile";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            try
            {
                if(FileList != null && FileList.Any())
                {
                    foreach (var filePath in FileList)
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }                
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

        }

        public void RemoveSFTPFile(List<string> FileList)
        {
            _methodName = "RemoveSFTPFile";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var sftpDataResult = new SftpDataResult();
            try
            {
                var sessionOptions = SessionData();
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);
                    _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Message: SFTP Session Open Success");
                    foreach (var filePath in FileList)
                    {
                        if (session.FileExists(filePath))
                        {
                            session.RemoveFiles(filePath);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }

        public void CreateDirectorySFTPFile(List<string> FileList, string directoryName)
        {
            _methodName = "CreateDirectorySFTPFile";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sftpDataResult = new SftpDataResult();
            try
            {
                UploadSFTPFile(FileList, directoryName);
                RemoveSFTPFile(FileList);
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }

        public void MoveSFTPFile(List<string> FileList, string directoryName)
        {
            _methodName = "MoveSFTPFile";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var sftpDataResult = new SftpDataResult();
            try
            {
                var sessionOptions = SessionData();
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);
                    _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Message: SFTP Session Open Success");
                    if (!session.FileExists(directoryName))
                    {
                        session.CreateDirectory(directoryName);
                    }
                    foreach (var filePath in FileList)
                    {
                        if (session.FileExists(filePath))
                        {
                            var FileArray = filePath.Split('/');
                            if (FileArray.Length > 0)
                            {
                                var fileAlredyExist = directoryName + FileArray[FileArray.Length - 1];
                                if (session.FileExists(fileAlredyExist))
                                {
                                    session.RemoveFiles(fileAlredyExist);
                                }
                            }
                            session.MoveFile(filePath, directoryName);
                            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Message: SFTP File Move Success");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception: {exception} Message: SFTP File Move Error";
                _logger.Error(message);
            }
        }
        #endregion

        #region Error CSV file Generate 
        /// <summary>
        /// Generate Error Csv Data
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <param name="syncFolder"></param>
        /// <param name="sapDataSyncResultDto"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public string GeneratErrorCsvDataAsync(string decryptedString, string syncFolder, SapDataSyncResultDto sapDataSyncResultDto, string subject)
        {
            _methodName = "GeneratErrorCsvDataAsync";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} SyncFolder {syncFolder}");
            var newHeaderDetails = new List<string>();
            var sbNew = new StringBuilder();
            var fileName = string.Empty;
            try
            {
                switch (syncFolder)
                {

                    case ConsoleSettings.DirectSaudaFolder:
                        var saudaViewDtoList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SaudaViewDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SaudaViewDto>(); 
                        sbNew = new StringBuilder();
                        //SPF and Rasoi Sauda                        
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.DirectSaudaCsv);
                        subject = string.Concat(ConsoleSettings.SaudaSPFRasoiCreationSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);                       
                        newHeaderDetails = new List<string>{"SaudaNumber", "SO_TYPE", "SO_ORG", "CUST_PO_NO",
                                                                    "CUST_PO_DT","VALID_FROM_DT",
                                                                    "VALID_TO_DT", "SOLD_TO_PARTY", "SHIP_TO_PARTY",
                                                                    "Material_No", "Quantity", "DOC_DT",
                                                                    "CUST_GRP", "Price_List_Type", "PRIC_GRP", "Usage",
                                                                    "INCO1", "INCO2", "BILL_DT",
                                                                    "DEL_PRIO", "PLANT", "STO_LOC",
                                                                    "Max_No_Of_Per_delivery","TradeTicketNumber",  "Cond_Type1","Rate",
                                                                    "Cond_Type2", "Rate","Cust_PO_Type","Uom", "Cond_Type3","Rate","Cond_Type4","Rate"};
                        foreach (var sauda in saudaViewDtoList)
                        {
                            sbNew.AppendLine(string.Concat(sauda.SaudaNumber, ConsoleSettings.CsvDelimiter, "ZSFQ", ConsoleSettings.CsvDelimiter, sauda.VerticalName, ConsoleSettings.CsvDelimiter, sauda.CustomerPoNumber,
                            ConsoleSettings.CsvDelimiter, sauda.CustomerPoDate.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, sauda.ValidFrom.ToString(ConsoleSettings.SAPDateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.ValidTo.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, sauda.SoldToParty, ConsoleSettings.CsvDelimiter, sauda.ShipToParty,
                            ConsoleSettings.CsvDelimiter, sauda.Sku, ConsoleSettings.CsvDelimiter, sauda.BidQuantity, ConsoleSettings.CsvDelimiter, sauda.DocumentDate.ToString(ConsoleSettings.SAPDateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.CustomerGroup, ConsoleSettings.CsvDelimiter, sauda.PriceListType, ConsoleSettings.CsvDelimiter, sauda.PriceGroup, ConsoleSettings.CsvDelimiter, sauda.Usage,
                            ConsoleSettings.CsvDelimiter, sauda.INCO1, ConsoleSettings.CsvDelimiter, sauda.INCO2, ConsoleSettings.CsvDelimiter, sauda.BillDate.ToString(ConsoleSettings.SAPDateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.DeliveryPriority, ConsoleSettings.CsvDelimiter, sauda.UserDepotMapping, ConsoleSettings.CsvDelimiter, sauda.PickingPoint,
                            ConsoleSettings.CsvDelimiter, sauda.MaximumNumberDeliveries, ConsoleSettings.CsvDelimiter, sauda.TradeTicketNumber, ConsoleSettings.CsvDelimiter, sauda.ConditionType1, ConsoleSettings.CsvDelimiter, sauda.BidAmount,
                            ConsoleSettings.CsvDelimiter, sauda.ConditionType2, ConsoleSettings.CsvDelimiter, sauda.Rate2, ConsoleSettings.CsvDelimiter, sauda.CustomerPOType, ConsoleSettings.CsvDelimiter, sauda.Uom,
                            ConsoleSettings.CsvDelimiter, sauda.ConditionType3, ConsoleSettings.CsvDelimiter, sauda.Rate3, ConsoleSettings.CsvDelimiter, sauda.ConditionType4, ConsoleSettings.CsvDelimiter, sauda.Rate4,
                            ConsoleSettings.CsvDelimiter));
                        }
                        break;
                    case ConsoleSettings.DirectTradeTiketFolder:
                        var tradeTicketList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPTradeTicketViewDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPTradeTicketViewDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.DirectTradeTicketCsv);
                        sbNew = new StringBuilder();
                        var sbModified = new StringBuilder();
                        //foreach (var tradeTicket in tradeTicketList)
                        //{

                        //    sbNew.AppendLine(string.Concat(tradeTicket.TradeTicketNumber, ConsoleSettings.CsvDelimiter, tradeTicket.ContractType, ConsoleSettings.CsvDelimiter, tradeTicket.BookingType, ConsoleSettings.CsvDelimiter, tradeTicket.MaterialType, ConsoleSettings.CsvDelimiter,
                        //    tradeTicket.MATERIAL_TYPE1, ConsoleSettings.CsvDelimiter, tradeTicket.MATERIAL_TYPE2, ConsoleSettings.CsvDelimiter, tradeTicket.MATERIAL_TYPE3, ConsoleSettings.CsvDelimiter, tradeTicket.MATERIAL_TYPE4, ConsoleSettings.CsvDelimiter, tradeTicket.MATERIAL_TYPE5, ConsoleSettings.CsvDelimiter,
                        //    tradeTicket.PRICE1, ConsoleSettings.CsvDelimiter, tradeTicket.PRICE2, ConsoleSettings.CsvDelimiter, tradeTicket.PRICE3, ConsoleSettings.CsvDelimiter, tradeTicket.PRICE4, ConsoleSettings.CsvDelimiter, tradeTicket.PRICE5, ConsoleSettings.CsvDelimiter,
                        //    tradeTicket.PRCOST1, ConsoleSettings.CsvDelimiter, tradeTicket.PRCOST2, ConsoleSettings.CsvDelimiter, tradeTicket.PRCOST3, ConsoleSettings.CsvDelimiter, tradeTicket.PRCOST4, ConsoleSettings.CsvDelimiter, tradeTicket.PRCOST5, ConsoleSettings.CsvDelimiter,
                        //    tradeTicket.PROPORTION1, ConsoleSettings.CsvDelimiter, tradeTicket.PROPORTION2, ConsoleSettings.CsvDelimiter, tradeTicket.PROPORTION3, ConsoleSettings.CsvDelimiter, tradeTicket.PROPORTION4, ConsoleSettings.CsvDelimiter, tradeTicket.PROPORTION5,
                        //    ConsoleSettings.CsvDelimiter, tradeTicket.ContractQuantity, ConsoleSettings.CsvDelimiter, tradeTicket.UnitOfMeasure, ConsoleSettings.CsvDelimiter, tradeTicket.PlantOrVendor, ConsoleSettings.CsvDelimiter, tradeTicket.ContractDate.ToString(ConsoleSettings.SAPDateFormat),
                        //    ConsoleSettings.CsvDelimiter, tradeTicket.ValidFrom == null ? "0" : tradeTicket.ValidFrom.Value.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, tradeTicket.ValidTo == null ? "0" : tradeTicket.ValidTo.Value.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, tradeTicket.OtherElement));

                        //}                       
                        newHeaderDetails = new List<string>{"Trade_Ticket", "CONT_TYPE", "B_TYPE", "MAT_TYPE_H", "OIL_TYPE1", "OIL_TYPE2", "OIL_TYPE", "OIL_TYPE4", "OIL_TYPE5",
                                                                "PRICE1", "PRICE2", "PRICE3", "PRICE4", "PRICE5", "PRCOST1", "PRCOST2", "PRCOST3", "PRCOST4", "PRCOST5",
                                                                "PROPORTION1", "PROPORTION2", "PROPORTION3", "PROPORTION4", "PROPORTION5", "CONT_QTY", "UNIT",
                                                                "PLANT_VEN", "CREATE_DATE", "VALID_FROM", "VALID_TO", "OTH_ELEMENT" };

                        break;
                    case ConsoleSettings.CustomerFolder:
                        var customerList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPCustomerDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPCustomerDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.CustomerCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "Code",
                         "UserCode",
                         "Name",
                         "Name",
                         "City",
                         "Region",
                         "Street",
                         "ADRNR",
                         "GSTN",
                         "District",
                         "DeliveringPlant",
                         "MobileNumber",
                         "Email",
                         "State",
                         "CentralDeletionFlag",
                         "VerticalCode",
                         "VTWEG","SPART","KDGRP","KONDA","PLTYP",
                         "FSSAINumber"};
                        foreach (var customer in customerList)
                        {
                            var nameArr = customer.Name.Split('-');
                            sbNew.AppendLine(string.Concat(customer.Code, ConsoleSettings.CsvDelimiter, customer.UserCode, ConsoleSettings.CsvDelimiter, nameArr[0], ConsoleSettings.CsvDelimiter, nameArr[0], ConsoleSettings.CsvDelimiter,
                            customer.City, ConsoleSettings.CsvDelimiter, customer.Region, ConsoleSettings.CsvDelimiter, customer.Street, ConsoleSettings.CsvDelimiter, customer.ADRNR, ConsoleSettings.CsvDelimiter,
                            customer.GSTN, ConsoleSettings.CsvDelimiter, customer.District, ConsoleSettings.CsvDelimiter, customer.DeliveringPlant, ConsoleSettings.CsvDelimiter,
                            customer.MobileNumber, ConsoleSettings.CsvDelimiter, customer.Email, ConsoleSettings.CsvDelimiter,
                            customer.State, ConsoleSettings.CsvDelimiter, customer.CentralDeletionFlag, ConsoleSettings.CsvDelimiter, customer.VerticalCode, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter,
                            ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, customer.FSSAINumber, ConsoleSettings.CsvDelimiter));

                        }                        
                        break;
                    case ConsoleSettings.BrokerFolder:
                        var brokerList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPCustomerDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPCustomerDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.BrokerCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "Code",
                         "UserCode",
                         "Name",
                         "Name",
                         "City",
                         "Region",
                         "Street",
                         "ADRNR",
                         "GSTN",
                         "District",
                         "DeliveringPlant",
                         "MobileNumber",
                         "Email",
                         "State",
                         "CentralDeletionFlag",
                         "VerticalCode",
                         "VTWEG","SPART","KDGRP","KONDA","PLTYP",
                         "FSSAINumber"};
                        foreach (var broker in brokerList)
                        {

                            var nameArr = broker.Name.Split('-');
                            sbNew.AppendLine(string.Concat(broker.Code, ConsoleSettings.CsvDelimiter, broker.UserCode, ConsoleSettings.CsvDelimiter, nameArr[0], ConsoleSettings.CsvDelimiter, nameArr[0], ConsoleSettings.CsvDelimiter,
                            broker.City, ConsoleSettings.CsvDelimiter, broker.Region, ConsoleSettings.CsvDelimiter, broker.Street, ConsoleSettings.CsvDelimiter, broker.ADRNR, ConsoleSettings.CsvDelimiter,
                            broker.GSTN, ConsoleSettings.CsvDelimiter, broker.District, ConsoleSettings.CsvDelimiter, broker.DeliveringPlant, ConsoleSettings.CsvDelimiter,
                            broker.MobileNumber, ConsoleSettings.CsvDelimiter, broker.Email, ConsoleSettings.CsvDelimiter,
                            broker.State, ConsoleSettings.CsvDelimiter, broker.CentralDeletionFlag, ConsoleSettings.CsvDelimiter, broker.VerticalCode, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter,
                            ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, broker.FSSAINumber, ConsoleSettings.CsvDelimiter));

                        }                       
                        break;
                    case ConsoleSettings.ShipToParty:
                        var shipToPartyList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPCustomerDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPCustomerDto>();
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.BrokerCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "Code",
                         "UserCode",
                         "Name",
                         "Name",
                         "City",
                         "Region",
                         "Street",
                         "ADRNR",
                         "GSTN",
                         "District",
                         "DeliveringPlant",
                         "MobileNumber",
                         "Email",
                         "State",
                         "CentralDeletionFlag",
                         "VerticalCode",
                         "VTWEG","SPART","KDGRP","KONDA","PLTYP",
                         "FSSAINumber"};
                        foreach (var shipToParty in shipToPartyList)
                        {

                            var nameArr = shipToParty.Name.Split('-');
                            sbNew.AppendLine(string.Concat(shipToParty.Code, ConsoleSettings.CsvDelimiter, shipToParty.UserCode, ConsoleSettings.CsvDelimiter, nameArr[0], ConsoleSettings.CsvDelimiter, nameArr[0], ConsoleSettings.CsvDelimiter,
                            shipToParty.City, ConsoleSettings.CsvDelimiter, shipToParty.Region, ConsoleSettings.CsvDelimiter, shipToParty.Street, ConsoleSettings.CsvDelimiter, shipToParty.ADRNR, ConsoleSettings.CsvDelimiter,
                            shipToParty.GSTN, ConsoleSettings.CsvDelimiter, shipToParty.District, ConsoleSettings.CsvDelimiter, shipToParty.DeliveringPlant, ConsoleSettings.CsvDelimiter,
                            shipToParty.MobileNumber, ConsoleSettings.CsvDelimiter, shipToParty.Email, ConsoleSettings.CsvDelimiter,
                            shipToParty.State, ConsoleSettings.CsvDelimiter, shipToParty.CentralDeletionFlag, ConsoleSettings.CsvDelimiter, shipToParty.VerticalCode, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter,
                            ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, shipToParty.FSSAINumber, ConsoleSettings.CsvDelimiter));

                        }
                        break;
                    case ConsoleSettings.DepotMasterFolder:
                        var deportList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPDepotDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPDepotDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.DeportCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "PlantCode",
                         "Name",
                         "ADRNR",
                         "Street1",
                         "Region",
                         "City",
                         "CentralArchiving",
                         "StateName",
                         "Street2",
                         "TelephoneNumber",
                         "Email",
                         "TaxNumber",
                         "IsPlant"};
                        foreach (var deport in deportList)
                        {
                            sbNew.AppendLine(string.Concat(deport.PlantCode, ConsoleSettings.CsvDelimiter, deport.Name, ConsoleSettings.CsvDelimiter, deport.ADRNR, ConsoleSettings.CsvDelimiter, deport.Street1, ConsoleSettings.CsvDelimiter,
                            deport.Region, ConsoleSettings.CsvDelimiter, deport.City, ConsoleSettings.CsvDelimiter, deport.CentralArchiving, ConsoleSettings.CsvDelimiter, deport.StateName, ConsoleSettings.CsvDelimiter,
                            deport.Street2, ConsoleSettings.CsvDelimiter, deport.TelephoneNumber, ConsoleSettings.CsvDelimiter, deport.Email, ConsoleSettings.CsvDelimiter,
                            deport.TaxNumber, ConsoleSettings.CsvDelimiter, deport.IsPlant == false ? "x" : string.Empty, ConsoleSettings.CsvDelimiter));
                        }                       
                        break;
                    case ConsoleSettings.TradeTicketFolder:
                        var tradeTicketNumberList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<TradeTicketNumberDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<TradeTicketNumberDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.TradeTicketNumberCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "Id",
                         "TradeTicketNumber",
                         "ErrorMessage"};
                        foreach (var tradeTicketNumber in tradeTicketNumberList)
                        {
                            sbNew.AppendLine(string.Concat(tradeTicketNumber.Id, ConsoleSettings.CsvDelimiter, tradeTicketNumber.TradeTicketNumber, ConsoleSettings.CsvDelimiter, tradeTicketNumber.ErrorMessage, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.SaudaFolder + "/" + ConsoleSettings.SaudaHBCFolder:
                        var hbcSaudaNumberList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SaudaNumberDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SaudaNumberDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaNumberCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                        "Id",
                        "SaudaNumber",
                        "SAUDA_STATUS",
                        "STATUS",
                        "ErrorMessage"};
                        foreach (var saudaNumber in hbcSaudaNumberList)
                        {
                            sbNew.AppendLine(string.Concat(saudaNumber.AppId, ConsoleSettings.CsvDelimiter, saudaNumber.SaudaNumber, ConsoleSettings.CsvDelimiter, "", ConsoleSettings.CsvDelimiter, "", ConsoleSettings.CsvDelimiter, saudaNumber.ErrorMessage, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.SaudaFolder + "/" + ConsoleSettings.SaudaSPFFolder:
                        var spfSaudaNumberList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SaudaNumberDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SaudaNumberDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaNumberCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "Id",
                         "SaudaNumber",
                         "SAUDA_STATUS",
                         "STATUS",
                         "ErrorMessage"};
                        foreach (var saudaNumber in spfSaudaNumberList)
                        {
                            sbNew.AppendLine(string.Concat(saudaNumber.AppId, ConsoleSettings.CsvDelimiter, saudaNumber.SaudaNumber, ConsoleSettings.CsvDelimiter, "", ConsoleSettings.CsvDelimiter,"", ConsoleSettings.CsvDelimiter, saudaNumber.ErrorMessage, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.SaudaReleaseFolder:
                        var saudaReleaseFolderList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SaudaReleaseDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SaudaReleaseDto>();
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaNumberCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{                        
                        "SaudaNumber",
                        "STATUS"};
                        foreach (var saudaNumber in saudaReleaseFolderList)
                        {
                            sbNew.AppendLine(string.Concat(saudaNumber.SaudaNumber, ConsoleSettings.CsvDelimiter, saudaNumber.SaudaStatus, ConsoleSettings.CsvDelimiter));
                        }
                        break;
                    case ConsoleSettings.SaudaLooseOilFolder:
                        var saudaLooseOilFolderList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SaudaNumberDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SaudaNumberDto>();
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaNumberCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                        "SaudaOrderId",
                        "ErrorMessage"};
                        foreach (var saudaNumber in saudaLooseOilFolderList)
                        {
                            sbNew.AppendLine(string.Concat(saudaNumber.AppId, ConsoleSettings.CsvDelimiter, saudaNumber.ErrorMessage, ConsoleSettings.CsvDelimiter));
                        }
                        break;
                    case ConsoleSettings.InvoiceFolder:
                        var invoiceList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPInvoiceDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPInvoiceDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.InvoiceCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "BillingDocument",
                         "UserCode",
                         "Plant",
                         "FromWarehouseId",
                         "Mode",
                         "NetValue",
                         "BillDiscount",
                         "BillDiscountType",
                         "BillDate",
                         "InvoiceDueDate",
                         "MaterialNumber",
                         "QuantityInCase",
                         "ActualBilledQuantity",
                         "Discount",
                         "DiscountType",
                         "Status",
                         "SKUInvoiceTax",
                         "SalesDocumentType",
                         "UnitPrice",
                         "VechicleId",
                         "DriverName",
                         "DriverNumber",
                         "GstAmount",
                         "VerticalCode",
                         "UOM",
                         "SaudaNumber",
                         "BatchNo",
                         "DoNumber"};
                        foreach (var invoice in invoiceList)
                        {
                            sbNew.AppendLine(string.Concat(invoice.BillingDocument, ConsoleSettings.CsvDelimiter, invoice.UserCode, ConsoleSettings.CsvDelimiter, invoice.Plant, ConsoleSettings.CsvDelimiter, invoice.FromWarehouseId, ConsoleSettings.CsvDelimiter,
                            invoice.Mode, ConsoleSettings.CsvDelimiter, invoice.NetValue, ConsoleSettings.CsvDelimiter, invoice.BillDiscount, ConsoleSettings.CsvDelimiter, invoice.BillDiscountType, ConsoleSettings.CsvDelimiter,
                            invoice.BillDate.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, invoice.InvoiceDueDate.Value.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, invoice.MaterialNumber, ConsoleSettings.CsvDelimiter, invoice.QuantityInCase, ConsoleSettings.CsvDelimiter,
                            invoice.ActualBilledQuantity, ConsoleSettings.CsvDelimiter, invoice.Discount, ConsoleSettings.CsvDelimiter, invoice.DiscountType, ConsoleSettings.CsvDelimiter, invoice.Status, ConsoleSettings.CsvDelimiter,
                            invoice.SKUInvoiceTax, ConsoleSettings.CsvDelimiter, invoice.SalesDocumentType, ConsoleSettings.CsvDelimiter, invoice.UnitPrice, ConsoleSettings.CsvDelimiter, invoice.VechicleId, ConsoleSettings.CsvDelimiter,
                            invoice.DriverName, ConsoleSettings.CsvDelimiter, invoice.DriverNumber, ConsoleSettings.CsvDelimiter, invoice.GstAmount, ConsoleSettings.CsvDelimiter, invoice.VerticalCode, ConsoleSettings.CsvDelimiter,
                            invoice.UOM, ConsoleSettings.CsvDelimiter, invoice.SaudaNumber, ConsoleSettings.CsvDelimiter,invoice.BatchNo, ConsoleSettings.CsvDelimiter,invoice.DoNumber, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.InvoiceCancelAndReturnFolder:
                        var invoiceCancelAndReturnList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPInvoiceDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPInvoiceDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.InvoiceCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "BillingDocument",
                         "UserCode",
                         "Plant",
                         "FromWarehouseId",
                         "Mode",
                         "NetValue",
                         "BillDiscount",
                         "BillDiscountType",
                         "BillDate",
                         "InvoiceDueDate",
                         "MaterialNumber",
                         "QuantityInCase",
                         "ActualBilledQuantity",
                         "Discount",
                         "DiscountType",
                         "Status",
                         "SKUInvoiceTax",
                         "SalesDocumentType",
                         "UnitPrice",
                         "VechicleId",
                         "DriverName",
                         "DriverNumber",
                         "GstAmount",
                         "VerticalCode",
                         "UOM",
                         "InvoiceCancelFlag",
                        "XBLNR",
                        "CONTRACT",
                        "BatchNo",
                         "DoNumber"};
                        foreach (var invoice in invoiceCancelAndReturnList)
                        {
                            sbNew.AppendLine(string.Concat(invoice.BillingDocument, ConsoleSettings.CsvDelimiter, invoice.UserCode, ConsoleSettings.CsvDelimiter, invoice.Plant, ConsoleSettings.CsvDelimiter, invoice.FromWarehouseId, ConsoleSettings.CsvDelimiter,
                            invoice.Mode, ConsoleSettings.CsvDelimiter, invoice.NetValue, ConsoleSettings.CsvDelimiter, invoice.BillDiscount, ConsoleSettings.CsvDelimiter, invoice.BillDiscountType, ConsoleSettings.CsvDelimiter,
                            invoice.BillDate.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, invoice.InvoiceDueDate.Value.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter, invoice.MaterialNumber, ConsoleSettings.CsvDelimiter, invoice.QuantityInCase, ConsoleSettings.CsvDelimiter,
                            invoice.ActualBilledQuantity, ConsoleSettings.CsvDelimiter, invoice.Discount, ConsoleSettings.CsvDelimiter, invoice.DiscountType, ConsoleSettings.CsvDelimiter, invoice.Status, ConsoleSettings.CsvDelimiter,
                            invoice.SKUInvoiceTax, ConsoleSettings.CsvDelimiter, invoice.SalesDocumentType, ConsoleSettings.CsvDelimiter, invoice.UnitPrice, ConsoleSettings.CsvDelimiter, invoice.VechicleId, ConsoleSettings.CsvDelimiter,
                            invoice.DriverName, ConsoleSettings.CsvDelimiter, invoice.DriverNumber, ConsoleSettings.CsvDelimiter, invoice.GstAmount, ConsoleSettings.CsvDelimiter, invoice.VerticalCode, ConsoleSettings.CsvDelimiter,
                            invoice.UOM, ConsoleSettings.CsvDelimiter, invoice.InvoiceCancelFlag ? "x" : string.Empty, ConsoleSettings.CsvDelimiter, string.Empty,ConsoleSettings.CsvDelimiter, invoice.SaudaNumber, ConsoleSettings.CsvDelimiter,
                            invoice.BatchNo, ConsoleSettings.CsvDelimiter, invoice.DoNumber,ConsoleSettings.CsvDelimiter));
                        }                      
                        break;
                    case ConsoleSettings.LiftingRequestFolder:
                        var liftingRequestList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<LiftingRequestDeliveryOrderNumberDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<LiftingRequestDeliveryOrderNumberDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.LiftingRequestCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "Id",
                         "DeliveryOrderNumber",
                         "SaudaNumber",
                         "ContractQuantity",
                         "PendingQuantity",
                         "LiftingQuantity",
                         "",
                         "",
                         "ErrorMessage"};
                        foreach (var liftingRequest in liftingRequestList)
                        {
                            sbNew.AppendLine(string.Concat(liftingRequest.Id, ConsoleSettings.CsvDelimiter, liftingRequest.DeliveryOrderNumber, ConsoleSettings.CsvDelimiter, liftingRequest.SaudaNumber, ConsoleSettings.CsvDelimiter,
                                liftingRequest.ContractQuantity, ConsoleSettings.CsvDelimiter, liftingRequest.PendingQuantity, ConsoleSettings.CsvDelimiter, liftingRequest.LiftingQuantity, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter,
                                liftingRequest.ErrorMessage, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.SaudaLimitFolder:
                        var SaudaLimitList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPSaudaLimitDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPSaudaLimitDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaLimitAddCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "CustomerCode",
                         "CustomerName",
                         //"PartnerFunction",
                         "VerticalCode",
                         "PendingContract",
                         "PendingDO",
                         "PendingOBD"};
                        foreach (var saudaLimit in SaudaLimitList)
                        {
                            sbNew.AppendLine(string.Concat(saudaLimit.CustomerCode, ConsoleSettings.CsvDelimiter, saudaLimit.CustomerName, ConsoleSettings.CsvDelimiter, 
                            saudaLimit.VerticalCode, ConsoleSettings.CsvDelimiter, saudaLimit.PendCont, ConsoleSettings.CsvDelimiter, saudaLimit.PendDO, ConsoleSettings.CsvDelimiter,
                            saudaLimit.PendOBD, ConsoleSettings.CsvDelimiter));
                        }                      
                        break;
                    case ConsoleSettings.SKUMasterFolder:
                        var sapSkuDtoList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPSkuDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPSkuDto>(); 
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "SkuCode",
                         "MaterialDescription",
                         "ConvertionType",
                         "ConvertionFactor",
                         "VerticalCode",
                         "SalesDivision",
                         "MaterialGroup1",
                         "OilTypeCode",
                         "VerticalGroupCode",
                         "PackTypeCode",
                         "MATKL",
                         "MaterialType",
                         "PackGroups"};
                        foreach (var saudaLimit in sapSkuDtoList)
                        {
                            sbNew.AppendLine(string.Concat(saudaLimit.SkuCode, ConsoleSettings.CsvDelimiter, saudaLimit.MaterialDescription, ConsoleSettings.CsvDelimiter, saudaLimit.ConvertionType, ConsoleSettings.CsvDelimiter, saudaLimit.ConvertionFactor, ConsoleSettings.CsvDelimiter,
                            saudaLimit.VerticalCode, ConsoleSettings.CsvDelimiter, saudaLimit.SalesDivision, ConsoleSettings.CsvDelimiter, saudaLimit.MaterialGroup1, ConsoleSettings.CsvDelimiter,
                            saudaLimit.OilTypeCode, ConsoleSettings.CsvDelimiter, saudaLimit.VerticalGroupCode, ConsoleSettings.CsvDelimiter, saudaLimit.PackTypeCode, ConsoleSettings.CsvDelimiter, ConsoleSettings.CsvDelimiter,
                            saudaLimit.MaterialType, ConsoleSettings.CsvDelimiter, saudaLimit.PackGroups, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.CreditMasterFolder:
                        var sapCreditMasterDtoList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPCreditMasterDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPCreditMasterDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.CreditMasterCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "CustomerCode",
                         "CreditArea",
                         "CreditAccountNumber",
                         "RiskCat",
                         "Curr",
                         "CreditLimit",
                         "CreditExposure",
                         "SalesValue",
                        "TotalReceivable",
                        "SaudaDepC",
                        "SecDepH",
                        "BankGuarM",
                        "AdvanceA",
                        "DueToday",
                       "TomorrowsDue",
                        "Overdue",
                        "NotDue",
                        "NextIntRev",
                        "Blocked",
                        "TotalReceivable",
                        "IndividLimit",
                        "AvailableCreditLimit"};
                        foreach (var saudaLimit in sapCreditMasterDtoList)
                        {
                            sbNew.AppendLine(string.Concat(saudaLimit.CustomerCode, ConsoleSettings.CsvDelimiter, saudaLimit.CCreditArea, ConsoleSettings.CsvDelimiter, saudaLimit.CreditAccountNumber, ConsoleSettings.CsvDelimiter,
                            saudaLimit.RiskCat, ConsoleSettings.CsvDelimiter, saudaLimit.Curr, ConsoleSettings.CsvDelimiter, saudaLimit.CreditLimit, ConsoleSettings.CsvDelimiter,
                            saudaLimit.CreditExposure, ConsoleSettings.CsvDelimiter, saudaLimit.SalesValue, ConsoleSettings.CsvDelimiter, saudaLimit.TotalReceivable, ConsoleSettings.CsvDelimiter,
                            saudaLimit.SaudaDepC, ConsoleSettings.CsvDelimiter, saudaLimit.SecDepH, ConsoleSettings.CsvDelimiter, saudaLimit.BankGuarM, ConsoleSettings.CsvDelimiter,
                            saudaLimit.AdvanceA, ConsoleSettings.CsvDelimiter, saudaLimit.DueToday, ConsoleSettings.CsvDelimiter, saudaLimit.TomorrowsDue, ConsoleSettings.CsvDelimiter,
                            saudaLimit.Overdue, ConsoleSettings.CsvDelimiter, saudaLimit.NotDue, ConsoleSettings.CsvDelimiter, saudaLimit.NextIntRev, ConsoleSettings.CsvDelimiter,
                            saudaLimit.Blocked, ConsoleSettings.CsvDelimiter, saudaLimit.TotalLimit, ConsoleSettings.CsvDelimiter, saudaLimit.IndividLimit, ConsoleSettings.CsvDelimiter,
                            saudaLimit.AvailableCreditLimit, ConsoleSettings.CsvDelimiter));
                        }                       
                        break;
                    case ConsoleSettings.CustomerLedgerFolder:
                        var customerLedgerList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPCustomerLedgerDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPCustomerLedgerDto>();  
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.CustomerLedgerCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "UserCode",
                         "PdfFileName"};
                        foreach (var saudaLimit in customerLedgerList)
                        {
                            sbNew.AppendLine(string.Concat(saudaLimit.UserCode, ConsoleSettings.CsvDelimiter, saudaLimit.PdfFileName, ConsoleSettings.CsvDelimiter));
                        }                      
                        break;
                    case ConsoleSettings.DODeleteFolder:
                        var doDeleteList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPDoDeleteDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPDoDeleteDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.DODeleteCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "DONumber",
                         "Status"};
                        foreach (var saudaLimit in doDeleteList)
                        {
                            sbNew.AppendLine(string.Concat(saudaLimit.DONumber, ConsoleSettings.CsvDelimiter, saudaLimit.Status, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.DOUpdateFolder:
                        var doUpdateList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPDoUpdateDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPDoUpdateDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.DOUpdateCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "DONumber",
                         "SoldToParty",
                         "ShipToParty",
                         "Payer",
                         "BillToParty",
                         "Vertical",
                         "OrderQuantity",
                         "Uom",
                         "MaterialNumber",
                         "SaudaNumber",
                        "EnquiryRemarks",
                        "EnquiryNumber"};
                        foreach (var doUpdate in doUpdateList)
                        {
                            sbNew.AppendLine(string.Concat(doUpdate.DONumber, ConsoleSettings.CsvDelimiter, doUpdate.SoldToParty, ConsoleSettings.CsvDelimiter,
                                doUpdate.ShipToParty, ConsoleSettings.CsvDelimiter, doUpdate.Payer, ConsoleSettings.CsvDelimiter,
                                doUpdate.BillToParty, ConsoleSettings.CsvDelimiter, doUpdate.Vertical, ConsoleSettings.CsvDelimiter,
                                doUpdate.OrderQuantity, ConsoleSettings.CsvDelimiter, doUpdate.Uom, ConsoleSettings.CsvDelimiter,
                                doUpdate.MaterialNumber, ConsoleSettings.CsvDelimiter, doUpdate.SaudaNumber, ConsoleSettings.CsvDelimiter, 
                                doUpdate.Enquiry, ConsoleSettings.CsvDelimiter, doUpdate.Reason, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;
                    case ConsoleSettings.SaudaAmendmentFolder:
                        var saudaAmendmentList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPSaudaAmendmentDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPSaudaAmendmentDto>();                     
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaAmeCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "SaudaNumber",
                         "SaudaOrderId",
                         "SkuCode",
                         "Quantity",
                         "DepotCode",
                         "INCO1",
                         "INCO2",
                         "ToDate",
                         "SoldToParty",
                         "ShipToParty",
                         "Payer",
                         "BillToParty",
                         "Remarks",
                         "Uom",
                         "Vertical" };
                        foreach (var saudaAmendment in saudaAmendmentList)
                        {
                            sbNew.AppendLine(string.Concat(saudaAmendment.SaudaNumber, ConsoleSettings.CsvDelimiter, saudaAmendment.SaudaOrderId, ConsoleSettings.CsvDelimiter,
                                saudaAmendment.SkuCode, ConsoleSettings.CsvDelimiter, saudaAmendment.Quantity, ConsoleSettings.CsvDelimiter,
                                saudaAmendment.DepotCode, ConsoleSettings.CsvDelimiter, saudaAmendment.INCO1, ConsoleSettings.CsvDelimiter,
                                saudaAmendment.INCO2, ConsoleSettings.CsvDelimiter, saudaAmendment.ToDate.ToString(ConsoleSettings.SAPDateFormat), ConsoleSettings.CsvDelimiter,
                                saudaAmendment.SoldToParty, ConsoleSettings.CsvDelimiter, saudaAmendment.ShipToParty, ConsoleSettings.CsvDelimiter,
                                saudaAmendment.Payer, ConsoleSettings.CsvDelimiter, saudaAmendment.BillToParty, ConsoleSettings.CsvDelimiter,
                                string.Empty, ConsoleSettings.CsvDelimiter, saudaAmendment.Uom, ConsoleSettings.CsvDelimiter,
                                saudaAmendment.Vertical, ConsoleSettings.CsvDelimiter));
                        }                        
                        break;

                    case ConsoleSettings.InvoicePaymentStatus:
                        var invoiceStatusList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<SAPInvoiceStatusDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<SAPInvoiceStatusDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.InvoiceStatusCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "VBELN",
                         "PAYMENT"
                         };

                        foreach (var invoice in invoiceStatusList)
                        {
                            sbNew.AppendLine(string.Concat(invoice.InvoiceNumber, ConsoleSettings.CsvDelimiter, invoice.PaymentStatus));
                        }                      
                        break;
                    case ConsoleSettings.LiftingInquiry:
                        var liftingRequestDetailsList = !string.IsNullOrEmpty(sapDataSyncResultDto.ErrorDetailsResponse.ToString()) ? (List<LiftingRequestInquiryNumberDto>)sapDataSyncResultDto.ErrorDetailsResponse : new List<LiftingRequestInquiryNumberDto>(); 
                        fileName = ConsoleSettings.FilePathCreation(ConsoleSettings.InquiryrCsv);
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string>{
                         "APP_TT_ID",
                         "INQUIRY_NUMBER",
                         "STATUS",
                         "MESSAGE"
                         };

                        foreach (var liftingData in liftingRequestDetailsList)
                        {
                            sbNew.AppendLine(string.Concat(liftingData.LiftingRequestId, ConsoleSettings.CsvDelimiter,
                                liftingData.EnquiryNumber, ConsoleSettings.CsvDelimiter, liftingData.Status, ConsoleSettings.CsvDelimiter,
                                liftingData.Message));
                        }                        
                        break;
                    default:
                        break;
                }

                var directoryPathNew = ConsoleSettings.OutboundDirectoryPath(syncFolder);
                var sb = new StringBuilder();
                var headerDetails = String.Empty;
                foreach (var item in newHeaderDetails)
                {
                    headerDetails = string.Concat(headerDetails, item, ConsoleSettings.CsvDelimiter);
                }
                //Title of the table
                sb.AppendLine(headerDetails);
                sb.AppendLine(sbNew.ToString());
                if(newHeaderDetails.Count != 0)
                {
                    fileName = ConsoleSettings.SystemPath(fileName);
                    var filePath = new List<string>();
                    File.WriteAllText(fileName, sb.ToString().TrimEnd());
                }                
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception:{exception}";
                _logger.Error(message);
                SendNotification(subject, message, syncFolder, sapDataSyncResultDto, null, true);
            }

            return fileName;
        }
        #endregion

        #region Notification Send

        public void SendNotification(string subject, string syncMessage, string syncType, SapDataSyncResultDto result = null, List<string> filePath = null, bool isFailed = false)
        {

            AmazonNotificationService amazonNotificationService = new AmazonNotificationService(); 
            if (isFailed)
            {
                subject = string.Concat(ConsoleSettings.SyncFailed, " - ", subject);
            }
            else
            {
                subject = string.Concat(ConsoleSettings.SyncSuccess, " - ", subject);
            }
            amazonNotificationService.SendNotification(subject, syncMessage, syncType, result, filePath);
        }

        #endregion

        #region Responce for App Data Save
        public void SyncProcessForSucessAndFailed(ResultDto response, string syncFolder, SAPDataResponseDto inputDto, string subject = "")
        {
            _methodName = "SyncProcessForSucessAndFailed";
            _logger.Info($"{ServiceName} Controller-Method {_methodName} - {syncFolder} SAP Service");
            try
            {  
                var errorFlag = false;                
                var syncMessage = string.Empty;
                var sapDataSyncResultDto = new SapDataSyncResultDto();
                var directoryPath = ConsoleSettings.InboundDirectoryFilePath(syncFolder);
                var decryptedString = string.Empty;
                if (response.IsSuccess)
                {                                        
                   // syncMessage = response.SuccessDto.Message;
                   // errorFlag = false;
                   // sapDataSyncResultDto = (SapDataSyncResultDto)response.SuccessDto.Response;
                   //// var _sftpConnectorService = new SftpConnectorService();
                   //// _sftpConnectorService.MoveSFTPFile(inputDto.SourceFileName, directoryPath);
                   // SendNotification(subject, syncMessage, syncFolder, sapDataSyncResultDto, null, errorFlag);                    
                   // _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Message Success:{syncMessage}");   
                }
                else if (response.ErrorDto.ErrorCode == "Internal Server Error")
                {
                    errorFlag = true;
                    sapDataSyncResultDto = (SapDataSyncResultDto)response.ErrorDto.Response;
                    SendNotification(subject, response.ErrorDto.Message, syncFolder, sapDataSyncResultDto, null, errorFlag);
                }
                else
                {                   
                    //directoryPath = ConsoleSettings.InboundDirectoryFilePath(syncFolder);                    
                    //DeleteLocalFile(inputDto.LocalFileName);
                    //MoveSFTPFile(inputDto.SourceFileName, directoryPath);
                    //inputDto.LocalFileName = new List<string>();                    
                    sapDataSyncResultDto = (SapDataSyncResultDto)response.ErrorDto.Response;                    
                    // var csvFileName = GeneratErrorCsvDataAsync(inputDto.Response.ToString(), syncFolder, sapDataSyncResultDto, subject);
                    //inputDto.LocalFileName.Add(csvFileName);
                    //var directoryPathNew = ConsoleSettings.InboundDirectoryFilePath(syncFolder, true);
                    //if (inputDto.ErrorPdf.Any())
                    //{
                    //    inputDto.LocalFileName.AddRange(inputDto.ErrorPdf);
                    //}
                    //CreateDirectorySFTPFile(inputDto.LocalFileName, directoryPathNew);
                    errorFlag = true;
                    SendNotification(subject, response.ErrorDto.Message, syncFolder, sapDataSyncResultDto, null, errorFlag);
                    _logger.Error($"SAP Service : {ServiceName} Controller-Method {_methodName} Error:{syncMessage}");
                }                
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }

        }
        #endregion

        #region App to SAP Data Send

        public void GetDataAsync(ResultDto response, string syncFolder, string subject, string csvFileName)
        {
            _methodName = "GetDataAsync";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} SAP Service");
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            sapDataSyncResultDto.SyncStartedDateTime = DateTime.Now;
            subject = string.Concat(subject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
            csvFileName = ConsoleSettings.FilePathCreation(csvFileName);
            try
            {   
                _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} Message:Login Success");
                if (response.IsSuccess)
                {
                    GenerateCsvDataAsync(response, syncFolder, sapDataSyncResultDto, subject, csvFileName);
                }
                else
                {
                    SendNotification(subject, response.ErrorDto.Message, syncFolder, sapDataSyncResultDto, null, true);
                }
            }
            catch (Exception exception)
            {
                var message = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception:{exception}";
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateTime.Now;
                SendNotification(subject, message, syncFolder, sapDataSyncResultDto, null, true);
            }
        }

        /// <summary>
        /// Generate CSV file for APP to SAP
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <param name="syncFolder"></param>
        /// <param name="sapDataSyncResultDto"></param>
        /// <param name="subject"></param>
        /// <param name="csvFileName"></param>

        public void GenerateCsvDataAsync(ResultDto response, string syncFolder, SapDataSyncResultDto sapDataSyncResultDto, string subject, string csvFileName)
        {
            _methodName = "GenerateCsvDataAsync";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName} SyncFolder {syncFolder}");
            var newHeaderDetails = new List<string>();
            var sbNew = new StringBuilder();
            try
            {
                
                switch (syncFolder)
                {
                    case ConsoleSettings.SaudaLimitFolder:
                        var saudaLimitDtoList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SAPSaudaLimitDto>)response.SuccessDto.Response : new List<SAPSaudaLimitDto>(); 
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaLimitDtoList.Count;
                        newHeaderDetails = new List<string>{"CustomerNumber", "CustomerName", "PartnerFunction",
                                                                "SalesOrganization","CustomerTotalQuantity","UOM"};
                        foreach (var inputDetails in saudaLimitDtoList)
                        {
                            sbNew.AppendLine(string.Concat(inputDetails.CustomerCode, ConsoleSettings.CsvDelimiter, inputDetails.CustomerName, ConsoleSettings.CsvDelimiter, inputDetails.PartnerFunction,
                            ConsoleSettings.CsvDelimiter, inputDetails.VerticalCode, ConsoleSettings.CsvDelimiter, inputDetails.CustomerTotalQuantity, ConsoleSettings.CsvDelimiter, inputDetails.UOM, ConsoleSettings.CsvDelimiter));
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaLimitDtoList.Count;
                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);

                        break;
                    case ConsoleSettings.LiftingRequestFolder:
                        var liftingRequestViewDtoList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SAPLiftingRequestViewDto>)response.SuccessDto.Response : new List<SAPLiftingRequestViewDto>(); 
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = liftingRequestViewDtoList.Count;

                        newHeaderDetails = new List<string>{"APP_TT_ID", "Customer_Code", "Contract_Type", "Sales_Organization",
                                                                    "Distrbution_Channel","Division","Oil_Type",
                                                                    "Material_Number", "Required_Quantity","UOM","ShipToPartyCode","LiftingRequestDate","LiftingRequestTime","ApproveDate","ApproveTime"};
                        foreach (var inputDetails in liftingRequestViewDtoList)
                        {

                            sbNew.AppendLine(string.Concat(inputDetails.LiftingRequestDetailsId, ConsoleSettings.CsvDelimiter, inputDetails.CustomerCode, ConsoleSettings.CsvDelimiter, inputDetails.ContractType, ConsoleSettings.CsvDelimiter, inputDetails.SalesOrganization,
                            ConsoleSettings.CsvDelimiter, inputDetails.DistrbutionChannel, ConsoleSettings.CsvDelimiter, inputDetails.Division, ConsoleSettings.CsvDelimiter, inputDetails.OilType,
                            ConsoleSettings.CsvDelimiter, inputDetails.MaterialNumber, ConsoleSettings.CsvDelimiter, Convert.ToInt32(inputDetails.RequiredQuantity),
                            ConsoleSettings.CsvDelimiter, inputDetails.UOM, ConsoleSettings.CsvDelimiter, inputDetails.ShipToPartyCode, ConsoleSettings.CsvDelimiter,
                            inputDetails.LiftingRequestDate.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, inputDetails.LiftingRequestDate.ToString(ConsoleSettings.TimeFormat, CultureInfo.CurrentCulture), ConsoleSettings.CsvDelimiter,
                            inputDetails.ApproveDate != null && inputDetails.ApproveDate != DateTime.MinValue ? inputDetails.ApproveDate.Value.ToString(ConsoleSettings.DateFormat) : string.Empty, ConsoleSettings.CsvDelimiter,
                            inputDetails.ApproveDate != null && inputDetails.ApproveDate != DateTime.MinValue ? inputDetails.ApproveDate.Value.ToString(ConsoleSettings.TimeFormat, CultureInfo.CurrentCulture) : string.Empty, ConsoleSettings.CsvDelimiter));
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = liftingRequestViewDtoList.Count;
                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);

                        break;
                    case ConsoleSettings.SaudaApproval:
                        var saudaStatusDto = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SaudaStatusDto>)response.SuccessDto.Response : new List<SaudaStatusDto>(); 
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaStatusDto.Count;
                        sbNew = new StringBuilder();
                        newHeaderDetails = new List<string> { "VBELN", "NRL", "RL" };
                        foreach (var sauda in saudaStatusDto)
                        {
                            sbNew.AppendLine(string.Concat(sauda.SaudaNumber, ConsoleSettings.CsvDelimiter, sauda.SaudaStatusId == (int)Status.Rejected ? "X" : string.Empty, ConsoleSettings.CsvDelimiter,
                            sauda.SaudaStatusId == (int)Status.Approved ? "X" : string.Empty, ConsoleSettings.CsvDelimiter));
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaStatusDto.Count;
                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);

                        break;
                    case ConsoleSettings.SaudaFolder:
                        syncFolder = ConsoleSettings.SaudaFolder + "/" + ConsoleSettings.SaudaHBCFolder;
                        var saudaViewDtoList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SaudaViewDto>)response.SuccessDto.Response : new List<SaudaViewDto>();  
                        csvFileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaHBCCreationCsv);
                        //HBC Sauda
                        var saudaHBCList = saudaViewDtoList.Where(_ => _.VerticalId == (int)Division.Hbc).ToList();
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaHBCList.Count;
                        var sbHBCNew = new StringBuilder();
                        var newHBCHeaderDetails = new List<string>{"APP_TT_ID", "SO_TYPE", "SO_ORG", "CUST_PO_NO",
                                                                    "CUST_PO_DT","VALID_FROM_DT",
                                                                    "VALID_TO_DT", "SOLD_TO_PARTY", "SHIP_TO_PARTY",
                                                                    "Material_No", "Quantity", "DOC_DT",
                                                                    "CUST_GRP", "Price_List_Type", "PRIC_GRP", "Usage",
                                                                    "INCO1", "INCO2", "BILL_DT",
                                                                    "DEL_PRIO", "PLANT", "STO_LOC",
                                                                    "Max_No_Of_Per_delivery", "Your_Reference", "Cond_Type1",
                                                                    "Rate","Cond_Type2", "Rate", "Cust_PO_Type","Cond_Type3","Rate"};
                        foreach (var sauda in saudaHBCList)
                        {

                            sbHBCNew.AppendLine(string.Concat(sauda.Id, ConsoleSettings.CsvDelimiter, "ZHQ", ConsoleSettings.CsvDelimiter, sauda.VerticalName, ConsoleSettings.CsvDelimiter, sauda.CustomerPoNumber,
                            ConsoleSettings.CsvDelimiter, sauda.CustomerPoDate.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, sauda.ValidFrom.ToString(ConsoleSettings.DateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.ValidTo.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, sauda.SoldToParty, ConsoleSettings.CsvDelimiter, sauda.ShipToParty,
                            ConsoleSettings.CsvDelimiter, sauda.Sku, ConsoleSettings.CsvDelimiter, sauda.BidQuantity, ConsoleSettings.CsvDelimiter, sauda.DocumentDate.ToString(ConsoleSettings.DateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.CustomerGroup, ConsoleSettings.CsvDelimiter, sauda.PriceListType, ConsoleSettings.CsvDelimiter, sauda.PriceGroup, ConsoleSettings.CsvDelimiter, sauda.Usage,
                            ConsoleSettings.CsvDelimiter, sauda.INCO1, ConsoleSettings.CsvDelimiter, sauda.INCO2, ConsoleSettings.CsvDelimiter, sauda.BillDate.ToString(ConsoleSettings.DateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.DeliveryPriority, ConsoleSettings.CsvDelimiter, sauda.UserDepotMapping, ConsoleSettings.CsvDelimiter, sauda.PickingPoint,
                            ConsoleSettings.CsvDelimiter, sauda.MaximumNumberDeliveries, ConsoleSettings.CsvDelimiter, sauda.TradeTicketNumber, ConsoleSettings.CsvDelimiter, sauda.ConditionType1,
                            ConsoleSettings.CsvDelimiter, sauda.BidAmount, ConsoleSettings.CsvDelimiter, sauda.ConditionType2, ConsoleSettings.CsvDelimiter, sauda.Rate2, ConsoleSettings.CsvDelimiter, sauda.CustomerPOType, ConsoleSettings.CsvDelimiter,
                            sauda.ConditionType3, ConsoleSettings.CsvDelimiter, sauda.Rate3, ConsoleSettings.CsvDelimiter));
                        }

                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaHBCList.Count;
                        GenerateCsvMoveAPPtoSAP(newHBCHeaderDetails, sbHBCNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);

                        //SPF and Rasoi Sauda
                        sapDataSyncResultDto = new SapDataSyncResultDto();
                        sapDataSyncResultDto.SyncStartedDateTime = DateTime.Now;
                        syncFolder = ConsoleSettings.SaudaFolder + "/" + ConsoleSettings.SaudaSPFFolder;
                        csvFileName = ConsoleSettings.FilePathCreation(ConsoleSettings.SaudaSPFCreationCsv);
                        subject = string.Concat(ConsoleSettings.SaudaSPFRasoiCreationSubject, " ", ConsoleSettings.AppToSapDataSyncEmailSubject);
                        var saudaSPFList = saudaViewDtoList.Where(_ => _.VerticalId != (int)Division.Hbc).ToList();
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaSPFList.Count;
                        var sbSPFNew = new StringBuilder();
                        var newSPFHeaderDetails = new List<string>{"APP_TT_ID", "SO_TYPE", "SO_ORG", "CUST_PO_NO",
                                                                    "CUST_PO_DT","VALID_FROM_DT",
                                                                    "VALID_TO_DT", "SOLD_TO_PARTY", "SHIP_TO_PARTY",
                                                                    "Material_No", "Quantity", "DOC_DT",
                                                                    "CUST_GRP", "Price_List_Type", "PRIC_GRP", "Usage",
                                                                    "INCO1", "INCO2", "BILL_DT",
                                                                    "DEL_PRIO", "PLANT", "STO_LOC",
                                                                    "Max_No_Of_Per_delivery",  "Cond_Type1","Rate",
                                                                    "Cond_Type2", "Rate", "Cond_Type3","Rate","Cust_PO_Type","Your_Reference"}; 
                        foreach (var sauda in saudaSPFList)
                        {
                            sbSPFNew.AppendLine(string.Concat(sauda.Id, ConsoleSettings.CsvDelimiter, "ZSFQ", ConsoleSettings.CsvDelimiter, sauda.VerticalName, ConsoleSettings.CsvDelimiter, sauda.CustomerPoNumber,
                            ConsoleSettings.CsvDelimiter, sauda.CustomerPoDate.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, sauda.ValidFrom.ToString(ConsoleSettings.DateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.ValidTo.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, sauda.SoldToParty, ConsoleSettings.CsvDelimiter, sauda.ShipToParty,
                            ConsoleSettings.CsvDelimiter, sauda.Sku, ConsoleSettings.CsvDelimiter, sauda.BidQuantity, ConsoleSettings.CsvDelimiter, sauda.DocumentDate.ToString(ConsoleSettings.DateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.CustomerGroup, ConsoleSettings.CsvDelimiter, sauda.PriceListType, ConsoleSettings.CsvDelimiter, sauda.PriceGroup, ConsoleSettings.CsvDelimiter, sauda.Usage,
                            ConsoleSettings.CsvDelimiter, sauda.INCO1, ConsoleSettings.CsvDelimiter, sauda.INCO2, ConsoleSettings.CsvDelimiter, sauda.BillDate.ToString(ConsoleSettings.DateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.DeliveryPriority, ConsoleSettings.CsvDelimiter, sauda.UserDepotMapping, ConsoleSettings.CsvDelimiter, sauda.PickingPoint,
                            ConsoleSettings.CsvDelimiter, sauda.MaximumNumberDeliveries, ConsoleSettings.CsvDelimiter, sauda.ConditionType1, ConsoleSettings.CsvDelimiter, sauda.BidAmount,
                            ConsoleSettings.CsvDelimiter, sauda.ConditionType2, ConsoleSettings.CsvDelimiter, sauda.Rate2, ConsoleSettings.CsvDelimiter, sauda.ConditionType3,
                            ConsoleSettings.CsvDelimiter, sauda.Rate3,ConsoleSettings.CsvDelimiter, sauda.CustomerPOType, ConsoleSettings.CsvDelimiter, sauda.TradeTicketNumber, ConsoleSettings.CsvDelimiter));
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaSPFList.Count;
                        GenerateCsvMoveAPPtoSAP(newSPFHeaderDetails, sbSPFNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);

                        break;
                    case ConsoleSettings.TradeTicketFolder:
                        var tradeTicketList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SAPTradeTicketViewDto>)response.SuccessDto.Response : new List<SAPTradeTicketViewDto>(); 
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = tradeTicketList.Count;
                        string[] separators = new string[] { "{1}", "{2}", "{3}", "{4}", "{5}" };
                        var replaceValue = " ";
                        var fileNameNew = ConsoleSettings.FilePathCreation(ConsoleSettings.TradeTicketNewCsv);
                        var fileNameModified = ConsoleSettings.FilePathCreation(ConsoleSettings.TradeTicketModifiedCsv);
                        sbNew = new StringBuilder();
                        var sbModified = new StringBuilder();

                        foreach (var tradeTicket in tradeTicketList)
                        {
                            var oilType = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}");
                            var price = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}");
                            var prcost = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}");
                            var proPortion = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}");
                            var count = 1;
                            //foreach (var tradeTicketDetail in tradeTicket.TradeTicketDetail)
                            //{
                            //    oilType = oilType.Replace("{" + count.ToString() + "}", tradeTicketDetail.OilType);
                            //    price = price.Replace("{" + count.ToString() + "}", tradeTicketDetail.OilCost.ToString());
                            //    prcost = prcost.Replace("{" + count.ToString() + "}", tradeTicketDetail.ProcessCost.ToString());
                            //    proPortion = proPortion.Replace("{" + count.ToString() + "}", tradeTicketDetail.Proportion.ToString());
                            //    count++;
                            //}
                            if (!tradeTicket.IsModified)
                            {
                                sbNew.AppendLine(string.Concat(tradeTicket.Id, ConsoleSettings.CsvDelimiter, tradeTicket.ContractType, ConsoleSettings.CsvDelimiter, tradeTicket.BookingType, ConsoleSettings.CsvDelimiter, tradeTicket.MaterialType, ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(oilType, separators, replaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(price, separators, replaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(prcost, separators, replaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(proPortion, separators, replaceValue),
                                ConsoleSettings.CsvDelimiter, tradeTicket.ContractQuantity, ConsoleSettings.CsvDelimiter, tradeTicket.UnitOfMeasure, ConsoleSettings.CsvDelimiter, tradeTicket.PlantOrVendor, ConsoleSettings.CsvDelimiter, tradeTicket.ContractDate.ToString(ConsoleSettings.DateFormat),
                                ConsoleSettings.CsvDelimiter, tradeTicket.ValidFrom != null && tradeTicket.ValidFrom != DateTime.MinValue ? tradeTicket.ValidFrom.Value.ToString(ConsoleSettings.DateFormat) : string.Empty, ConsoleSettings.CsvDelimiter, tradeTicket.ValidTo != null && tradeTicket.ValidTo != DateTime.MinValue ? tradeTicket.ValidTo.Value.ToString(ConsoleSettings.DateFormat) : string.Empty, ConsoleSettings.CsvDelimiter, tradeTicket.OtherElement, ConsoleSettings.CsvDelimiter));
                            }
                            else
                            {
                                sbModified.AppendLine(string.Concat(tradeTicket.Id, ConsoleSettings.CsvDelimiter, tradeTicket.TradeTicketNumber, ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(price, separators, replaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(prcost, separators, replaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(proPortion, separators, replaceValue),
                                ConsoleSettings.CsvDelimiter, tradeTicket.ContractQuantity, ConsoleSettings.CsvDelimiter));
                            }
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = tradeTicketList.Count;
                        newHeaderDetails = new List<string>{"APP_TT_ID", "CONT_TYPE", "B_TYPE", "MAT_TYPE_H", "OIL_TYPE1", "OIL_TYPE2", "OIL_TYPE", "OIL_TYPE4", "OIL_TYPE5",
                                                                "PRICE1", "PRICE2", "PRICE3", "PRICE4", "PRICE5", "PRCOST1", "PRCOST2", "PRCOST3", "PRCOST4", "PRCOST5",
                                                                "PROPORTION1", "PROPORTION2", "PROPORTION3", "PROPORTION4", "PROPORTION5", "CONT_QTY", "UNIT",
                                                                "PLANT_VEN", "CREATE_DATE", "VALID_FROM", "VALID_TO", "OTH_ELEMENT" };

                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, fileNameNew, subject, false, sapDataSyncResultDto);

                        newHeaderDetails = new List<string>{"APP_TT_ID", "CONT_NUMBER",
                                 "PRICE1", "PRICE2", "PRICE3", "PRICE4", "PRICE5",
                                 "PRCOST1", "PRCOST2", "PRCOST3", "PRCOST4", "PRCOST5",
                                 "PROPORTION1", "PROPORTION2", "PROPORTION3", "PROPORTION4", "PROPORTION5",
                                 "CONT_QTY" };

                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbModified, syncFolder, fileNameModified, subject, true, sapDataSyncResultDto);
                        break;
                    case ConsoleSettings.SpecialityFatTradeTicketFolder:
                        var SFtradeTicketList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SAPTradeTicketViewDto>)response.SuccessDto.Response : new List<SAPTradeTicketViewDto>();
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = SFtradeTicketList.Count;
                        string[] sfseparators = new string[] { "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{10}" };
                        var sfreplaceValue = " ";
                        var SFfileNameNew = ConsoleSettings.FilePathCreation(ConsoleSettings.SpecialityFatTradeTicketNewCsv);
                        var SFfileNameModified = ConsoleSettings.FilePathCreation(ConsoleSettings.SpecialityFatTradeTicketModifiedCsv);
                        sbNew = new StringBuilder();
                        var sbSFModified = new StringBuilder();

                        foreach (var SFtradeTicket in SFtradeTicketList)
                        {
                            var oilType = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}", ConsoleSettings.CsvDelimiter, "{6}", ConsoleSettings.CsvDelimiter, "{7}", ConsoleSettings.CsvDelimiter, "{8}", ConsoleSettings.CsvDelimiter, "{9}", ConsoleSettings.CsvDelimiter, "{10}");
                            var price = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}", ConsoleSettings.CsvDelimiter, "{6}", ConsoleSettings.CsvDelimiter, "{7}", ConsoleSettings.CsvDelimiter, "{8}", ConsoleSettings.CsvDelimiter, "{9}", ConsoleSettings.CsvDelimiter, "{10}");
                            var prcost = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}", ConsoleSettings.CsvDelimiter, "{6}", ConsoleSettings.CsvDelimiter, "{7}", ConsoleSettings.CsvDelimiter, "{8}", ConsoleSettings.CsvDelimiter, "{9}", ConsoleSettings.CsvDelimiter, "{10}");
                            var proPortion = string.Concat("{1}", ConsoleSettings.CsvDelimiter, "{2}", ConsoleSettings.CsvDelimiter, "{3}", ConsoleSettings.CsvDelimiter, "{4}", ConsoleSettings.CsvDelimiter, "{5}", ConsoleSettings.CsvDelimiter, "{6}", ConsoleSettings.CsvDelimiter, "{7}", ConsoleSettings.CsvDelimiter, "{8}", ConsoleSettings.CsvDelimiter, "{9}", ConsoleSettings.CsvDelimiter, "{10}");
                            var count = 1;
                            //foreach (var tradeTicketDetail in SFtradeTicket.TradeTicketDetail)
                            //{
                            //    oilType = oilType.Replace("{" + count.ToString() + "}", tradeTicketDetail.OilType);
                            //    price = price.Replace("{" + count.ToString() + "}", tradeTicketDetail.OilCost.ToString());
                            //    prcost = prcost.Replace("{" + count.ToString() + "}", tradeTicketDetail.ProcessCost.ToString());
                            //    proPortion = proPortion.Replace("{" + count.ToString() + "}", tradeTicketDetail.Proportion.ToString());
                            //    count++;
                            //}
                            if (!SFtradeTicket.IsModified)
                            {
                                sbNew.AppendLine(string.Concat(SFtradeTicket.Id, ConsoleSettings.CsvDelimiter, SFtradeTicket.ContractType, ConsoleSettings.CsvDelimiter, SFtradeTicket.BookingType, ConsoleSettings.CsvDelimiter, SFtradeTicket.MaterialType, ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(oilType, sfseparators, sfreplaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(price, sfseparators, sfreplaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(prcost, sfseparators, sfreplaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(proPortion, sfseparators, sfreplaceValue),
                                ConsoleSettings.CsvDelimiter, SFtradeTicket.ContractQuantity, ConsoleSettings.CsvDelimiter, SFtradeTicket.UnitOfMeasure, ConsoleSettings.CsvDelimiter, SFtradeTicket.PlantOrVendor, ConsoleSettings.CsvDelimiter, SFtradeTicket.ContractDate.ToString(ConsoleSettings.DateFormat),
                                ConsoleSettings.CsvDelimiter, SFtradeTicket.ValidFrom != null && SFtradeTicket.ValidFrom != DateTime.MinValue ? SFtradeTicket.ValidFrom.Value.ToString(ConsoleSettings.DateFormat) : string.Empty, ConsoleSettings.CsvDelimiter, SFtradeTicket.ValidTo != null && SFtradeTicket.ValidTo != DateTime.MinValue ? SFtradeTicket.ValidTo.Value.ToString(ConsoleSettings.DateFormat) : string.Empty, ConsoleSettings.CsvDelimiter, SFtradeTicket.OtherElement, ConsoleSettings.CsvDelimiter));
                            }
                            else
                            {
                                sbSFModified.AppendLine(string.Concat(SFtradeTicket.Id, ConsoleSettings.CsvDelimiter, SFtradeTicket.TradeTicketNumber, ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(price, sfseparators, sfreplaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(prcost, sfseparators, sfreplaceValue), ConsoleSettings.CsvDelimiter,
                                ConsoleSettings.ReplaceString(proPortion, sfseparators, sfreplaceValue),
                                ConsoleSettings.CsvDelimiter, SFtradeTicket.ContractQuantity, ConsoleSettings.CsvDelimiter));
                            }
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = SFtradeTicketList.Count;
                        newHeaderDetails = new List<string>{"APP_TT_ID", "CONT_TYPE", "B_TYPE", "MAT_TYPE_H", "OIL_TYPE1", "OIL_TYPE2", "OIL_TYPE3", "OIL_TYPE4", "OIL_TYPE5","OIL_TYPE6", "OIL_TYPE7", "OIL_TYPE8", "OIL_TYPE9", "OIL_TYPE10",
                                                                "PRICE1", "PRICE2", "PRICE3", "PRICE4", "PRICE5","PRICE6", "PRICE7", "PRICE8", "PRICE9", "PRICE10", "PRCOST1", "PRCOST2", "PRCOST3", "PRCOST4", "PRCOST5","PRCOST6", "PRCOST7", "PRCOST8", "PRCOST9", "PRCOST10",
                                                                "PROPORTION1", "PROPORTION2", "PROPORTION3", "PROPORTION4", "PROPORTION5","PROPORTION6", "PROPORTION7", "PROPORTION8", "PROPORTION9", "PROPORTION10", "CONT_QTY", "UNIT",
                                                                "PLANT_VEN", "CREATE_DATE", "VALID_FROM", "VALID_TO", "OTH_ELEMENT" };

                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, SFfileNameNew, subject, false, sapDataSyncResultDto);

                        newHeaderDetails = new List<string>{"APP_TT_ID", "CONT_NUMBER",
                                 "PRICE1", "PRICE2", "PRICE3", "PRICE4", "PRICE5","PRICE6", "PRICE7", "PRICE8", "PRICE9", "PRICE10",
                                 "PRCOST1", "PRCOST2", "PRCOST3", "PRCOST4", "PRCOST5","PRCOST6", "PRCOST7", "PRCOST8", "PRCOST9", "PRCOST10",
                                 "PROPORTION1", "PROPORTION2", "PROPORTION3", "PROPORTION4", "PROPORTION5","PROPORTION6", "PROPORTION7", "PROPORTION8", "PROPORTION9", "PROPORTION10",
                                 "CONT_QTY" };

                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbSFModified, syncFolder, SFfileNameModified, subject, true, sapDataSyncResultDto);
                        break;
                    case ConsoleSettings.LiftingInquiry:
                        //var liftingRequestEnquiryDtoList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SAPLiftingRequestEnquiryNumberDto>)response.SuccessDto.Response : new List<SAPLiftingRequestEnquiryNumberDto>(); 
                        //sapDataSyncResultDto.OutstandingResult.DataRetrieved = liftingRequestEnquiryDtoList.Count;

                        //newHeaderDetails = new List<string>{"APP_TT_ID", "Sold_To_Party", "Bill_To_Party", "Ship_To_Party", "Sales_Organization","Sys_Date",
                        //                                            "Material_Code","Quantity_Of_Order","Unit_Of_Mes","LiftingRequestDate","LiftingRequestTime","ApproveDate","ApproveTime","CustomerRemarks","VehicleSize"};
                        //foreach (var inputDetails in liftingRequestEnquiryDtoList)
                        //{
                        //    sbNew.AppendLine(string.Concat(
                        //        inputDetails.LiftingRequestId, ConsoleSettings.CsvDelimiter,
                        //        inputDetails.CustomerCode, ConsoleSettings.CsvDelimiter,
                        //        inputDetails.CustomerCode, ConsoleSettings.CsvDelimiter,
                        //        inputDetails.ShipToPartyCode, ConsoleSettings.CsvDelimiter,
                        //        inputDetails.SalesOrganization, ConsoleSettings.CsvDelimiter,
                        //        inputDetails.CreatedDate.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter,
                        //        inputDetails.MaterialNumber, ConsoleSettings.CsvDelimiter,
                        //        Convert.ToInt32(inputDetails.RequiredQuantity), ConsoleSettings.CsvDelimiter,
                        //        inputDetails.UOM, ConsoleSettings.CsvDelimiter, inputDetails.LiftingRequestDate.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, inputDetails.LiftingRequestDate.ToString(ConsoleSettings.TimeFormat, CultureInfo.CurrentCulture), ConsoleSettings.CsvDelimiter,
                        //        inputDetails.ApproveDate != null && inputDetails.ApproveDate != DateTime.MinValue ? inputDetails.ApproveDate.Value.ToString(ConsoleSettings.DateFormat) : string.Empty ,ConsoleSettings.CsvDelimiter,
                        //        inputDetails.ApproveDate != null && inputDetails.ApproveDate != DateTime.MinValue ? inputDetails.ApproveDate.Value.ToString(ConsoleSettings.TimeFormat, CultureInfo.CurrentCulture) : string.Empty,ConsoleSettings.CsvDelimiter, inputDetails.CustomerRemarks, ConsoleSettings.CsvDelimiter,
                        //        inputDetails.VehicleSize, ConsoleSettings.CsvDelimiter));
                        //}
                        //sapDataSyncResultDto.OutstandingResult.DataSynced = liftingRequestEnquiryDtoList.Count;
                        //GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);
                        break;
                    case ConsoleSettings.SaudaConversion:
                        var saudaConversionDtoList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SaudaConversionViewDto>)response.SuccessDto.Response : new List<SaudaConversionViewDto>();
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaConversionDtoList.Count;
                        newHeaderDetails = new List<string>{"APPID", "KUNNR", "WERKS",
                                                                "MATNR_OLD","MENGE_OLD","MATNR_NEW","MENGE_NEW","PR00","FRC1_PRIMARY","FRC1_SECONDARY","UNIT","PackGroup"};                       

                        foreach (var inputDetails in saudaConversionDtoList)
                        {
                            sbNew.AppendLine(string.Concat(inputDetails.SaudaConversionId, ConsoleSettings.CsvDelimiter, inputDetails.Dealer, ConsoleSettings.CsvDelimiter, 
                                inputDetails.Plant,ConsoleSettings.CsvDelimiter, inputDetails.OldMaterialNumber, ConsoleSettings.CsvDelimiter, inputDetails.OldQuantityInCase,
                                ConsoleSettings.CsvDelimiter,inputDetails.NewMaterialNumber, ConsoleSettings.CsvDelimiter, inputDetails.NewQuantityInCase, ConsoleSettings.CsvDelimiter,
                                inputDetails.PROO,ConsoleSettings.CsvDelimiter, inputDetails.PrimaryFright, ConsoleSettings.CsvDelimiter, inputDetails.FRC1, ConsoleSettings.CsvDelimiter, 
                                inputDetails.ToUnit, ConsoleSettings.CsvDelimiter, inputDetails.PackGroup, ConsoleSettings.CsvDelimiter));
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaConversionDtoList.Count;
                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);

                        break;
                    case ConsoleSettings.SaudaExtension:
                        var saudaExtensionDtoList = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SAPSaudaExtension>)response.SuccessDto.Response : new List<SAPSaudaExtension>();
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaExtensionDtoList.Count;
                        newHeaderDetails = new List<string>{"sauda_number", "extended_date"};

                        foreach (var inputDetails in saudaExtensionDtoList)
                        {
                            sbNew.AppendLine(string.Concat(inputDetails.SaudaNumber, ConsoleSettings.CsvDelimiter,inputDetails.ExtensionDate.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter));
                        }
                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaExtensionDtoList.Count;
                        GenerateCsvMoveAPPtoSAP(newHeaderDetails, sbNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);

                        break;

                    case ConsoleSettings.SaudaLooseOilFolder:
                        //Loose Sauda
                        var saudaListwithLooseVertical = !string.IsNullOrEmpty(response.SuccessDto.Response.ToString()) ? (List<SaudaViewDto>)response.SuccessDto.Response : new List<SaudaViewDto>();
                        sapDataSyncResultDto.OutstandingResult.DataRetrieved = saudaListwithLooseVertical.Count;
                        var sbLooseNew = new StringBuilder();
                        var newLooseHeaderDetails = new List<string>{"APP_TT_ID","Cust_PO_Type", "CUST_PO_NO",
                                                                    "CUST_PO_DT","SOLD_TO_PARTY", "SHIP_TO_PARTY", "BILL_TO_PARTY" , "Payer" ,"VALID_FROM_DT",
                                                                    "VALID_TO_DT", "Material_No","Quantity","PLANT","STO_LOC",
                                                                    "INCO1", "INCO2","Cond_Type1",
                                                                    "Rate","Cond_Type2", "Rate","BookingId"};
                        foreach (var sauda in saudaListwithLooseVertical)
                        {

                            sbLooseNew.AppendLine(string.Concat(sauda.Id, ConsoleSettings.CsvDelimiter, sauda.CustomerPOType , ConsoleSettings.CsvDelimiter, sauda.CustomerPoNumber,
                            ConsoleSettings.CsvDelimiter, sauda.CustomerPoDate.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, sauda.SoldToParty, ConsoleSettings.CsvDelimiter, sauda.ShipToParty, ConsoleSettings.CsvDelimiter ,
                            sauda.BillToParty, ConsoleSettings.CsvDelimiter, sauda.Payer, ConsoleSettings.CsvDelimiter, sauda.ValidFrom.ToString(ConsoleSettings.DateFormat),
                            ConsoleSettings.CsvDelimiter, sauda.ValidTo.ToString(ConsoleSettings.DateFormat), ConsoleSettings.CsvDelimiter, 
                            sauda.Sku, ConsoleSettings.CsvDelimiter, sauda.BidQuantity, ConsoleSettings.CsvDelimiter, sauda.UserDepotMapping, ConsoleSettings.CsvDelimiter, sauda.PickingPoint,
                            ConsoleSettings.CsvDelimiter, sauda.INCO1, ConsoleSettings.CsvDelimiter, sauda.INCO2, ConsoleSettings.CsvDelimiter, sauda.ConditionType1,
                            ConsoleSettings.CsvDelimiter, sauda.BidAmount, ConsoleSettings.CsvDelimiter, sauda.ConditionType2, ConsoleSettings.CsvDelimiter, sauda.Rate2, ConsoleSettings.CsvDelimiter, sauda.BookingId, ConsoleSettings.CsvDelimiter
                            ));
                        }

                        sapDataSyncResultDto.OutstandingResult.DataSynced = saudaListwithLooseVertical.Count;
                        GenerateCsvMoveAPPtoSAP(newLooseHeaderDetails, sbLooseNew, syncFolder, csvFileName, subject, false, sapDataSyncResultDto);
                         break;

                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception:{exception}";
                _logger.Error(message);
                SendNotification(subject, message, syncFolder, sapDataSyncResultDto, null, true);
            }
        }

        /// <summary>
        /// Generate CSV file Move APP to SAP
        /// </summary>
        /// <param name="headerString"></param>
        /// <param name="detailsData"></param>
        /// <param name="syncFolder"></param>
        /// <param name="fileName"></param>
        /// <param name="subject"></param>
        /// <param name="isModified"></param>
        /// <param name="sapDataSyncResultDto"></param>
        public void GenerateCsvMoveAPPtoSAP(List<string> headerString, StringBuilder detailsData, string syncFolder, string fileName, string subject, bool isModified = false, SapDataSyncResultDto sapDataSyncResultDto = null)
        {
            _methodName = "GenerateCsvMoveAPPtoSAP";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            if (detailsData != null && detailsData.ToString() != "")
            {
                var syncDataResult = new SyncDataResult();
                var _sftpConnectorService = new SftpConnectorService();
                var syncMessage = string.Empty;
                try
                {
                    var directoryPathNew = ConsoleSettings.OutboundDirectoryPath(syncFolder, isModified);
                    var sb = new StringBuilder();
                    var headerDetails = String.Empty;
                    foreach (var item in headerString)
                    {
                        headerDetails = string.Concat(headerDetails, item, ConsoleSettings.CsvDelimiter);
                    }
                    //Title of the table
                    sb.AppendLine(headerDetails);
                    sb.AppendLine(detailsData.ToString());
                    var pathNew = ConsoleSettings.SystemPath(fileName);
                    var filePath = new List<string>();
                    File.WriteAllText(pathNew, sb.ToString().TrimEnd());
                    filePath.Add(pathNew);
                    syncDataResult.FilePath.Add(pathNew);
                    _sftpConnectorService.CreateDirectorySFTPFile(filePath, directoryPathNew);
                    syncDataResult.PostStatus = true;
                    syncMessage = ConsoleSettings.SyncSuccessMessage;
                    sapDataSyncResultDto.SyncCompletedDateTime = DateTime.Now;
                    SendNotification(subject, syncMessage, syncFolder, sapDataSyncResultDto, syncDataResult.FilePath);
                }
                catch (Exception ex)
                {
                    syncMessage = $"SAP Service : {ServiceName} Controller-Method {_methodName} Exception:{ex.ToString()}";
                    SendNotification(subject, syncMessage, syncFolder, sapDataSyncResultDto, syncDataResult.FilePath, true);
                    _logger.Error(syncMessage);
                }
            }

        }
        #endregion
    }
}
