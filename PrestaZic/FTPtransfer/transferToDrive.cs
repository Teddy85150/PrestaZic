using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrestaZic
{
    public class transferToDrive
    {
        static string[] Scopes = { DriveService.Scope.DriveFile };
        static string serviceAccountFile = @ConfigurationManager.AppSettings["GoogleDriveAPIkey"].ToString();
        static string ApplicationName = "PhotoboothApp";
        static string tempFolderPath = @ConfigurationManager.AppSettings["SaveImgTmp"].ToString(); // Ton dossier temporaire
        static string driveFolderId = ConfigurationManager.AppSettings["GoogleDriveFolderID"].ToString(); // Remplace par l'ID du dossier Google Drive

        public transferToDrive()
        {
            try
            {
                var credential = GoogleCredential.FromFile(serviceAccountFile)
                    .CreateScoped(DriveService.ScopeConstants.DriveFile);

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "PhotoboothApp",
                });

                // Récupérer tous les fichiers images du dossier temporaire
                var images = Directory.GetFiles(tempFolderPath, "*.jpg"); // Modifie l'extension si nécessaire

                foreach (var imagePath in images)
                {
                    try
                    {
                        UploadFile(service, imagePath);
                        File.Delete(imagePath); // Supprime après upload réussi
                        Console.WriteLine($"Supprimé: {imagePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'upload de {imagePath}: {ex.Message}");
                    }
                }

                Console.WriteLine("Upload terminé !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur principale: {ex.Message}");
            }
        }

        static void UploadFile(DriveService service, string filePath)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath),
                Parents = new[] { driveFolderId } // Place le fichier dans le dossier Drive
            };

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var request = service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
                Console.WriteLine($"Fichier uploadé: {filePath} (ID: {request.ResponseBody.Id})");
            }
        }
    }
}
