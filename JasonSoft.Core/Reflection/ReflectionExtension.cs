using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JasonSoft.Reflection
{
    public static class ReflectionExtension
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private static readonly IDictionary<String, FastSetHandler> setHandlerCache = new Dictionary<String, FastSetHandler>();
        private static readonly IDictionary<String, FastGetHandler> getHandlerCache = new Dictionary<String, FastGetHandler>();
        private static readonly IDictionary<String, FastInstanceHandler> instanceHandlerCache = new Dictionary<String, FastInstanceHandler>();
        private static readonly IDictionary<String, FastInstanceWithParameterHandler> instanceWithParameterHandlerCache = new Dictionary<String, FastInstanceWithParameterHandler>();


        public static object CreaetInstance(this Type source)
        {
            lock(instanceHandlerCache)
            {
                FastInstanceHandler fastInstanceHandler = null;
                String key = source.FullName;

                if(instanceHandlerCache.ContainsKey(key))
                {
                    fastInstanceHandler = instanceHandlerCache[key];
                }
                else
                {
                    fastInstanceHandler = DynamicMethodHelper.CreateInstanceHandler(source);
                    instanceHandlerCache.Add(key, fastInstanceHandler);
                }

                return fastInstanceHandler.Invoke();
            }
        }


        public static T GetField<T>(this Object source, string fieldName)
        {
            if (fieldName.IsNullOrEmpty()) throw new ArgumentNullException("fieldName","fieldName of parameter can't be null");

            Type targetType = source.GetType();
            String key = targetType.FullName + "|" + fieldName;

            lock (getHandlerCache)
            {
                FastGetHandler fastGetHandler = null;
                if (getHandlerCache.ContainsKey(key))
                {
                    fastGetHandler = getHandlerCache[key];
                }
                else
                {
                    FieldInfo fieldInfo = targetType.GetField(fieldName, BINDING_FLAGS);

                    if (fieldInfo == null) throw new InvalidOperationException("field name is not found");

                    fastGetHandler = DynamicMethodHelper.CreateFastGetHandler(fieldInfo);

                    getHandlerCache.Add(key, fastGetHandler);
                }

                return (T)fastGetHandler(source);
            }
        }


        public static void SetField(this object source, string fieldName, object value) 
        {
            if (fieldName.IsNullOrEmpty()) throw new ArgumentNullException("fieldName", "fieldName of parameter can't be null");
            
            Type targetType = source.GetType();
            String key = targetType.FullName + "|" + fieldName;

            lock (setHandlerCache)
            {
                FastSetHandler setHandler = null;
                if (setHandlerCache.ContainsKey(key))
                {
                    setHandler = setHandlerCache[key];
                }
                else
                {
                    FieldInfo fieldInfo = targetType.GetField(fieldName, BINDING_FLAGS);

                    if (fieldInfo == null) throw new InvalidOperationException("field name is not found");

                    setHandler = DynamicMethodHelper.CreateFastSetHandler(fieldInfo);

                    setHandlerCache.Add(key, setHandler);
                }

                setHandler(source, value);
            }
        }

        public static Object GetProperty(this object source, string propertyName)
        {
            if (propertyName.IsNullOrEmpty()) throw new ArgumentNullException("propertyName");

            Type targetType = source.GetType();
            String key = targetType.FullName + "|" + propertyName;

            lock (getHandlerCache)
            {
                FastGetHandler fastGetHandler = null;
                if (getHandlerCache.ContainsKey(key))
                {
                    fastGetHandler = getHandlerCache[key];
                }
                else
                {
                    PropertyInfo propInfo = targetType.GetProperty(propertyName, BINDING_FLAGS);

                    if (propInfo == null) throw new InvalidOperationException("property name is not found");

                    fastGetHandler = DynamicMethodHelper.CreateFastGetHandler(propInfo);

                    getHandlerCache.Add(key, fastGetHandler);
                }

                return fastGetHandler(source);
            }
        }

        public static T GetProperty<T>(this object source, string propertyName)
        {
            return (T) source.GetProperty(propertyName);
        }

        public static void SetProperty(this object source, string propertyName, object value)
        {
            if (propertyName.IsNullOrEmpty()) throw new ArgumentNullException("propertyName");

            Type targetType = source.GetType();
            String key = targetType.FullName + "|" + propertyName;

            lock (setHandlerCache)
            {
                FastSetHandler setHandler = null;
                if (setHandlerCache.ContainsKey(key))
                {
                    setHandler = setHandlerCache[key];
                }
                else
                {
                    PropertyInfo propInfo = targetType.GetProperty(propertyName, BINDING_FLAGS);

                    if (propInfo == null) throw new InvalidOperationException("property name is not found");

                    setHandler = DynamicMethodHelper.CreateFastSetHandler(propInfo);

                    setHandlerCache.Add(key, setHandler);
                }

                setHandler(source, value);
            }
        }

        public static DirectoryInfo GetDirectory(this Assembly source)
        {
            return new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)); 
        }
    }
}
