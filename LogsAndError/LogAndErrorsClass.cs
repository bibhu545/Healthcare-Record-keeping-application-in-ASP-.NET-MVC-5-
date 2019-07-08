using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace LogsAndError
{
    public class LogAndErrorsClass
    {
        static String recordFilePath = System.Configuration.ConfigurationManager.AppSettings["recordFile"];
        public void CatchException(Exception ex)
        {
            using (StreamWriter sw = File.AppendText(recordFilePath + DateTime.Now.ToString("dd.MM.yyyy") + ".txt"))
            {
                sw.WriteLine("" + ex.Message + " on " + DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
            }
        }
    }
}
