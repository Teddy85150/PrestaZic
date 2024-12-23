using ManagedNativeWifi;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Diagnostics;



namespace PrestaZic.UI
{
    public partial class WifiSettings : Form
    {
        Log log = new Log();

        public WifiSettings()
        {
            InitializeComponent();
            ListAvailableNetworks();
        }

        private async Task RefreshNetworksAsync()
        {
            // Appelle la méthode pour scanner les réseaux sans fil
            await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(10));
            // Après le scan, mettez à jour la liste des réseaux
            ListAvailableNetworks();
        }

        private async void ListAvailableNetworks()
        {
            await RefreshNetworksAsync();
            // Clear the ListBox before adding new entries
            listBoxNetworks.Items.Clear();

            // Get all available SSIDs
            var ssids = NativeWifi.EnumerateAvailableNetworkSsids();

            foreach (var ssidInfo in ssids)
            {
                // Convert SSID to string
                string ssid = Encoding.UTF8.GetString(ssidInfo.ToBytes()).TrimEnd('\0');
                listBoxNetworks.Items.Add(ssid);
            }
        }

        private string ExecuteCommand(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private void ConnectToNetwork(string ssid, string password)
        {

            
            string profileXml = $@"
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <SSIDConfig>
        <SSID>{ssid}</SSID>
    </SSIDConfig>
    <ConnectionType>ibss</ConnectionType>
    <ConnectionMode>auto</ConnectionMode>
    <MACRandomization>enabled</MACRandomization>
    <keyManagement>wpa2psk</keyManagement>
    <security>
        <authEncryption>
            <authentication>wpa2-psk</authentication>
            <encryption>aes</encryption>
            <useOneX>false</useOneX>
        </authEncryption>
        <sharedKey>
            <keyType>passPhrase</keyType>
            <key>{password}</key>
        </sharedKey>
    </security>
</WLANProfile>";

            string cmd = $"netsh wlan add profile filename=\"profile.xml\"";

            // Sauvegardez le XML dans un fichier
            System.IO.File.WriteAllText("profile.xml", profileXml);
            log.WriteToFile("Export path profile");

            string output = ExecuteCommand(cmd);

            // Vérifiez si le profil a été ajouté avec succès
            if (!output.Contains("successfully"))
            {
                MessageBox.Show($"Erreur lors de l'ajout du profil : {output}");
                return;
            }

            // Tentez de vous connecter au réseau
            string connectCommand = $"netsh wlan connect name=\"{ssid}\"";
            output = ExecuteCommand(connectCommand);

            // Vérifiez la sortie pour des messages de succès ou d'erreur
            if (output.Contains("successfully connected"))
            {
                MessageBox.Show($"Connexion réussie avec {ssid}");
            }
            else
            {
                MessageBox.Show($"Échec de la connexion à {ssid}: {output}");
            }
        }

        private void listBoxNetworks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxNetworks.SelectedItem != null)
            {
                string selectedSSID = listBoxNetworks.SelectedItem.ToString();

                // Demander le mot de passe pour se connecter
                string password = PromptForPassword();

                ConnectToNetwork(selectedSSID, password);
            }
        }

        private string PromptForPassword()
        {
            // Affichez un dialogue pour entrer le mot de passe
            using (Form prompt = new Form())
            {
                prompt.Width = 300;
                prompt.Height = 150;
                prompt.Text = "Entrer le mot de passe";

                Label textLabel = new Label() { Left = 20, Top = 20, Text = "Mot de passe:" };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240, UseSystemPasswordChar = true };
                Button confirmation = new Button() { Text = "OK", Left = 200, Width = 60, Top = 70 };

                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.ShowDialog();

                return textBox.Text;
            }
        }

    }
}
