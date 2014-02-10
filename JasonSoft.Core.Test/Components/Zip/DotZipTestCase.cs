using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using JasonSoft.Components.Zip;
using JasonSoft.Drawing;
using Xunit;

namespace JasonSoft.Tests.Components.Zip
{

    public class DotZipTestCase
    {
        [Fact]
        public void MakeZip()
        {
              try
               {
                 using (ZipFile zip = new ZipFile())
                 {
                   // add this map file into the "images" directory in the zip archive
                   zip.AddFile("c:\\images\\personal\\7440-N49th.png", "images");
                   // add the report into a different directory in the archive
                   zip.AddFile("c:\\Reports\\2008-Regional-Sales-Report.pdf", "files");
                   zip.AddFile("ReadMe.txt");
                   zip.Save("MyZipFile.zip");
                     
                 }
               }
               catch (System.Exception ex1)
               {
                 System.Console.Error.WriteLine("exception: " + ex1);
               }

        }

        [Fact]
        public void SelfExtracting()
        {
            String target = @"D:\JasonSoft\Tool";

            using(ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(target, "test");

                SelfExtractorSaveOptions sfxOptions = new SelfExtractorSaveOptions();

                sfxOptions.Flavor = SelfExtractorFlavor.WinFormsApplication;

                sfxOptions.ExtractExistingFile = ExtractExistingFileAction.Throw;
                
                //sfxOptions.IconFile = "client_vdb.ico";

                sfxOptions.Quiet = false; // hides the window after startup

                //sfxOptions.DefaultExtractDirectory = @"c:\";

                zipFile.SaveSelfExtractor("abc.exe", sfxOptions);
            }
        }


        [Fact]
        public void ExtractFromInputStreamTest()
        {
            FileInfo tempFile = new FileInfo(@"C:\Temp\Temp.zip");

            if (tempFile.Exists)
            {
                using (ZipFile zipFile = ZipFile.Read(tempFile.OpenRead()))
                {
                    foreach (ZipEntry zipEntry in zipFile)
                    {
                        Boolean answer = Path.HasExtension(zipEntry.FileName);

                        if (answer == true)
                        {
                            String extension = Path.GetExtension(zipEntry.FileName).ToLower();

                            if(extension == ".jpg" || extension == ".png")
                            {
                                MemoryStream ms = new MemoryStream();

                                try
                                {
                                    zipEntry.Extract(ms);
                                }
                                catch (BadPasswordException passwordEx)
                                {
                                    Console.WriteLine("need password");
                                }

                                if (ms.Length > 410000)
                                {
                                    Image image = new Bitmap(ms);
                                    //image.Resize(@"c:\temp\" + Guid.NewGuid().ToString("N") + ".jpg");
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
