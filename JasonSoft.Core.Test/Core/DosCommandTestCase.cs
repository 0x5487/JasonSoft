using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonSoft;
using Xunit;

namespace JasonSoft.Tests.Core
{
    public class DosCommandTestCase
    {
        [Fact]
        public void DosCommandTest()
        {
            DosCommand command = new DosCommand("ipconfig", "/all");
            Boolean result = command.Start();

            Assert.True(result);
            Assert.True(command.OutputMessage.IsNullOrEmpty() == false);
        }
    }
}
