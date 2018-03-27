﻿using System;
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

            UInt16 rpm = (UInt16)CalcRPMByte(data.engineRate);
            UInt16 drs = (UInt16)data.drs;
            UInt16 gear = (UInt16)data.gear;
            UInt16 lapTime = (UInt16)data.lapTime;
            UInt16 kmh = (UInt16)(data.speed * 3.6f);

            serial.Write(StructUtils.ToByteArray(new SerialData {
                rpm = rpm,
                drs = drs,
                gear = gear,
                kmh = kmh,
                lapTime = lapTime
            }));
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
