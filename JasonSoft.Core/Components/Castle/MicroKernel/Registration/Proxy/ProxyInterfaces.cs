// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
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
using JasonSoft.Components.Castle.Core;
using JasonSoft.Components.Castle.MicroKernel.Proxy;

namespace JasonSoft.Components.Castle.MicroKernel.Registration.Proxy
{
    public class ProxyInterfaces<S, T> : ComponentDescriptor<S, T>
    {
        private readonly Type[] interfaces;

        public ProxyInterfaces(Type[] interfaces)
        {
            this.interfaces = interfaces;
        }

        protected internal override void ApplyToModel(ComponentModel model)
        {
            if (interfaces.Length > 0)
            {
                ProxyOptions options = ProxyUtil.ObtainProxyOptions(model, true);
                options.AddAdditionalInterfaces(interfaces);
            }
        }
    }
}