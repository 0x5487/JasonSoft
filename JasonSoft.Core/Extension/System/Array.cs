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
using System.Collections;

namespace JasonSoft.Extension
{
    public static partial class JasonSoftExtensionMethod
    {
        public static bool IsAssignableArrayFrom(this Array source, Array target) 
        {
            if (source == null || target == null)
            {
                throw new ArgumentNullException("source");
            }
            
            if (source == target) 
            {
                return true;
            }
            
            if (source.Length != target.Length) 
            {
                return false;
            }
 
            int i = 0;
            while (i < source.Length) 
            {
                if (source.GetType().IsAssignableFrom(target.GetValue(i).GetType())) 
                {
                    return false;
                }

                i++;
            }            

            return true;        
        }
    }
}
