// Copyright 2008 JasonSoft - http://www.jasonsoft.net/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using JasonSoft.Components.Castle.MicroKernel;
using JasonSoft;
using JasonSoft.Reflection;

namespace JasonSoft.Components.Container
{
    public class ObjectContainer : IObjectContainer
    {
        private IKernel _kernel;

        protected ObjectContainer() 
        {
            _kernel = new DefaultKernel();
        }

        public ObjectContainer(params IConfigSource[] configSources) : this()
        {
            if(configSources.Length <= 0) throw new ArgumentNullException("configSources");

            foreach (IConfigSource source in configSources) source.Load(this);
        }



        public void RegisterInstance<TimplementType>(Object instance)
        {
            _kernel.AddComponentInstance<TimplementType>(instance);
        }

        public void RegisterInstance(string key, object instance)
        {
            _kernel.AddComponentInstance(key, instance);
        }

        public void RegisterObject<TimplementType>()
        {
            RegisterObject<TimplementType>(new RegisteredForm());
        }

        public void RegisterObject<TServiceType, TimplementType>() where TimplementType : TServiceType
        {
            Type serviceType = typeof(TServiceType);

            RegisteredForm form = new RegisteredForm();
            form.Services.Add(serviceType);

            RegisterObject<TimplementType>(form);        
        }

        public void RegisterObject<TServiceType, TimplementType>(RegisteredForm form) where TimplementType : TServiceType
        {
            Type serviceType = typeof(TServiceType);

            form.Services.Add(serviceType);

            RegisterObject<TimplementType>(form);
        }

        public void RegisterObject<T>(RegisteredForm form) 
        {
            if(form == null) throw new ArgumentNullException("form");

            Type sourceType = typeof (T);
            String key = null;

            if (!form.Key.IsNullOrEmpty()) key = form.Key;

            if(form.Services.IsNullOrEmpty())
            {
                if (key.IsNullOrEmpty()) key = sourceType.FullName;
                _kernel.AddComponent(key, sourceType, new InstanceScopeAdapter(form.Scope).LifestyleType);
            }
            else
            {
                foreach (Type service in form.Services)
                {
                    if(key.IsNullOrEmpty()) key = service.FullName;
                    _kernel.AddComponent(key, service, sourceType, new InstanceScopeAdapter(form.Scope).LifestyleType);
                    
                }                
            }

            if (!form.Parameters.IsNullOrEmpty()) 
            {
                Hashtable hash = new Hashtable();
                
                foreach (Parameter parameter in form.Parameters) 
                {
                    hash.Add(parameter.Name, parameter.Value);    
                }

                _kernel.RegisterCustomDependencies(key, hash);            
            }
        }



        public Object GetInstance(String key)
        {
            if (key.IsNullOrEmpty()) throw new ArgumentNullException("key");

            if (HasObject(key)) 
            {
                IHandler handler = _kernel.GetHandler(key);
                return _kernel.Resolve(key, handler.ComponentModel.Service);            
            }
            
            return null;
        }

        public Object GetInstance(Type source) 
        {
            return _kernel.Resolve(source);
        }

        public TReturnType GetInstance<TReturnType>()
        {
            return _kernel.Resolve<TReturnType>();
            
        }



        public void InjectInstanceProperty(Object instance)
        {
            Type sourceType = instance.GetType();

            PropertyInfo[] properties = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (!properties.IsNullOrEmpty())
            {
                foreach (PropertyInfo property in properties)
                {
                    Type propertyType = property.PropertyType;

                    if(propertyType.IsPrimitive == false && propertyType != typeof(String) && property.CanWrite)
                    {
                        if(HasObject(propertyType))
                        {  
                            instance.SetProperty(property.Name, GetInstance(propertyType));                            
                        }
                    }
                }

            }
        }

        public bool RemoveObject(String key)
        {
            return _kernel.RemoveComponent(key);
        }


        public bool HasObject(String key)
        {
            if (key.IsNullOrEmpty()) throw new ArgumentNullException("key");
            return _kernel.HasComponent(key);
        }

        public bool HasObject(Type source)
        {
            return _kernel.HasComponent(source);
        }

        public bool HasObject<T>()
        {
            Type sourceType = typeof(T);
            return HasObject(sourceType);
        }

        public void Dispose() 
        {
            _kernel.Dispose();
        }

    }
}
