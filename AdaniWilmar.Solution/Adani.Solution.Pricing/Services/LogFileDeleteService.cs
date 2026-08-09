using Adani.Solution.Console.Common;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Adani.Solution.Pricing.Services
{
    public class LogFileDeleteService
    {
        private const string ServiceName = "LogFileDeleteService";
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _methodName;

        public void MoveAndDeleteLogFile()
        {
            _methodName = "MoveAndDeleteLogFile";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                    var oldfilePath = WebConfig.IsLogFileDirctoryPathOld;
                    var newDirectoryPath = WebConfig.IsLogFileDirctoryPathNew;
                    FileInfo file = new FileInfo(oldfilePath);
                    if (file.Exists)
                    {
                        _logger.Info("File Readed");
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow).ToString("MM_dd_yyyy_hh_mm_ss");
                        var newFileName = $@"{newDirectoryPath}\{"Debug_API" + "_" + currentDate + ".log"}";                    
                        file.MoveTo(newFileName);
                    }

                    List<string> DeletePath = new List<string>();
                    DirectoryInfo info = new DirectoryInfo(WebConfig.IsLogFileDirctoryPathNew);
                    FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo data in files)
                    {
                        DateTime CreationTime = file.CreationTime;
                        DateTime whichFileTobeDeleted = DateTime.Now.AddDays(WebConfig.ThreeDaysBackup);
                        if (data.CreationTime < whichFileTobeDeleted)
                        {
                           data.Delete();
                        }
                    }           
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
        }

        public void DeleteBackupLogFile()
        {
            _methodName = "DeleteBackupLogFile";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var templatePath = WebConfig.LogBackupFolderPath;
                var deletefilename= WebConfig.LogBackupFolderFileName;
                
                string[] filePaths = Directory.GetFiles(templatePath);
                foreach (string filePath in filePaths)
                {
                    
                    var filename = new FileInfo(filePath).Name;
                    var filedate = new FileInfo(filePath).LastWriteTime;
                    var deletionTime = int.Parse(WebConfig.LogBackupFolderDeleteDays);
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    var datedif = (currentDate - filedate).Days;
                    if (datedif > deletionTime)
                    {
                        File.Delete(filePath);
                    }

                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
        }
    }
}
