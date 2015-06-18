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
using System.Configuration;

namespace JasonSoft.Web.ViewState
{
    public interface IViewstateStorage
    {
        void Add(KeyValuePair<object, string> viewstateItem);

        KeyValuePair<object, string> Get(object key);

        void Remove(object key);
    }

    public static class ViewstateStorageFactory
    {
        public static IViewstateStorage GetStorageMedium()
        {
            ViewStateConfiguration config =
                (ViewStateConfiguration) ConfigurationManager.GetSection("ViewStateConfiguration");
            IViewstateStorage _retval = GetStorageMedium(config.Mode);
            if (_retval == null)
            {
                return new ViewStateSessionStorage();
            }
            else
                return _retval;
        }

        public static IViewstateStorage GetStorageMedium(string type)
        {
            Type Itype = Type.GetType(type);
            return ((IViewstateStorage) Activator.CreateInstance(Itype));
        }


        public static IViewStateUniqueID GetUserUniqueID()
        {
            ViewStateConfiguration config =
                (ViewStateConfiguration) ConfigurationManager.GetSection("ViewStateConfiguration");
            Type Itype = Type.GetType(config.UniqueIDType);
            IViewStateUniqueID _retval = ((IViewStateUniqueID) Activator.CreateInstance(Itype));
            return _retval;
        }
    }
}