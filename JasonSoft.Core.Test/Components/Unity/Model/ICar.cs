using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Tests.Container
{
    public interface ICar
    {
        String Name { get; set; }
        int Doors { get; set; }
        int MaxSpeed { get; set; }
        int Seats { get; set; }

        void StartEngine();


    }
}
