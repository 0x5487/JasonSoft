using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Tests.Container
{
    public class Car: ICar
    {
        public Car() 
        {
        
        }
        
        public Car(LowSpeedEngine engine) 
        {
            this.Name = "FamilyCar";
            this.Doors = 4;
            this.MaxSpeed = engine.Speed;
            this.Seats = 4;
        }

        public Car(String name, int seats)
        {
            this.Name = name;
            this.Doors = 4;
            this.Seats = seats;
        }

        public Car(IEngine engine, String Name)
        {
            this.Engine = engine;
            this.Name = Name;
        }


        public String Name { get; set; }
        public int Doors { get; set; }
        public int MaxSpeed { get; set; }
        public int Seats { get; set; }
        public Color Color { get; set; }
        public IEngine Engine { get; set; }

        public void StartEngine() { }


    }
}
