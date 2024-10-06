using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestaZic
{
    public class ImagePrinter
    {
        private Image _imageToPrint;

        public ImagePrinter(string imagePath)
        {
            _imageToPrint = Image.FromFile(imagePath);
        }

        public void PrintImage()
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(PrintPageHandler);

            // Utiliser un contrôleur d'impression sans boîte de dialogue de statut
            //printDoc.PrintController = new StandardPrintController();

            printDoc.Print();
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            // Adjust the image size and position as necessary
            e.Graphics.DrawImage(_imageToPrint, e.MarginBounds);
        }
    }
}
