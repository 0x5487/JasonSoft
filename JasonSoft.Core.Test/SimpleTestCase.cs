using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using JasonSoft.Components.EnterpriseLibrary.Data.Sql;
using JasonSoft.Components.Logging;
using JasonSoft.Components.SmartThreadPool;
using JasonSoft.Math;
using JasonSoft;
using JasonSoft.Net;
using JasonSoft.IO;
using JasonSoft.Web;
using JasonSoft.XML;
using JasonSoft.Web.Security;

using Xunit;



namespace JasonSoft.Tests
{

    public class SimpleTestCase
    {




        [Fact]
        public void Test()
        {

            string file = "abcd";
            string result = Path.GetFileName(file);

            string reuslt = Path.GetExtension(file);




        }

    }



}
