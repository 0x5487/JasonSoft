using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Tests.Container
{
    public class SportCar : Car
    {
        public SportCar() 
        {
        
        }

        public SportCar(HighSpeedEngine engine) : base("SportCar", 2)
        {
    
            Doors = 2;
            MaxSpeed = engine.Speed;
            Engine = engine;
        }

    }
}
