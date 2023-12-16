using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;



namespace ExPaper.SharedMethods.Lib.Converter;

public class ImageToPdfConverter
{
    public static void ConvertToPdf(IFormFile imageFile)
    {
        ////Create a new PDF document
        //PdfDocument doc = new PdfDocument();
        ////Add a page to the document
        //PdfPage page = doc.Pages.Add();
        ////Get page size to draw image which fits the page
        //SizeF pageSize = page.GetClientSize();
        ////Create PDF graphics for the page
        //PdfGraphics graphics = page.Graphics;
        ////Load the image from the disk
        //PdfBitmap image = new PdfBitmap("Autumn Leaves.jpg");
        ////Draw the image
        //graphics.DrawImage(image, new RectangleF(0, 0, pageSize.Width, pageSize.Height));
        ////Save the document
        //doc.Save("OutputImage.pdf");
        ////Close the document
        //doc.Close(true);
        ////This will open the PDF file so, the result will be seen in default PDF viewer
        //Process.Start("OutputImage.pdf");

    }

}
