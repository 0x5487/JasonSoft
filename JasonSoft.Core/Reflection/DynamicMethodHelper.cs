using System;
using System.Reflection;
using System.Reflection.Emit;

namespace JasonSoft.Reflection
{
    /// <summary>Delegate for calling a method that is not known at runtime.</summary>
    /// <param name="target">the object to be called or null if the call is to a static method.</param>
    /// <param name="parameters">the parameters to the method</param>
    /// <returns>the return value for the method or null if it doesn't return anything.</returns>
    internal delegate object FastInvokeHandler(object target, object[] parameters);

    /// <summary>Delegate for creating and object at runtime using the default constructor.</summary>
    /// <returns>the newly created object.</returns>
    internal delegate object FastInstanceHandler();

    internal delegate object FastInstanceWithParameterHandler(Object[] parameters);

    /// <summary>Delegate to get an arbitraty property at runtime.</summary>
    /// <param name="target">the object instance whose property will be obtained.</param>
    /// <returns>the property value.</returns>
    internal delegate object FastGetHandler(object target);


    /// <summary>Delegate to set an arbitrary property at runtime.</summary>
    /// <param name="target">the object instance whose property will be modified.</param>
    /// <param name="parameter"></param>
    internal delegate void FastSetHandler(object target, object parameter);

    /// <summary>Class with helper methods for dynamic invocation generating IL on the fly.</summary>
    internal static class DynamicMethodHelper
    {
        internal static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            // generates a dynamic method to generate a FastInvokeHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            ParameterInfo[] parameters = methodInfo.GetParameters();

            Type[] paramTypes = new Type[parameters.Length];

            // copies the parameter types to an array
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                    paramTypes[i] = parameters[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = parameters[i].ParameterType;
            }

            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            // generates a local variable for each parameter
            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = ilGenerator.DeclareLocal(paramTypes[i], true);
            }

            // creates code to copy the parameters to the local variables
            for (int i = 0; i < paramTypes.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                EmitFastInt(ilGenerator, i);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(ilGenerator, paramTypes[i]);
                ilGenerator.Emit(OpCodes.Stloc, locals[i]);
            }

            if (!methodInfo.IsStatic)
            {
                // loads the object into the stack
                ilGenerator.Emit(OpCodes.Ldarg_0);
            }

