using System;

namespace F1LedsConsole
{
    class F1DataInterface
    {
        private ArduinoSerial serial;
        private double now;

        private SerialData serialData = new SerialData
        {
            gear = 0,
            kmh = 0,
            rpm = 0,
            lapTime = 0,
            drsAllowed = 0,
            drs = 0,
        };

        public F1DataInterface(ArduinoSerial _serial) { this.serial = _serial; }

        public void Send(F1DataStruct.F1Data data)
        {
            now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

            serialData.gear = (UInt32)data.gear;
            serialData.kmh = (UInt32)(data.speed * 3.6f);
            serialData.rpm = CalcRPMByte(data.engineRate);
            serialData.lapTime = (UInt32)(data.lapTime * 1000f);
            serialData.drsAllowed = (UInt32)data.drsAllowed;
            serialData.drs = (UInt32)data.drs;

            serial.Write(StructUtils.ToByteArray(serialData));
        }

        // RPM Constants
        private static float RPM_MIN = 11030f; // 11030f | 4300f
        private static float RPM_MAX = 11630f;
        private static int NUM_LEDS_MULTP = 64;
        private static int NUM_LEDS = 16 * NUM_LEDS_MULTP;
        private static int ms_blink = 50;
        private double last_date = 0;
        private int maxrpmstate = 0;

        public UInt32 CalcRPMByte(float engineRate)
        {
            float percent = ((float)engineRate - RPM_MIN) * 100f / (RPM_MAX - RPM_MIN);
            int leds = (int)Math.Ceiling(NUM_LEDS * percent / 100f);

            if (leds <= 0)
                leds = 0;
            else if (leds > NUM_LEDS)
            {
                if (now - last_date > ms_blink)
                {
                    maxrpmstate = maxrpmstate == 0 ? NUM_LEDS : 0;
                    last_date = now;
                }
                leds = maxrpmstate;
            }
            return (UInt32)leds;
        }
    }
}
