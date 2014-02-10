using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
//using Aspose.Slides;
using Xunit;

namespace JasonSoft.Tests.MSOffice
{
    public class PowerPointTestCase
    {
        [Fact]
        public void ExportPPTToJPG()
        {
            //ApplicationClass pptApplication = new ApplicationClass();
            //Presentation pptPresentation = pptApplication.Presentations.Open(@"C:\Temp\office.ppt", MsoTriState.msoFalse,
            //MsoTriState.msoFalse, MsoTriState.msoFalse);

            //Console.WriteLine(pptApplication.Version);

            //pptPresentation.SaveAs(@"c:\temp\powerpoint\a.png", PpSaveAsFileType.ppSaveAsPNG, MsoTriState.msoFalse);
            ////pptPresentation.Slides[1].Export(@"c:\temp\slide.png", "PNG", 1024, 768);
            //pptPresentation.Close();

            //Instantiate a Presentation object that represents a PPT file
            //Presentation pres = new Presentation(@"C:\Temp\office.ppt");

            //Accessing a slide using its slide position
            //Slide slide = pres.GetSlideByPosition(1);

            //Getting the thumbnail image of the slide of a specified size
            //Image image = slide.GetThumbnail(new Size(1024, 768));

            //Saving the thumbnail image in jpeg format
            //image.Save(@"c:\temp\slide-1.jpg", ImageFormat.Png);
        }
    }
}
