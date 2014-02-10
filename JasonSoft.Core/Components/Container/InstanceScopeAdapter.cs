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
using JasonSoft.Components.Castle.Core;


namespace JasonSoft.Components.Container
{
    internal class InstanceScopeAdapter
    {
        public InstanceScopeAdapter(InstanceScope scope)
        {
            switch(scope)
            {
                case InstanceScope.New:
                    _type = LifestyleType.Transient;
                    break;
                case InstanceScope.Singleton:
                    _type = LifestyleType.Singleton;
                    break;
                case InstanceScope.WebRequest:
                    _type = LifestyleType.PerWebRequest;
                    break;
            }
        }

        private LifestyleType _type;

        public LifestyleType LifestyleType
        {
            get { return _type; }
        }
    }
}
