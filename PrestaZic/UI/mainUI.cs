using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PrestaZic
{
    public partial class mainUI : Form
    {
        string printerName = ConfigurationManager.AppSettings["PrinterName"].ToString();
        string enablePrinter = ConfigurationManager.AppSettings["EnablePrinter"].ToString();
        public mainUI()
        {
            InitializeComponent();
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
            ResizeLbl();
            
        }
        private void ResizeLbl()
        {
            // Taille de la fenêtre
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;
            // Position du texte sous le loader
            lbl_startArg.Left = (formWidth - lbl_startArg.Width) / 2;
            lbl_startArg.Top = img_loader.Bottom + 10; // Ajuster l'espacement avec le loader
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

        private string startWSLmachine(string vmName)
        {
            string result = "Erreur";
            try
            {
                // Créer une instance de ManagementObjectSearcher pour récupérer la VM
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Msvm_ComputerSystem WHERE ElementName = '" + vmName + "'");
                
                foreach (ManagementObject vm in searcher.Get())
                {
                    // Démarrer la VM
                    ManagementBaseObject inParams = vm.GetMethodParameters("PowerOn");
                    ManagementBaseObject outParams = vm.InvokeMethod("PowerOn", inParams, null);

                    if (outParams["ReturnValue"].ToString() == "0")
                    {
                        Console.WriteLine($"VM '{vmName}' démarrée avec succès.");
                        result = "Serveur démarré !";
                    }
                    else
                    {
                        Console.WriteLine($"Erreur lors du démarrage de la VM. Code d'erreur : {outParams["ReturnValue"]}");
                        result = $"Erreur lors du démarrage de la VM. Code d'erreur : {outParams["ReturnValue"]}";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
                return "Erreur." + ex.Message;
            }
        }
 

        private void SetLblMsg(string text)
        {
            lbl_startArg.Text = text;
            ResizeLbl();
        }

        private async void mainUI_Shown(object sender, EventArgs e)
        {
            if(enablePrinter == "Y")
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
                await Task.Delay(2000);
                SetLblMsg("Démarrage du service WSLService...");
                await Task.Delay(500);
                SetLblMsg(StartWindowsService("WSLService"));
                await Task.Delay(2000);
                SetLblMsg("Démarrage du serveur...");
                await Task.Delay(2000);
                await ExecuteCommandAsync("wsl --distribution Ubuntu");
                await Task.Delay(2000);
                SetLblMsg("Démarrage du service PrestaZic...");
                await Task.Delay(500);
                SetLblMsg(StartWindowsService("PrestaZic"));
                await Task.Delay(2000);
                SetLblMsg("Ouverture du photobooth...");
                await Task.Delay(5000);
                await ExecuteCommandAsync("\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\" --kiosk localhost");
            }
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
    }
}
