//// Copyright 2008 JasonSoft - http://www.jasonsoft.net/
//// 
//// Licensed under the Apache License, Version 2.0 (the "License");
//// you may not use this file except in compliance with the License.
//// You may obtain a copy of the License at
//// 
////     http://www.apache.org/licenses/LICENSE-2.0
//// 
//// Unless required by applicable law or agreed to in writing, software
//// distributed under the License is distributed on an "AS IS" BASIS,
//// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//// See the License for the specific language governing permissions and
//// limitations under the License.




//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Script.Serialization;
//using System.Reflection;
//using System.IO;
//using System.Xml.Serialization;

//namespace JasonSoft.Extension
//{
//    public static partial class JasonSoftExtensionMethod
//    {
//        public static string ToJSON(this object target)
//        {
//            JavaScriptSerializer serializer = new JavaScriptSerializer();
//            return serializer.Serialize(target);
//        }



//        public static T GetField<T>(this object target, string fieldName) 
//        {
//            if (fieldName.IsNullOrEmpty()) 
//            {
//                throw new ArgumentNullException("fieldName of parameter can't be null");
//            }

//            return (T)target.GetType().InvokeMember(fieldName,
//               BindingFlags.Static |
//               BindingFlags.Public |
//               BindingFlags.NonPublic |
//               BindingFlags.Instance |
//               BindingFlags.GetField, null, target, null);
//        }

//        public static void SetField(this object target, string fieldName, object value) 
//        {
//            if (fieldName.IsNullOrEmpty())
//            {
//                throw new ArgumentNullException("fieldName of parameter can't be null");
//            }

//            target.GetType().InvokeMember(fieldName,
//                           BindingFlags.Static |
//                           BindingFlags.Public |
//                           BindingFlags.NonPublic |
//                           BindingFlags.Instance |
//                           BindingFlags.SetField, null, target, new Object[] { value });
//        }



//        public static T GetProperty<T>(this object target, string propertyName)
//        {
//            if (propertyName.IsNullOrEmpty())
//            {
//                throw new ArgumentNullException("propertyName of parameter can't be null");
//            }

//            return (T)target.GetType().InvokeMember(propertyName,
//                           BindingFlags.Static |
//                           BindingFlags.Public |
//                           BindingFlags.NonPublic |
//                           BindingFlags.Instance |
//                           BindingFlags.GetProperty, null, target, null);
//        }

//        public static Object GetProperty(this Object source, String propertyName) 
//        {
//            if (propertyName.IsNullOrEmpty()) throw new ArgumentNullException("propertyName can't be null or empty");

//            return source.GetType().InvokeMember(propertyName, 
//                           BindingFlags.Static |
//                           BindingFlags.Public |
//                           BindingFlags.NonPublic |
//                           BindingFlags.Instance |
//                           BindingFlags.GetProperty, null, source, null);
//        }

//        public static void SetProperty(this object target, string propertyName, object value)
//        {
//            if (propertyName.IsNullOrEmpty())
//            {
//                throw new ArgumentNullException("propertyName of parameters can't be null");
                
//            }

//            target.GetType().InvokeMember(propertyName,
//                                       BindingFlags.Static |
//                                       BindingFlags.Public |
//                                       BindingFlags.NonPublic |
//                                       BindingFlags.Instance |
//                                       BindingFlags.SetProperty, null, target, new Object[] { value });

//        }

//        public static void InvokeMethod(this object target, string methodName)
//        {
//            InvokeMethod(target, methodName, null);
//        }

//        public static void InvokeMethod(this object target, string methodName, params object[] parameters) 
//        {
//            InvokeMethod(target, methodName, null,parameters);
//        }

//        public static void InvokeMethod(this object target, string methodName, Type[] genericTypes)
//        {
//            InvokeMethod(target, methodName, genericTypes, null);
//        }

//        public static void InvokeMethod(this object source, string methodName, Type[] genericTypes, params object[] parameters) 
//        {
//            MethodInfo methodInfo = source.GetMethodInfo(methodName, genericTypes, parameters);

//            if (methodInfo != null && methodInfo.ReturnType == typeof(void))
//            {
//                methodInfo.Invoke(source, parameters);
//            }
//            else 
//            {
//                throw new System.Exception("can't execute it...some wrong");
//            }
//        }

//        public static ReturnType InvokeMethod<ReturnType>(this object target, string methodName)
//        {
//            return InvokeMethod<ReturnType>(target, methodName, null);
//        }

//        public static ReturnType InvokeMethod<ReturnType>(this object target, string methodName, params object[] parameters)
//        {
//            return InvokeMethod<ReturnType>(target, methodName, null, parameters);
//        }

//        public static ReturnType InvokeMethod<ReturnType>(this object target, string methodName, Type[] genericTypes)
//        {
//            return InvokeMethod<ReturnType>(target, methodName, genericTypes, null);
//        }

//        public static ReturnType InvokeMethod<ReturnType>(this object source, string methodName, Type[] genericTypes, params object[] parameters)
//        {

//            MethodInfo methodInfo = source.GetMethodInfo(methodName, genericTypes, parameters);

//            if (methodInfo != null && methodInfo.ReturnType == typeof(ReturnType))
//            {
//                return (ReturnType)methodInfo.Invoke(source, parameters);
//            }
//            else 
//            {
//                throw new System.Exception("can't execute it...some wrong");
//            }
//        }

