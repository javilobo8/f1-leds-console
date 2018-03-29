﻿using System;

namespace F1LedsConsole
{
    class F1DataInterface
    {
        private ArduinoSerial serial;
        private double now;

        public F1DataInterface(ArduinoSerial _serial)
        {
            this.serial = _serial;
        }

        public void Send(F1DataStruct.F1Data data)
        {
            now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

            UInt32 gear = (UInt32)data.gear;
            UInt32 kmh = (UInt32)(data.speed * 3.6f);
            UInt32 rpm = (UInt32)CalcRPMByte(data.engineRate);
            UInt32 lapTime = (UInt32)(data.lapTime * 10f);

            serial.Write(StructUtils.ToByteArray(new SerialData{
                gear = gear,
                kmh = kmh,
                rpm = rpm,
                lapTime = lapTime
            }));
        }

        // RPM Constants
        private static float RPM_MIN = 11030f;
        private static float RPM_MAX = 11630f;
        private static int N_LEDS = 8;
        private static int ms_blink = 10;
        private double last_date = 0;
        private int maxrpmstate = 0;

        public int CalcRPMByte(float engineRate)
        {
            float percent = ((float)engineRate - RPM_MIN) * 100f / (RPM_MAX - RPM_MIN);
            int leds = (int)Math.Ceiling(N_LEDS * percent / 100f);

            if (leds <= 0)
                leds = 0;
            else if (leds > N_LEDS)
            {
                if (now - last_date > ms_blink)
                {
                    maxrpmstate = maxrpmstate == 0 ? N_LEDS : 0;
                    last_date = now;
                }
                leds = maxrpmstate;
            }
            return leds;
        }
    }
}
