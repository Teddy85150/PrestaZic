using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrestaZic
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var MyService = new main();
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "--update":
                        MyService.OnDebug("update");
                        break;
                    case "--UI":
                        MyService.OnDebug("UI");
                        break;
                }
            }
            else
            {
#if DEBUG
                // Debug the Windows Service.
                MyService.OnDebug("");
                Thread.Sleep(Timeout.Infinite);
#else
                ServiceBase.Run(MyService);
#endif
            }
        }
    }
}
