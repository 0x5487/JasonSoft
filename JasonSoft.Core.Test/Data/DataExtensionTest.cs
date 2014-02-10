using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using JasonSoft.Data;
using Xunit;

namespace JasonSoft.Tests
{
    public class DataExtensionTest
    {

        [Fact]
        public void CVSToDataTableTest()
        {
            String cvsPath = @"D:\JasonSoft\Project\JasonSoft\JasonSoft.Core.Test\Resource\Example1.csv";
            DataTable table = cvsPath.CSVToDataTable(false);

            

        }
    }
}
