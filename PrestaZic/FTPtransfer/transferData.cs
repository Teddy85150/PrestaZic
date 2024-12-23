using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PrestaZic
{
    public class transferData
    {
        public transferData() 
        {
            string ftpServer = "ftp://exemple.com/chemin/repertoire"; // URL du serveur FTP
            string ftpUsername = ConfigurationManager.AppSettings["TimerPrestaZicFunctionsInSecond"].ToString();
            string ftpPassword = ConfigurationManager.AppSettings["TimerPrestaZicFunctionsInSecond"].ToString();
            
            string localFolder = @"C:\Temp\PrestaZic\Photobooth\SaveTmp"; // Répertoire local contenant les fichiers
            string[] files = Directory.GetFiles(localFolder); // Récupère tous les fichiers dans le répertoire

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath); // Nom du fichier à envoyer
                string ftpFilePath = $"{ftpServer}/{fileName}"; // Chemin complet sur le serveur FTP

                try
                {
                    // Envoi du fichier au serveur FTP
                    UploadFileToFtp(ftpFilePath, filePath, ftpUsername, ftpPassword);

                    // Suppression locale après un envoi réussi
                    File.Delete(filePath);
                    Console.WriteLine($"Fichier '{fileName}' envoyé et supprimé localement.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du transfert de '{fileName}': {ex.Message}");
                }
            }
        }

        static void UploadFileToFtp(string ftpFilePath, string localFilePath, string username, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFilePath);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Authentification
            request.Credentials = new NetworkCredential(username, password);

            // Lecture du fichier local
            byte[] fileContents = File.ReadAllBytes(localFilePath);
            request.ContentLength = fileContents.Length;

            // Écriture sur le flux de requête
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            // Vérification de la réponse du serveur FTP
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload terminé : {response.StatusDescription}");
            }
        }
    }
}
