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
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Container
{
    public interface IObjectContainer
    {
        void RegisterInstance<TimplementType>(Object instance);
        void RegisterInstance(String key, Object instance);

        void RegisterObject<TimplementType>();
        void RegisterObject<TimplementType>(RegisteredForm form);
        void RegisterObject<TServiceType, TimplementType>() where TimplementType : TServiceType;
        void RegisterObject<TServiceType, TimplementType>(RegisteredForm form) where TimplementType : TServiceType;
        

        Object GetInstance(String key);
        Object GetInstance(Type source);
        TReturnType GetInstance<TReturnType>();
        //TReturnType GetInstance<TReturnType>(String key);

        void InjectInstanceProperty(Object instance);

        bool RemoveObject(String key);
        //bool RemoveObjects<T>();

        bool HasObject(String key);
        bool HasObject(Type source);
        bool HasObject<T>();

        void Dispose();
    }
}
