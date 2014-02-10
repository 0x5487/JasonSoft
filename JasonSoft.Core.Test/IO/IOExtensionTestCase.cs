using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JasonSoft.IO;
using Xunit;


namespace JasonSoft.Tests.IO
{

    public class IOExtensionTestCase
    {
        [Fact]
        public void SaveToFileTest()
        {
            String abc = "abc";
            abc.WriteToFile(@"c:\abc\abc.txt");
        }

        [Fact]
        public void Temp()
        {
            FileInfo aa = new FileInfo(@"c:\jason\angle\readme.txt");
            //aa.Directory.Create();
            aa.Create();
        }

        [Fact]
        public void IsSubDirectoryOfTest()
        {
            DirectoryInfo temp = new DirectoryInfo(@"C:\Temp\Web");
            DirectoryInfo backup = new DirectoryInfo(@"C:\");
            Assert.True(backup.IsSubDirectoryOf(temp));
        }

        [Fact]
        public void GetDirSizeTest()
        {
            DirectoryInfo temp = new DirectoryInfo(@"C:\WINDOWS");
            Int32 fileCount = 0;
            Console.WriteLine(temp.DirSize(ref fileCount) /1024 /1024);
            Console.WriteLine(String.Format("FileCount: {0}", fileCount));
        }

        [Fact]
        public void RenameFile()
        {
            String path = @"c:\temp\setup.txt";
            FileInfo file = new FileInfo(path);
            Assert.True(file.Exists);

            FileInfo newFile = file.Rename("abc.txt");
            Assert.False(file.Exists);
            Assert.True(newFile.Exists);
        }
    }
}
