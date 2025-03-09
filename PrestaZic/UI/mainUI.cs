using PrestaZic.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Pipes;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PrestaZic
{
    public partial class mainUI : Form
    {

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const byte VK_MENU = 0x12; // Touche ALT
        private const byte VK_TAB = 0x09; // Touche TAB
        private const uint KEYEVENTF_KEYUP = 0x0002;

        string printerName = ConfigurationManager.AppSettings["PrinterName"].ToString();
        string enablePrinter = ConfigurationManager.AppSettings["EnablePrinter"].ToString();        
        WifiSettings wifiSettings = new WifiSettings();
        private readonly HttpClient _httpClient = new HttpClient();
        static HttpSelfHostServer server;

        public mainUI()
        {
            InitializeComponent();
            Focus();
            //pipeClient.Connect();
#if !DEBUG
            var config = new HttpSelfHostConfiguration("http://localhost:8012");
#else
            var config = new HttpSelfHostConfiguration("http://localhost:8011");
#endif
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "PrestaZic client UI",
                routeTemplate: "client/{action}",
                defaults: new { controller = "Client" }
                );
            Console.WriteLine("Activated : API for PrestaZic Client UI");

            server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
            Console.WriteLine("Web server started !");
        }

        private void CenterElements()
        {
            img_logo.Size = new Size(img_logo.Width + 20, img_logo.Height + 20);
            img_loader.Size = new Size(img_loader.Width + 10, img_loader.Height + 10);
            lbl_startArg.Font = new Font(lbl_startArg.Font.FontFamily, lbl_startArg.Font.Size + 2, FontStyle.Bold);

            // Taille de la fenêtre
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // PictureBox principale (image)
            int mainPictureWidth = img_logo.Width;
            int mainPictureHeight = img_logo.Height;

            // PictureBox du loader
            int loaderPictureWidth = img_loader.Width;
            int loaderPictureHeight = img_loader.Height;

            // Label du texte
            int labelHeight = lbl_startArg.Height;

            // Calcul de la position centrée pour la première PictureBox (image)
            img_logo.Left = (formWidth - mainPictureWidth) / 2;
            img_logo.Top = (formHeight - mainPictureHeight) / 3; // Ajustement pour centrer plus haut

            // Calcul de la position centrée pour la deuxième PictureBox (loader)
            img_loader.Left = (formWidth - loaderPictureWidth) / 2;
            img_loader.Top = img_logo.Bottom + 20; // Ajuster l'espacement avec la première PictureBox
            ResizeLbl(lbl_startArg);

            btn_wifi.Location = new Point(10, 150);
            btn_wifi.Size = new Size(200, 100);
            btn_wifi.Font = new Font(btn_shutdown.Font.FontFamily, 20);
            btn_wifi.BackColor = Color.White;
            btn_wifi.ForeColor = Color.Black;
            btn_wifi.FlatStyle = FlatStyle.Flat;

            if(ConfigurationManager.AppSettings["EnableWifiSupport"].ToString() == "Y") btn_wifi.Visible = true;

        }
        private void ResizeLbl(Label label)
        {
            // Taille de la fenêtre
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;
            // Position du texte sous le loader
            label.Left = (formWidth - label.Width) / 2;
            label.Top = img_loader.Bottom + 10; // Ajuster l'espacement avec le loader
        }

        private void mainUI_Resize(object sender, EventArgs e)
        {
            CenterElements();
        }

        public async Task<string> ExecuteCommandAsync(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,   // Rediriger la sortie standard
                RedirectStandardError = true,    // Rediriger les erreurs
                UseShellExecute = false,         // Ne pas utiliser le shell
                CreateNoWindow = true            // Ne pas afficher de fenêtre
            };

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();

                // Lire la sortie et les erreurs de manière asynchrone
                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                // Démarrer un délai d'attente de 3 secondes
                var timeoutTask = Task.Delay(3000);

                // Attendre que le processus se termine ou que le délai d'attente expire
                var completedTask = await Task.WhenAny(Task.Run(() => process.WaitForExit()), timeoutTask);

                if (completedTask == timeoutTask)
                {
                    // Si le délai d'attente est atteint, tuer le processus
                    if (!process.HasExited)
                    {
                        Console.WriteLine("Le processus n'a pas répondu, il va être fermé.");
                        process.Kill(); // Fermer le processus
                    }
                    return "Erreur : Le processus a été fermé après 3 secondes d'attente.";
                }

                // Récupérer les sorties après que le processus a terminé
                string output = await outputTask;
                string error = await errorTask;

                if (!string.IsNullOrEmpty(error))
                {
                    return $"Erreur : {error}";
                }

                return output;
            }
        }

        private string StartWindowsService(string serviceName)
        {
            try
            {
                using (ServiceController serviceController = new ServiceController(serviceName))
                {
                    // Vérifier si le service est arrêté avant de le démarrer
                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        Console.WriteLine($"Démarrage du service '{serviceName}'...");
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running); // Attendre que le service soit en cours d'exécution
                        Console.WriteLine($"Le service '{serviceName}' a démarré avec succès.");
                        return $"Service {serviceName} démarré !";
                    }
                    else
                    {
                        Console.WriteLine($"Le service '{serviceName}' est déjà en cours d'exécution.");
                        return $"Service {serviceName} déjà démarré !";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du démarrage du service : {ex.Message}");
                return $"Erreur lors du démarrage du service {serviceName}, merci de contacter l'équipe PrestaZic";
            }
        } 

        private void SetLblMsg(string text)
        {
            lbl_startArg.Text = text;
            ResizeLbl(lbl_startArg);
        }

        private async void mainUI_Shown(object sender, EventArgs e)
        {
            lbl_help.Text = "Assisstance Presta'Zic \nTeddy : 06 30 16 74 26 \nUgo : 06 40 24 79 08";
            lbl_help.Font = new Font(lbl_help.Font.FontFamily, lbl_help.Font.Size + 6);
            int padding = 10; // Padding from the edges
            lbl_help.Location = new Point(this.ClientSize.Width - lbl_help.Width - padding, padding);

            if (enablePrinter == "Y")
            {
                SetLblMsg("Vérification de l'imprimante...");
                await Task.Delay(2000);
                if (IsPrinterInstalled(printerName))
                {
                    SetLblMsg("Imprimante installée, connexion...");
                    await Task.Delay(2000);
                    bool isConnected = await Task.Run(() => IsPrinterConnected(printerName));
                    if (isConnected)
                    {
                        SetLblMsg("Connecté à l'imprimante");
                    }
                    else
                    {
                        SetLblMsg("Erreur de connexion à l'imprimante");
                    }
                }
                else
                {
                    SetLblMsg("Aucune imprimante installée");
                }
            }
            await Task.Delay(2000);
            SetLblMsg("Démarrage du service WSLService...");
            await Task.Delay(500);
            SetLblMsg(StartWindowsService("WSLService"));
            await Task.Delay(2000);
            SetLblMsg("Démarrage du système...");                
            await ExecuteCommandAsync("wsl");
            await Task.Delay(5000);
            SetLblMsg("Connexion à l'appareil photo...");
            await Task.Delay(2000);
            await ExecuteCommandAsync("c:\\PrestaZicService\\wsl.bat");
            await Task.Delay(2000);
            SetLblMsg("Démarrage du service PrestaZic...");
            await Task.Delay(500);
            SetLblMsg(StartWindowsService("PrestaZic"));
            await Task.Delay(2000);
            SetLblMsg("Ouverture du photobooth...");
            await Task.Delay(5000);
            await ExecuteCommandAsync("\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\" --kiosk localhost");
            await Task.Delay(5000);
            SetLblMsg("Le photobooth est démarré !");
            await Task.Delay(5000);
            SetLblMsg("");
            img_loader.Hide();
            btn_shutdown.Location = new Point(10, 10);
            btn_shutdown.Size = new Size(200, 100);
            btn_shutdown.Font = new Font(btn_shutdown.Font.FontFamily, 20);
            btn_shutdown.BackColor = Color.Red;
            btn_shutdown.ForeColor = Color.White;
            btn_shutdown.FlatStyle = FlatStyle.Flat;
            btn_shutdown.Show();

            btn_sendToDrive.Location = new Point(10, 300);
            btn_sendToDrive.Size = new Size(200, 100);
            btn_sendToDrive.Font = new Font(btn_sendToDrive.Font.FontFamily, 20);
            btn_sendToDrive.FlatStyle = FlatStyle.Flat;
            btn_sendToDrive.Show();

        }

        // Vérifie si l'imprimante est installée
        static bool IsPrinterInstalled(string printerName)
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                if (printer.Equals(printerName, StringComparison.OrdinalIgnoreCase))
                {
                    return true; // Imprimante trouvée
                }
            }
            return false; // Imprimante non trouvée
        }

        // Vérifie si l'imprimante est connectée via USB
        static bool IsPrinterConnected(string printerName)
        {
            string query = "SELECT * FROM Win32_Printer WHERE Name LIKE '%" + printerName + "%'";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject printer in searcher.Get())
                {
                    string portName = printer["PortName"]?.ToString();
                    bool isOnline = printer["WorkOffline"]?.ToString().ToLower() == "false"; // Vérifie si l'imprimante n'est pas hors ligne

                    // Vérifie si l'imprimante est connectée via un port USB et en ligne
                    if (portName != null && portName.StartsWith("USB", StringComparison.OrdinalIgnoreCase) && isOnline)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void btn_wifi_Click(object sender, EventArgs e)
        {            
            wifiSettings.Show();
            wifiSettings.Focus();
        }

        private async void btn_sendToDrive_ClickAsync(object sender, EventArgs e)
        {
#if DEBUG
            string url = "http://localhost:8009/photobooth/SendToDrive"; // Remplace par ton URL
#else
            string url = "http://localhost:80010/photobooth/SendToDrive"; // Remplace par ton URL
#endif
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                MessageBox.Show(responseBody, "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void BringToFront()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr hWnd = currentProcess.MainWindowHandle;

            if (hWnd != IntPtr.Zero)
            {
                if (IsIconic(hWnd)) // Vérifie si la fenêtre est minimisée
                {
                    ShowWindow(hWnd, SW_RESTORE); // Restaurer la fenêtre
                }

                // Met la fenêtre en "Always On Top", puis la remet en normal
                SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
                SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);

                SetForegroundWindow(hWnd); // Donner le focus
            }
        }

        public static void ForceFocus()
        {
            keybd_event(VK_MENU, 0, 0, 0);  // Simule ALT enfoncé
            keybd_event(VK_TAB, 0, 0, 0);  // Simule ALT enfoncé
            keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, 0); // Relâche ALT

            BringToFront(); // Ensuite, ramène la fenêtre au premier plan
        }
    }
}
