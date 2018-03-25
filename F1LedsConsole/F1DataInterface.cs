using System;

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

        public void Send(F1Data data)
        {
            now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

            byte rpm_byte = CalcRPMByte(data.Get("engineRate"));
            byte drs_byte = (byte)(int)data.Get("drs");

            byte[] serial_data = {
                rpm_byte,
                drs_byte
            };

            serial.Write(serial_data);
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
