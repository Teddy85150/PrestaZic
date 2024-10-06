using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RawPrint.NetStd;

namespace PrestaZic
{
    public class ImagePrinter
    {
        private Image _imageToPrint;
        private string imagePath;
        private string fileName;

        public ImagePrinter(string paramImagePath)
        {
            imagePath = paramImagePath;
            fileName = Path.GetFileName(imagePath);
            _imageToPrint = Image.FromFile(paramImagePath);
        }

        public void PrintImage()
        {
            IPrinter printer = new Printer();
            printer.PrintRawFile(ConfigurationManager.AppSettings["PrinterName"].ToString(), imagePath, fileName);

            /**
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(PrintPageHandler);

            // Utiliser un contrôleur d'impression sans boîte de dialogue de statut
            //printDoc.PrintController = new StandardPrintController();

            printDoc.Print();
            **/
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            // Adjust the image size and position as necessary
            e.Graphics.DrawImage(_imageToPrint, e.MarginBounds);
        }
    }
}
