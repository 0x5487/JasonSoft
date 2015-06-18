using System;

namespace JasonSoft
{
    public class ApplicationHelper
    {
        public static CLRVersion RunOnClr()
        {
            Int32 result = IntPtr.Size;
            return result == 8 ? CLRVersion._64Bit : CLRVersion._32Bit;
        }
    }

    public enum CLRVersion
    {
        _32Bit,
        _64Bit
    }
}