            // loads the parameters copied to the local variables into the stack
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                    ilGenerator.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    ilGenerator.Emit(OpCodes.Ldloc, locals[i]);
            }

            // calls the method
            if (!methodInfo.IsStatic)
            {
                ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
            }
            else
            {
                ilGenerator.EmitCall(OpCodes.Call, methodInfo, null);
            }

            // creates code for handling the return value
            if (methodInfo.ReturnType == typeof(void))
            {
                ilGenerator.Emit(OpCodes.Ldnull);
            }
            else
            {
                EmitBoxIfNeeded(ilGenerator, methodInfo.ReturnType);
            }

            // iterates through the parameters updating the parameters passed by ref
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                {
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(ilGenerator, i);
                    ilGenerator.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        ilGenerator.Emit(OpCodes.Box, locals[i].LocalType);
                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                }
            }

            // returns the value to the caller
            ilGenerator.Emit(OpCodes.Ret);

            // converts the DynamicMethod to a FastInvokeHandler delegate to call to the method
            FastInvokeHandler invoker = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));

            return invoker;
        }

        /// <summary>Gets the instance creator delegate that can be use to create instances of the specified type.</summary>
        /// <param name="type">The type of the objects we want to create.</param>
        /// <returns>A delegate that can be used to create the objects.</returns>
        internal static FastInstanceHandler CreateInstanceHandler(Type type)
        {
            // generates a dynamic method to generate a FastCreateInstanceHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, type, new Type[0],true);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            // generates code to create a new object of the specified type using the default constructor
            ilGenerator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));

            // returns the value to the caller
            ilGenerator.Emit(OpCodes.Ret);

            // converts the DynamicMethod to a FastCreateInstanceHandler delegate to create the object
            FastInstanceHandler creator = (FastInstanceHandler)dynamicMethod.CreateDelegate(typeof(FastInstanceHandler));

            return creator;
        }

        internal static FastInstanceWithParameterHandler CreateInstanceWithParameterHandler(Type source, Type[] parameter)
        {
            // generates a dynamic method to generate a FastCreateInstanceHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, source, parameter, typeof(DynamicMethodHelper).Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            // generates code to create a new object of the specified type using the default constructor
            ilGenerator.Emit(OpCodes.Newobj, source.GetConstructor(parameter));

            // returns the value to the caller
            ilGenerator.Emit(OpCodes.Ret);

            // converts the DynamicMethod to a FastCreateInstanceHandler delegate to create the object
            FastInstanceWithParameterHandler creator = (FastInstanceWithParameterHandler)dynamicMethod.CreateDelegate(typeof(FastInstanceWithParameterHandler));

            return creator;
        }

        internal static FastGetHandler CreateFastGetHandler(FieldInfo fieldInfo)
        {
            // generates a dynamic method to generate a FastGetHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, fieldInfo.DeclaringType.Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            // loads the object into the stack
            ilGenerator.Emit(OpCodes.Ldarg_0);

            // calls the getter
            ilGenerator.Emit(OpCodes.Ldfld, fieldInfo);

            // creates code for handling the return value
            EmitBoxIfNeeded(ilGenerator, fieldInfo.FieldType);

            // returns the value to the caller
            ilGenerator.Emit(OpCodes.Ret);

            // converts the DynamicMethod to a FastGetHandler delegate to get the property
            FastGetHandler getter = (FastGetHandler)dynamicMethod.CreateDelegate(typeof(FastGetHandler));

            return getter;
        }

        internal static FastGetHandler CreateFastGetHandler(PropertyInfo propInfo)
        {
            // generates a dynamic method to generate a FastGetHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, propInfo.DeclaringType.Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            // loads the object into the stack
            ilGenerator.Emit(OpCodes.Ldarg_0);

            // calls the getter
            ilGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetGetMethod(), null);

            // creates code for handling the return value
            EmitBoxIfNeeded(ilGenerator, propInfo.PropertyType);

            // returns the value to the caller
            ilGenerator.Emit(OpCodes.Ret);

            // converts the DynamicMethod to a FastGetHandler delegate to get the property
            FastGetHandler getter = (FastGetHandler)dynamicMethod.CreateDelegate(typeof(FastGetHandler));

            return getter;
        }

        internal static FastSetHandler CreateFastSetHandler(PropertyInfo propInfo)
        {
            // generates a dynamic method to generate a FastSetHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[] { typeof(object), typeof(object) }, propInfo.DeclaringType.Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            // loads the object into the stack
            ilGenerator.Emit(OpCodes.Ldarg_0);

            // loads the parameter from the stack
            ilGenerator.Emit(OpCodes.Ldarg_1);

            // cast to the proper type (unboxing if needed)
            EmitCastToReference(ilGenerator, propInfo.PropertyType);

            // calls the setter
            ilGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetSetMethod(true), null);

            // terminates the call
            ilGenerator.Emit(OpCodes.Ret);

            // converts the DynamicMethod to a FastGetHandler delegate to get the property
            FastSetHandler setter = (FastSetHandler)dynamicMethod.CreateDelegate(typeof(FastSetHandler));

            return setter;
        }

        internal static FastSetHandler CreateFastSetHandler(FieldInfo fieldInfo)
        {
            // generates a dynamic method to generate a FastSetHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[] { typeof(object), typeof(object) }, fieldInfo.DeclaringType.Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            // loads the object into the stack
            ilGenerator.Emit(OpCodes.Ldarg_0);

            // loads the parameter from the stack
            ilGenerator.Emit(OpCodes.Ldarg_1);

            // cast to the proper type (unboxing if needed)
            EmitCastToReference(ilGenerator, fieldInfo.FieldType);

            // calls the setter
            ilGenerator.Emit(OpCodes.Stfld, fieldInfo);

            // terminates the call
            ilGenerator.Emit(OpCodes.Ret);

            // converts the DynamicMethod to a FastGetHandler delegate to get the property
            FastSetHandler setter = (FastSetHandler)dynamicMethod.CreateDelegate(typeof(FastSetHandler));

            return setter;
        }

        /// <summary>Emits the cast to a reference, unboxing if needed.</summary>
        /// <param name="ilGenerator">The MSIL generator.</param>
        /// <param name="type">The type to cast.</param>
        private static void EmitCastToReference(ILGenerator ilGenerator, Type type)
        {
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Castclass, type);
            }
        }

        /// <summary>Boxes a type if needed.</summary>
        /// <param name="ilGenerator">The MSIL generator.</param>
        /// <param name="type">The type.</param>
        private static void EmitBoxIfNeeded(ILGenerator ilGenerator, Type type)
        {
            if (type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, type);
            }
        }

        /// <summary>Emits code to save an integer to the evaluation stack.</summary>
        /// <param name="ilGenerator">The MSIL generator.</param>
        /// <param name="value">The value to push.</param>
        private static void EmitFastInt(ILGenerator ilGenerator, int value)
        {
            // for small integers, emit the proper opcode
            switch (value)
            {
                case -1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    ilGenerator.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    ilGenerator.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    ilGenerator.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    ilGenerator.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    ilGenerator.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    ilGenerator.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    ilGenerator.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    ilGenerator.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            // for bigger values emit the short or long opcode
            if (value > -129 && value < 128)
            {
                ilGenerator.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
}
