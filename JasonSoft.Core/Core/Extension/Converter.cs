using System;
using System.ComponentModel;

namespace JasonSoft
{
    public static partial class CoreExtension
    {
        public static Char ToASCII(this int source)
        {
            if (!source.IsBetween(0, 127)) throw new ArgumentException("source must between 0 and 127");
            return (Char)source;
        }

        //[Obsolete("please use ChangeTypeTo", true)]
        //public static int ToInt32(this string source)
        //{
        //    return Int32.Parse(source);
        //}

        //public static Int32 ToInt32(this String source, Exception exception)
        //{
        //    try
        //    {
        //        return Int32.Parse(source);
        //    }
        //    catch
        //    {
        //        throw exception;
        //    }
        //}

        //public static Double ToDouble(this string source)
        //{
        //    return Double.Parse(source);
        //}

        //public static Single ToSingle(this string source)
        //{
        //    return Single.Parse(source);
        //}

        //public static Byte ToByte(this string source)
        //{
        //    return Byte.Parse(source);
        //}

        //public static Boolean ToBoolean(this String source)
        //{
        //    return Boolean.Parse(source);
        //}

        public static bool TryInt32(this string str, out int result)
        {
            return int.TryParse(str, out result);
        }


        //public static short ToInt16(this string source)
        //{
        //    return Int16.Parse(source);
        //}

        //public static long ToInt64(this string str)
        //{
        //    return Int64.Parse(str);
        //}

        public static T ToEnum<T>(this String source)
        {
            return (T)Enum.Parse(typeof(T), source, false);
        }

        /// <summary>
        /// Returns an Object with the specified Type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An Object that implements the IConvertible interface.</param>
        /// <returns>
        /// An object whose Type is conversionType (or conversionType's underlying type if conversionType
        /// is Nullable&lt;&gt;) and whose value is equivalent to value. -or- a null reference, if value is a null
        /// reference and conversionType is not a value type.
        /// </returns>
        /// <remarks>
        /// This method exists as a workaround to System.Convert.ChangeType(Object, Type) which does not handle
        /// nullables as of version 2.0 (2.0.50727.42) of the .NET Framework. The idea is that this method will
        /// be deleted once Convert.ChangeType is updated in a future version of the .NET Framework to handle
        /// nullable types, so we want this to behave as closely to Convert.ChangeType as possible.
        /// This method was written by Peter Johnson at:
        /// http://aspalliance.com/852
        /// </remarks>
        /// 
        public static T ChangeTypeTo<T>(this object value)
        {
            Type conversionType = typeof(T);
            return (T)ChangeTypeTo(value, conversionType);
        }

        private static object ChangeTypeTo(this object source, Type conversionType)
        {
            // Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
            // checking properties on conversionType below.
            if (conversionType == null)
                throw new ArgumentNullException("conversionType");

            // If it's not a nullable type, just pass through the parameters to Convert.ChangeType

            if (conversionType.IsNullableType())
            {
                // It's a nullable type, so instead of calling Convert.ChangeType directly which would throw a
                // InvalidCastException (per http://weblogs.asp.net/pjohnson/archive/2006/02/07/437631.aspx),
                // determine what the underlying type is
                // If it's null, it won't convert to the underlying type, but that's fine since nulls don't really
                // have a type--so just return null
                // Note: We only do this check if we're converting to a nullable type, since doing it outside
                // would diverge from Convert.ChangeType's behavior, which throws an InvalidCastException if
                // value is null and conversionType is a value type.
                if (source == null)
                    return null;

                // It's a nullable type, and not null, so that means it can be converted to its underlying type,
                // so overwrite the passed-in conversion type with this underlying type
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            //Jason: support Guid type
            if (conversionType == typeof(Guid))
            {
                return new Guid(source.ToString());
            }


            // Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
            // nullable type), pass the call on to Convert.ChangeType
            return Convert.ChangeType(source, conversionType);
        }
    }
}
