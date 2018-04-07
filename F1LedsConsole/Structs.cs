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
        public UInt32 drsAllowed;
        public UInt32 drs;
    }

    struct LEDData
    {
        public UInt32[] neopixel;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ArduinoSerialStructure
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 16), FieldOffset(0)]
        public UInt32[] led_color;
    }

    class StructUtils
    {
        public static byte[] structToBytes<T>(T str) where T : struct
        {
            int size = Marshal.SizeOf<T>();
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr<T>(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }

     
}
