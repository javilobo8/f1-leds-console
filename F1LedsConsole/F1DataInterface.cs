using System;

namespace F1LedsConsole
{
    class F1DataInterface
    {
        private ArduinoSerial serial;
        private NeoPixelRPM neoPixelRpm;

        private ArduinoSerialStructure arduino_data = new ArduinoSerialStructure
        {
            led_color = new UInt32[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        public F1DataInterface(ArduinoSerial _serial)
        {
            this.serial = _serial;
            neoPixelRpm = new NeoPixelRPM(16, 64, 11030f, 11630f);
        }

        public void Send(F1DataStruct.F1Data data)
        {
            arduino_data.led_color = neoPixelRpm.CalcRGBLeds(data.engineRate);

            serial.Write(StructUtils.structToBytes(arduino_data));
        }

        //// RPM Constants
        //private static float RPM_MIN = 11030f; // 11030f | 4300f
        //private static float RPM_MAX = 11630f;
        //private static int NUM_LEDS_MULTP = 64;
        //private static int NUM_LEDS = 16 * NUM_LEDS_MULTP;
        //private static int ms_blink = 50;
        //private double last_date = 0;
        //private int maxrpmstate = 0;

        //public UInt32 CalcRPMByte(float engineRate)
        //{
        //    float percent = ((float)engineRate - RPM_MIN) * 100f / (RPM_MAX - RPM_MIN);
        //    int leds = (int)Math.Ceiling(NUM_LEDS * percent / 100f);

        //    if (leds <= 0)
        //        leds = 0;
        //    else if (leds > NUM_LEDS)
        //    {
        //        if (now - last_date > ms_blink)
        //        {
        //            maxrpmstate = maxrpmstate == 0 ? NUM_LEDS : 0;
        //            last_date = now;
        //        }
        //        leds = maxrpmstate;
        //    }
        //    return (UInt32)leds;
        //}
    }
}
