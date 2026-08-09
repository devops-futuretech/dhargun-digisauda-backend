using System.IO;
using System.Web;

namespace Adani.Solution.API.Log
{
    public static class Log
    {
        public static string CreateLogFile()
        {
            string filePath = HttpContext.Current.Server.MapPath("Log");
            string path = filePath + "\\log.txt";
            //path = @"E:\AppServ\Example.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();

                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine("The very first line!");
                }
            }
            else if (File.Exists(path))
            {

            }
            return path;
        }
    }
}