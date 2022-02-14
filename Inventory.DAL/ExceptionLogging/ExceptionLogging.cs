using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Inventory.DAL.ExceptionLogging
{
    public static class ExceptionLogging
    {
        private static string ErrorLineNo, ErrorMessage, ExType, ErrorLocation;
        public static void WriteException(Exception ex)
        {
            var Line = Environment.NewLine + Environment.NewLine;
            ErrorLineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            ErrorMessage = ex.GetType().Name.ToString();
            ExType = ex.GetType().ToString();
            ErrorLocation = ex.Message.ToString();
            try
            {
                string filePath = @"E:\.net\Inventory\Exceptions";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath += "\\"+ DateTime.Today.ToString("dd MMM yyyy") + ".txt";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    string error = "Log Written Date: " + " " + DateTime.Now.ToString() + Line + " Error Line No:" + ErrorLineNo + Line + "Error Message:" + " " + ErrorMessage + Line + "Exception Type:" + " " + ExType + Line + "Error Location :" + " " + ErrorLocation + Line + DateTime.Now.ToString() + "-----------------";
                    writer.WriteLine("-------------------------------------------------------------------------------------");
                    writer.WriteLine(Line);
                    writer.WriteLine(error);
                    writer.WriteLine("-------------------------------------------------------------------------------------");
                    writer.WriteLine(Line);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
                throw ex;
            }
        }
    }
}
