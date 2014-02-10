using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using JasonSoft;
using JasonSoft.Extension;
using System.Diagnostics;
using JasonSoft.Reflection;
using Xunit;


namespace JasonSoft.Tests.Core
{

    public class ReflectionPerformanceTestCase
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private const int run = 1000000;

        [Fact]
        public void CreateInstanceTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = (SimpleObject)typeof(SimpleObject).CreaetInstance();
            }
            sw.Stop();
            Console.WriteLine("Dynamic Generic:" + sw.Elapsed);


            sw.Reset();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
            }
            sw.Stop();
            Console.WriteLine("Native:" + sw.Elapsed);
        }


        [Fact]
        public void GetFieldTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                String myString = simpleObject.GetField<String>("fieldString");
            }
            sw.Stop();
            Console.WriteLine("Dynamic Generic:" + sw.Elapsed);


            sw.Reset();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                String myString = simpleObject.fieldString;
            }
            sw.Stop();
            Console.WriteLine("Native:" + sw.Elapsed);
        }

        [Fact]
        public void SetFieldTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                simpleObject.SetField("fieldString", "ABC");
            }
            sw.Stop();
            Console.WriteLine("Dynamic Generic:" + sw.Elapsed);


            sw.Reset();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                simpleObject.fieldString = "ABC";
            }
            sw.Stop();
            Console.WriteLine("Native:" + sw.Elapsed);
        }

        [Fact]
        public void GetPropertyTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                String myString = simpleObject.GetProperty<String>("MyString");
            }
            sw.Stop();
            Console.WriteLine("Dynamic Generic:" + sw.Elapsed);


            sw.Reset();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                String myString = simpleObject.MyString;
            }
            sw.Stop();
            Console.WriteLine("Native:" + sw.Elapsed);

        }

        [Fact]
        public void SetPropertyTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                simpleObject.SetProperty("MyString", "JasonLee");
                //Assert.IsTrue(simpleObject.MyString == "JasonLee");
            }
            sw.Stop();
            Console.WriteLine("Dynaymic:" + sw.Elapsed);


            sw.Reset();
            sw.Start();
            for (int i = 0; i < run; i++)
            {
                SimpleObject simpleObject = new SimpleObject();
                simpleObject.MyString = "JasonLee";
                //Assert.IsTrue(simpleObject.MyString == "JasonLee");
            }
            sw.Stop();
            Console.WriteLine("Native:" + sw.Elapsed);
        }




    }
}
