using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using JasonSoft.Components.Unity;
using JasonSoft.Components.Membership;

namespace JasonSoft.Tests.Container
{
    public class ContainerConfig : IConfigSource
    {
        public ContainerConfig() 
        {
            
        }

        public void Load(IUnityContainer container)
        {
            container.RegisterType<IEngine, HighSpeedEngine>(new ContainerControlledLifetimeManager());
            container.RegisterType<LowSpeedEngine>(null);

            container.RegisterType<ICar, Car>("Toyota").Configure<InjectedMembers>()
                .ConfigureInjectionFor<Car>("Toyota", new InjectionConstructor("Toyota", 5));

            container.RegisterType<ICar, Car>().Configure<InjectedMembers>()
                .ConfigureInjectionFor<Car>(new InjectionConstructor(typeof (IEngine), "Honda"));

            container.RegisterType<SportCar>(null);

            container.RegisterType<IDbConnection, SqlConnection>().Configure<InjectedMembers>()
                .ConfigureInjectionFor<SqlConnection>(
                new InjectionConstructor(ConfigurationManager.ConnectionStrings["ServiceDB"].ToString()));

            //container.RegisterType<IUserProfile, UserProfile>(new ContainerControlledLifetimeManager());

            //container.RegisterObject<LowSpeedEngine>(new RegisteredForm() { Scope= InstanceScope.Singleton });
            //container.RegisterObject<IEngine, HighSpeedEngine>(new RegisteredForm() { Key = "HighSpeedEngine" });

            //container.RegisterObject<ICar, Car>(new RegisteredForm()
            //{
            //    Key = "Car",     
            //    Scope = InstanceScope.New,
            //    Parameters = { new Parameter() { Name = "name", Value = "Honda Accord" }, new Parameter() { Name = "seats", Value = 5 } }
            //});

            //container.RegisterObject<SportCar>();

            //container.RegisterObject<IDbConnection, SqlConnection>(new RegisteredForm()
            //{
            //    Key = "Connection",
            //    Scope = InstanceScope.New,
            //    Parameters = { new Parameter() { Name = "connectionString", Value = ConfigurationManager.ConnectionStrings["ServiceDB"].ToString() } }
            //});

            //container.RegisterObject<IUserProfile, UserProfile>();
        }



                    
        
    }
}
