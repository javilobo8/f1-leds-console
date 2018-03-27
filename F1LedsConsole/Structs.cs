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
        public UInt16 rpm;
        public UInt16 drs;
        public UInt16 gear;
        public UInt16 kmh;
        public UInt16 lapTime;
    }

    class StructUtils
    {
        public static byte[] ToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }

     
}
