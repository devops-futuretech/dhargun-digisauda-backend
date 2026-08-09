using Adani.Solution.Console.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Pricing.Services
{
    public class SapTemplateDeleteService
    {
        private const string ServiceName = "DeleteSAPTemplateFiles";
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _methodName;
        public void DeleteSAPTemplateFiles()
        {
            _methodName = "DeleteSAPTemplateFiles";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var templatePath = WebConfig.SapTemplateFileDirctoryPath;
                //var defaultTemplate = WebConfig.SapTemplateFileName;
                //var defaultTemplateType = WebConfig.SapTemplateFileType;
                string[] filePaths = Directory.GetFiles(templatePath);
                foreach (string filePath in filePaths)
                {
                    //var filename = new FileInfo(filePath).Name;
                    //var filetype = new FileInfo(filePath).Extension;
                    //filename = filename.ToLower();
                    //if (filetype.ToLower() == defaultTemplateType.ToLower())
                    //{
                    //    if (filename != defaultTemplate.ToLower())
                    //    {
                    File.Delete(filePath);
                    //    }
                    //}

                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            
        }
    }
}
