// Copyright 2009 JasonSoft - http://www.jasonsoft.net/
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Security;
using System.Xml;
using JasonSoft.Reflection;

namespace JasonSoft
{
    public static partial class CoreExtension
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        

        /// <summary>
        /// include begin and end
        /// </summary>
        /// <param name="source"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Boolean IsBetween(this int source, int begin, int end)
        {
            if (end < begin) throw new ArgumentException("end number must bigger than begin number");
            if (begin <= source && end >= source) return true;
            return false;
        }

        

        public static string ToMD5(this string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        

        public static DateTime ConvertTime(this DateTime source, String destinationTimeZoneID)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(source, destinationTimeZoneID);
        }

        public static DateTime ConvertTime(this DateTime source, String sourceTimeZoneID, String destinationTimeZoneID)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(source, sourceTimeZoneID, destinationTimeZoneID);
        }

        public static DateTime ConvertUTCTime(this DateTime source, String destinationTimeZoneID)
        {
            TimeZoneInfo utc = TimeZoneInfo.CreateCustomTimeZone("UTC", TimeSpan.Zero, "UTC", "UTC");
            source = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(source, utc.Id, destinationTimeZoneID);
            return source;
        }

        public static bool IsNullOrEmpty(this IEnumerable list)
        {
            return list == null || !list.GetEnumerator().MoveNext();
        }

        public static void Each<T>(this IEnumerable<T> source, Action<T> func)
        {
            foreach (T item in source)
            {
                func(item);
            }
        }

        public static void Each<T>(this IEnumerable<T> source, Action<Int32, T> func)
        {
            Int32 i = 0;
            foreach (T item in source)
            {
                func(i, item);
                i++;
            }
        }

        public static void Times(this Int32 source, Action<int> action)  
        {  
            for (int i = 0; i < source; i++)  
            {  
                action(i);  
            }  
        }  

        public static string ToJSON(this object source)
        {
            //if (!IsSerializable(source))
            //{
            //    throw new Exception("Source object must be serializable.");
            //}
            string json;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(source.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, source);
                json = Encoding.UTF8.GetString(ms.ToArray());
            }
            return json;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Serialize(source);
        }

        public static T FromJSON<T>(this string source)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(source)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(ms);
            }

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Deserialize<T>(source);
        }

        public static bool IsSerializable(this object source)
        {
            MemoryStream ms = null;
            BinaryFormatter bf = null;
            try
            {
                ms = new MemoryStream();
                bf = new BinaryFormatter();
                bf.Serialize(ms, source);
                return true;
            }
            catch (SerializationException)
            {
                return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }
        }

        /// <summary>
        /// Can't work with GetType();
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Boolean IsNullableType(this Type source)
        {
            //Reference: http://msdn.microsoft.com/en-us/library/ms366789.aspx
            return (source.IsGenericType && source.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// case sensitive
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Load(this object source, Object target)
        {
            Type sourceType = source.GetType();
            Type targetType = target.GetType();
            

            PropertyInfo[] propertyInfos = sourceType.GetProperties(BINDING_FLAGS);
            if (propertyInfos.IsNullOrEmpty()) return;

            foreach (PropertyInfo sourcePropertyInfo in propertyInfos)
            {
                PropertyInfo targetPropertyInfo = targetType.GetProperty(sourcePropertyInfo.Name, BINDING_FLAGS);
                Type sourcePropertyType = sourcePropertyInfo.PropertyType;

                if (sourcePropertyInfo.CanWrite && targetPropertyInfo != null)
                {
                    if (sourcePropertyType.FullName == targetPropertyInfo.PropertyType.FullName)
                    {
                        source.SetProperty(sourcePropertyInfo.Name, target.GetProperty(sourcePropertyInfo.Name));
                    }
                    else if(sourcePropertyType.UnderlyingSystemType.IsNullableType() && sourcePropertyType.GetType() == targetPropertyInfo.PropertyType.GetType())
                    {
                        source.SetProperty(sourcePropertyInfo.Name, target.GetProperty(sourcePropertyInfo.Name));
                    }

                }
            }
        }

        public static List<T> OrderRandomly<T>(this IList<T> source)
        {
            return source.OrderBy(emp => Guid.NewGuid()).ToList();
        }

        public static String Serialize(this Object source)
        {
            DataContractSerializer dcs = new DataContractSerializer(source.GetType());
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            dcs.WriteObject(xw, source);
            xw.Close();

            return sb.ToString();
        }

        public static T Deserialize<T>(this String source)
        {
            DataContractSerializer dsc = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(source));
            return (T)dsc.ReadObject(ms);
        }
    }
}
