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
using System.Reflection;
using System.Reflection.Emit;

namespace JasonSoft
{
    internal delegate object GetHandler(object source);

    

    internal delegate object InstantiateObjectHandler();

    internal static class DynamicMethodCompiler
    {

        // CreateInstantiateObjectDelegate
        public static InstantiateObjectHandler CreateInstantiateObjectHandler(Type type)
        {
            ConstructorInfo constructorInfo =
                type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null,
                                    new Type[0], null);
            if (constructorInfo == null)
            {
                throw new ApplicationException(
                    string.Format(
                        "The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).",
                        type));
            }

            DynamicMethod dynamicMethod =
                new DynamicMethod("InstantiateObject", MethodAttributes.Static | MethodAttributes.Public,
                                  CallingConventions.Standard, typeof (object), null, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Emit(OpCodes.Ret);
            return (InstantiateObjectHandler) dynamicMethod.CreateDelegate(typeof (InstantiateObjectHandler));
        }

        // CreateGetDelegate
        public static GetHandler CreateGetHandler(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Call, getMethodInfo);
            BoxIfNeeded(getMethodInfo.ReturnType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler) dynamicGet.CreateDelegate(typeof (GetHandler));
        }

        // CreateGetDelegate
        public static GetHandler CreateGetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicGet = CreateGetDynamicMethod(type);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            BoxIfNeeded(fieldInfo.FieldType, getGenerator);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler) dynamicGet.CreateDelegate(typeof (GetHandler));
        }

        internal delegate void SetHandler(object source, object value);

        // CreateSetDelegate
        public static SetHandler CreateSetHandler(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(propertyInfo.PropertyType, setGenerator);
            setGenerator.Emit(OpCodes.Callvirt, setMethodInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler) dynamicSet.CreateDelegate(typeof (SetHandler));
        }

        // CreateSetDelegate
        public static SetHandler CreateSetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicSet = CreateSetDynamicMethod(type);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            UnboxIfNeeded(fieldInfo.FieldType, setGenerator);
            setGenerator.Emit(OpCodes.Stfld, fieldInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler) dynamicSet.CreateDelegate(typeof (SetHandler));
        }

        // CreateGetDynamicMethod
        public static DynamicMethod CreateGetDynamicMethod(Type type)
        {
            return new DynamicMethod("DynamicGet", typeof (object), new Type[] {typeof (object)}, type, true);
        }

        // CreateSetDynamicMethod
        public static DynamicMethod CreateSetDynamicMethod(Type type)
        {
            return
                new DynamicMethod("DynamicSet", null, new Type[] {typeof (object), typeof (object)}, type, true);
        }

        // BoxIfNeeded
        private static void BoxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Box, type);
            }
        }

        // UnboxIfNeeded
        private static void UnboxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                generator.Emit(OpCodes.Castclass, type);
            }
        }
    }
}