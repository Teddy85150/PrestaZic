using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace PrestaZic
{
    public class PhotoboothController : ApiController
    {
        readonly static Log log = new Log();

        [HttpPost]
        public async Task<IHttpActionResult> Print()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();

                string cheminImage = Path.Combine("C:\\Temp\\PrestaZic\\Photobooth\\ExportImg", filename);

                // Sauvegarder l'image reçue
                File.WriteAllBytes(cheminImage, buffer);

                // Code pour déclencher l'impression de l'image
                log.WriteToFile("Starting printing image " + filename + " in background on printer " + ConfigurationManager.AppSettings["PrinterName"].ToString());

                ImagePrinter imagePrinter = new ImagePrinter(cheminImage);
                _ = Task.Run( () => imagePrinter.PrintImage());

            }

            return Ok("Printing started");
        }

        [HttpGet]
        public IHttpActionResult Shutdown()
        {
            ProcessStartInfo psi = new ProcessStartInfo("shutdown", "/s /f /t 0")
            {
                CreateNoWindow = true, // Ne pas afficher de fenêtre
                UseShellExecute = false // Exécuter directement sans shell
            };
            Process.Start(psi);
            return Ok("Shutdown of computer");
        }
    }
}
