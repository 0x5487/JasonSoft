using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using JasonSoft.Drawing;
using Xunit;


namespace JasonSoft.Tests.Drawing
{

    public class DrawingExtensionTest
    {
        [Fact]
        public void ImageResize()
        {
            FileInfo file = new FileInfo(@"C:\Temp\before.jpg");
            Image oldImage = Image.FromFile(file.FullName);
            oldImage.Resize(@"C:\Temp\after.jpg", 75L);
            oldImage.Resize(@"C:\Temp\after_head.jpg", new Size(100, 75), 100L, ContentAlignment.MiddleCenter, ThumbMode.Max);
            //Image bitmap = new Bitmap(File.OpenRead(@"C:\Temp\all_2.jpg"));
            

            //Stream stream = oldImage.Resize("", new Size(1024, 765), 100, ContentAlignment.MiddleCenter, ThumbMode.Full);
            
            //oldImage.Resize(@"C:\Temp\wrong_size.jpg", 100, ContentAlignment.MiddleCenter, ThumbMode.Max);

        }

        public void ResizeAll()
        {
            DirectoryInfo directory = new DirectoryInfo(@"");
            var files = directory.GetFiles("*.jpg", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var image = Image.FromFile(file.FullName);
                
            }
        }

    }
}
