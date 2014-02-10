//===============================================================================
// Microsoft patterns & practices
// Unity Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using JasonSoft.Components.EnterpriseLibrary.Unity.Utility;
using JasonSoft.Components.EnterpriseLibrary.ObjectBuilder2;

namespace JasonSoft.Components.EnterpriseLibrary.Unity.InterceptionExtension
{
    internal static class MethodBaseMethods
    {
        internal static MethodInfo GetMethodFromHandle
        {
            get
            {
                return StaticReflection.GetMethodInfo(
                    () => MethodBase.GetMethodFromHandle(default(RuntimeMethodHandle)));
            }
        }

        internal static MethodInfo GetMethodForGenericFromHandle
        {
            get
            {
                return StaticReflection.GetMethodInfo(
                    () => MethodBase.GetMethodFromHandle(default(RuntimeMethodHandle), default(RuntimeTypeHandle)));
            }
        }
    }
}
