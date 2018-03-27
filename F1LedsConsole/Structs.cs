using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace F1LedsConsole
{
    struct SerialData
    {
        public UInt32 gear;
        public UInt32 kmh;
        public UInt32 rpm;
        public UInt32 lapTime;
    }

    class StructUtils
    {
        public static byte[] ToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            // Console.WriteLine(len);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }

     
}
