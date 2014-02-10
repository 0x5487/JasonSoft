using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using JasonSoft.Validation;

namespace JasonSoft.Tests.Validation
{
    public class Validate
    {
        [Fact]
        public void IsDateTimeTest()
        {
            Assert.True("2009/12/02 12:00:00".IsDateTime());
        }
    }
}
