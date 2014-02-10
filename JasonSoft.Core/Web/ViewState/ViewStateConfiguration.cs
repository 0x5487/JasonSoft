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
using System.Configuration;

namespace JasonSoft.Web.ViewState
{
    internal class ViewStateConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("Mode", DefaultValue = "JasonSoft.Web.ViewState.ViewStateSessionStorage")]
        public String Mode
        {
            get { return (String) this["Mode"]; }
            set { this["Mode"] = value; }
        }


        [ConfigurationProperty("Compression", DefaultValue = false, IsRequired=false)]
        public bool Compression
        {
            get { return (bool) this["Compression"]; }
            set { this["Compression"] = value; }
        }


        [ConfigurationProperty("UniqueIdSource", IsRequired = false,
            DefaultValue = "JasonSoft.Web.ViewState.ViewStateGUID")]
        public String UniqueIDType
        {
            get { return (String) this["UniqueIdSource"]; }
            set { this["UniqueIdSource"] = value; }
        }


        [ConfigurationProperty("defaultDatabase", IsRequired = false)]
        public String defaultDatabase
        {
            get { return (String) this["defaultDatabase"]; }
            set { this["defaultDatabase"] = value; }
        }
    }
}