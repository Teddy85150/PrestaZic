using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows.Forms;
using AutoUpdaterDotNET;

namespace PrestaZic
{
    public partial class main : ServiceBase
    {
        Log log = new Log();
        static System.Timers.Timer timer = new System.Timers.Timer();
        static HttpSelfHostServer server;
        int retryFTPprocess = 0;
        public main()
        {
            
        }

        protected override void OnStart(string[] args = null)
        {
            log.WriteToFile("PrestaZic - Version " + Assembly.GetEntryAssembly().GetName().Version);
            log.WriteToFile("Service is started at " + DateTime.Now);
            log.WriteToFile("Starting web server...");
#if !DEBUG
            var config = new HttpSelfHostConfiguration("http://localhost:8010");
#else
            var config = new HttpSelfHostConfiguration("http://localhost:8009");
#endif
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "PrestaZic config",
                routeTemplate: "admin/config/{action}",
                defaults: new { controller = "Administration" }
                );
            log.WriteToFile("Activated : API for PrestaZic administration");

            config.Routes.MapHttpRoute(
                name: "PrestaZic Photobooth",
                routeTemplate: "photobooth/{action}",
                defaults: new { controller = "Photobooth" }
                );
            log.WriteToFile("Activated : API for Photobooth");

            config.MaxReceivedMessageSize = 104857600;  // 100 Mo (en octets)
            config.MaxBufferSize = 104857600;
            log.WriteToFile("Web server : increasing max request limit to 100 Mo");

            server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
            log.WriteToFile("Web server started !");

            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = (int.Parse(ConfigurationManager.AppSettings["TimerPrestaZicFunctionsInSecond"].ToString()) * 1000); //number in milisecinds
            timer.Enabled = true;
            timer.Start();
            log.WriteToFile("Timer set to launch PrestaZic functions every " + ConfigurationManager.AppSettings["TimerPrestaZicFunctionsInSecond"].ToString() + " seconds");
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            log.WriteToFile("Service is recall at " + DateTime.Now);
            OnWorking();
        }

        private void OnWorking()
        {
            log.WriteToFile("Service is working !");
            if(retryFTPprocess < 5)
            {
                log.WriteToFile("Service will try to connect to FTP and send images data !");
                if (IsConnectedToInternet())
                {
                    transferData transferData = new transferData();
                }
                else retryFTPprocess++;
            }
        }

        static bool IsConnectedToInternet()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 3000); // Google DNS
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        protected override void OnStop()
        {
            log.WriteToFile("Service stopped");
        }

        private void OnError(string exception = "Internal error")
        {
            // Write to file error
            log.WriteToFile("Error : " + exception + ". Service will be stopped");
            Stop();
        }

        internal void OnDebug(string action)
        {
            switch (action) {
                case "update":
                    CheckUpdate();
                    Stop();
                    break;
                case "UI":
                    Application.Run(new mainUI());
                    break;
                default:
                    Console.Write("Starting PrestaZic in DEBUG mode");
                    OnStart();
                    break;
            }
        }

        public void CheckUpdate()
        {
            AutoUpdater.Synchronous = true;
            AutoUpdater.Start("ftp://cloud.tedev.fr/prestazic_service_version.xml", new NetworkCredential("prestazic@cloud.tedev.fr", "bPFZcvWpL]!I"));
        }
    }
}
