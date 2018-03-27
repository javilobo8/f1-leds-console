using System;
using System.Runtime.InteropServices;

namespace F1LedsConsole
{
    class F1DataInterface
    {
        private ArduinoSerial serial;

        // RPM Constants
        private static float RPM_MIN = 11030f;
        private static float RPM_MAX = 11600f;
        private static int N_LEDS = 6;
        private static int ms_blink = 150;
        private double last_date = 0;
        private double now;

        public F1DataInterface(ArduinoSerial _serial)
        {
            this.serial = _serial;
        }

        public void Send(F1DataStruct.F1Data data)
        {
            now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

            // UInt32 rpm = (UInt32)CalcRPMByte(data.engineRate);
            // UInt32 drs = (UInt32)data.drs;
            UInt32 gear = (UInt32)data.gear;
            UInt32 kmh = (UInt32)(data.speed * 3.6f);
            UInt32 rpm = (UInt32)data.engineRate;
            UInt32 lapTime = (UInt32)(data.lapTime * 10f);

            SerialData sd = new SerialData
            {
                gear = gear,
                kmh = kmh,
                rpm = rpm,
                lapTime = lapTime
            };

            // Console.WriteLine(sd.gear);
            // Console.WriteLine(sd.kmh);
            // Console.WriteLine(sd.lapTime);

            serial.Write(StructUtils.ToByteArray(sd));
        }



        public byte CalcRPMByte(float engineRate)
        {
            float percent = ((float)engineRate - RPM_MIN) * 100f / (RPM_MAX - RPM_MIN);
            int leds = (int)(N_LEDS * percent / 100f);
            if (leds <= 0)
            {
                leds = 0;
            }
            else if (leds == N_LEDS)
            {
                leds = 8;
            }
            else if (leds >= N_LEDS + 1)
            {
                if (now - last_date > ms_blink)
                {
                    leds = 8;
                    last_date = now;
                }
                else
                {
                    leds = 0;
                }
            }

            byte data = (byte)(Math.Pow(2, leds) - 1);
            return data;
        }
    }
}
