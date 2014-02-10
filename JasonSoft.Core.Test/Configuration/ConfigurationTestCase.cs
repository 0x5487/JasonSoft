using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using JasonSoft.Configuration;
using Xunit;


namespace JasonSoft.Tests.Configuration
{

    public class ConfigurationTestCase
    {
        [Fact]
        public void UpdateConnectionString()
        {
            FileInfo configFile = new FileInfo(@"C:\Program Files\Test\UGuardWebSetup\Web.Config");
            ConfigurationHelper helper = new ConfigurationHelper(configFile);
            helper.ChangeConfigurationString("UGuardDB", "aa", "bb");
        }
    }
}