//        private static MethodInfo GetMethodInfo(this object target, string methodName, Type[] genericTypes, params object[] parameters) 
//        {
//            if (methodName.IsNullOrEmpty())
//            {
//                throw new ArgumentNullException("methodName of parameters can't be null");
//            }            
                       
            
//            MethodInfo[] targetMethods = target.GetType().GetMethods(BindingFlags.Static |
//                                                        BindingFlags.Public |
//                                                        BindingFlags.NonPublic |
//                                                        BindingFlags.Instance |
//                                                        BindingFlags.InvokeMethod |
//                                                        BindingFlags.CreateInstance);

//            IList<MethodInfo> result = new List<MethodInfo>();

          
//            //ensure the methodname is exist.
//            if (targetMethods.IsNullOrEmpty())
//            {
//                throw new ArgumentException(string.Format("The {0} method is not found", methodName));
//            }
//            else 
//            {
//                result = targetMethods.Where(o => o.Name == methodName).ToList();                

//                if(result.Count() > 0)                
//                    targetMethods = result.ToArray<MethodInfo>();               
//                else                
//                    throw new ArgumentException(string.Format("The {0} method is not found", methodName));                
//            }

//            if (result.Count > 0)            
//                targetMethods = result.ToArray<MethodInfo>();            
//            else             
//                throw new ArgumentException(string.Format("The {0} method is not found", methodName));
            


//            //ensure parameter's type is the same
//            result.Clear();
//            foreach(MethodInfo methodInfo in targetMethods)
//            {
//                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

//                Type[] targetMethodParameterTypes = new Type[parameterInfos.Length];
//                Type[] parameterTypes = parameters.GetTypeArray();

//                int i = 0;
//                foreach (ParameterInfo parameterInfo in parameterInfos)
//                {
//                    targetMethodParameterTypes[i] = parameterInfo.ParameterType;                    
//                    i++;
//                }

//                if (targetMethodParameterTypes.IsNullOrEmpty() && parameterTypes.IsNullOrEmpty()) 
//                {
//                    result.Add(methodInfo);
//                }
//                else if (parameterTypes.IsAssignableArrayFrom(targetMethodParameterTypes))
//                {
//                    result.Add(methodInfo);
//                }
//            }

//            if (result.Count > 0)
//                targetMethods = result.ToArray<MethodInfo>();
//            else         
//                throw new ArgumentException("no method with signature does match");     



//            //detect the method is generic method or stand method
//            result.Clear();
//            if (genericTypes.IsNullOrEmpty())
//            {
//                foreach (MethodInfo methodInfo in targetMethods)
//                {
//                    if (methodInfo.IsGenericMethod == false)
//                    {
//                        result.Add(methodInfo);
//                    }
//                }
//            }
//            else
//            {
//                foreach (MethodInfo methodInfo in targetMethods)
//                {
//                    if (methodInfo.IsGenericMethod)
//                    {
//                        MethodInfo newMethodInfo = methodInfo.MakeGenericMethod(genericTypes);
//                        result.Add(newMethodInfo);
//                    }
//                }
//            }

//            if (result.Count > 0)
//                targetMethods = result.ToArray<MethodInfo>();
//            else
//                throw new ArgumentException("no method with generic type does match");    


//            //excute it
//            if (targetMethods.Length == 1)
//            {
//                return targetMethods[0];            
//            }
//            else 
//            {
//                throw null;            
//            }
            
//        }






//        // To-Do: 
//        // 1. support invoke generic method
//        // 2. support invoke property
//        public static object DynamicInvokeMember(this object target, string methodName, params object[] parameters) 
//        {
            
//            Type[] paramTypes = new Type[0];

//            if (parameters != null && parameters.Length > 0)
//            {
//                paramTypes = new Type[parameters.Length];

//                int index = 0;
//                foreach (object parameter in parameters)
//                {
//                    paramTypes[index] = parameters[index].GetType();
//                    index++;
//                }
//            }

//            // Verify that the method exists and get its MethodInfo obj.
//            MethodInfo invokedMethod = target.GetType().GetMethod(methodName, paramTypes);

//            if (invokedMethod != null)
//            {
//                if (!parameters.IsNullOrEmpty())
//                {
//                    // Create the parameter list for the dynamically invoked methods.
//                    int index = 0;
//                    // For each parameter, add it to the list.
//                    foreach (object parameter in parameters)
//                    {
//                        // Get the type of the parameter.
//                        Type paramType = invokedMethod.GetParameters()[index].ParameterType;
//                        // Change the value to that type and assign it.
//                        parameters[index] = Convert.ChangeType(parameter, paramType);
//                        index++;
//                    }
//                }

//                // Invoke the method with the parameters.
//                object result = invokedMethod.Invoke(target, parameters);

//                // Return the returned object.
//                return (result);
//            }
//            else 
//            {
//                throw new ArgumentException(string.Format("The {0} method is not exist", methodName));
//            }

//        }



//        public static Type[] GetTypeArray(this object[] target) 
//        {
//            Type[] paramTypes = null;

//            if (!target.IsNullOrEmpty())
//            {
//                paramTypes = new Type[target.Length];

//                int index = 0;
//                foreach (object parameter in target)
//                {
//                    paramTypes[index] = target[index].GetType();
//                    index++;
//                }
//            }

//            return paramTypes;
//        }
//    }
//}
