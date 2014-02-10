using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using JasonSoft.Components.Container;
using JasonSoft.Components.Membership;
using JasonSoft.Components.Unity;
using NUnit.Framework;


namespace JasonSoft.Tests.Container
{
    [TestFixture]
    public class UnityContainerTestCase
    {

        public UnityContainerTestCase() 
        {
            
            
        }

        [Test]
        public void RegisterObjectsTest()
        {
            IUnityContainer container = new UnityContainer();
            container.Load(new ContainerConfig());
            
            IEngine highSpeedEngine = container.Resolve<HighSpeedEngine>();
            Assert.IsTrue(highSpeedEngine is HighSpeedEngine);

            IEngine lowSpeedEngine = container.Resolve<LowSpeedEngine>();
            Assert.IsTrue(lowSpeedEngine is LowSpeedEngine);

            ICar honda = container.Resolve<ICar>();
            Assert.AreEqual("Honda", honda.Name);

            Car toyota = container.Resolve<Car>("Toyota");
            Assert.AreEqual("Toyota", toyota.Name);
            Assert.AreEqual(5, toyota.Seats);

            SportCar prelude = container.Resolve<SportCar>();
            Assert.IsTrue(prelude is SportCar);

            IDbConnection connection = container.Resolve<IDbConnection>();
            connection.Open();
            connection.Close();
        }


        [Test]
        public void InstanceScopeTest()
        {
            IUnityContainer container = new UnityContainer();
            container.Load(new ContainerConfig());

            //new
            SportCar prelude01 = container.Resolve<SportCar>();
            SportCar prelude02 = container.Resolve<SportCar>();

            Assert.IsTrue(prelude01 != prelude02);
            Assert.IsTrue(prelude01.GetHashCode() != prelude02.GetHashCode());
            Assert.IsFalse(prelude01.Equals(prelude02));

            //singleton
            HighSpeedEngine highSpeedEngine1 = container.Resolve<HighSpeedEngine>();
            HighSpeedEngine highSpeedEngine2 = container.Resolve<HighSpeedEngine>();

            Assert.IsTrue(highSpeedEngine1 == highSpeedEngine2);
            Assert.IsTrue(highSpeedEngine1.GetHashCode() == highSpeedEngine2.GetHashCode());
            Assert.IsTrue(highSpeedEngine1.Equals(highSpeedEngine2));

        }

        [Test]
        public void RegisterInstanceTest()
        {
            //IUnityContainer container = new UnityContainer();
            //container.Load(new ContainerConfig());

            //IUserProfile userProfile = container.Resolve<IUserProfile>();
            //Assert.IsTrue(userProfile is UserProfile);

            //AdminProfile adminProfile = new AdminProfile();
            //userProfile = adminProfile;
            //container.RegisterInstance<IUserProfile>(adminProfile);

            //userProfile = container.Resolve<IUserProfile>();
            //Assert.IsTrue(userProfile is AdminProfile);

          
        }

        //[Test]
        //public void InjectInstanceTest() 
        //{
        //    IObjectContainer container = new ObjectContainer(new ContainerConfig());

        //    SportCar sportCar = new SportCar();

        //    container.InjectInstanceProperty(sportCar);

        //    Assert.IsNotNull(sportCar.Engine);         
        
        //}

        [Test]
        public void UnitySimpleTest()
        {
            IUnityContainer unityContainer = new UnityContainer();

            //unityContainer.RegisterType<ICar, Car>("name").Configure<InjectedMembers>()
            //    .ConfigureInjectionFor<Car>(new InjectionConstructor("honda", 5));

            unityContainer.RegisterType<IEngine, HighSpeedEngine>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<ICar, Car>(new ContainerControlledLifetimeManager())
                .Configure<InjectedMembers>()
                .ConfigureInjectionFor<Car>(new InjectionConstructor(typeof(IEngine), "Honda"));

            unityContainer.RegisterType<ICar, Car>().Configure<InjectedMembers>()
                .ConfigureInjectionFor<Car>(new InjectionConstructor("Toyota", 5));

            ICar car = unityContainer.Resolve<ICar>();





        }
    }
}
