using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestaZic
{
    internal class Log
    {
        private static readonly object verrou = new object();

       public void WriteToFile(string Message)
        {
#if DEBUG
            Console.WriteLine(Message);
#endif
            lock(verrou)
            {
                WriteToFileLock(Message);
            }
        }

        private static void WriteToFileLock(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            using (var sw = File.AppendText(filepath))
            {
                sw.WriteLine(DateTime.Now.ToString("dddd dd MMMM yyyy à HH:mm:ss") + "    : " + Message);
            }
        }

    }
}
