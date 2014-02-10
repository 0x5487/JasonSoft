using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Tests.Container
{
    public class HighSpeedEngine : IEngine
    {
        public HighSpeedEngine() 
        {
            this.Speed = 500;
        }

        public int Speed { get; set; }
    }
}
