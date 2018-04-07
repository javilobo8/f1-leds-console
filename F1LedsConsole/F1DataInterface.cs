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


        private double now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        private double last_date = 0;
        private double rate = 1000 / 120; // FPS

        public void Send(F1DataStruct.F1Data data)
        {
            if (CanSend())
            { 
                arduino_data.led_color = neoPixelRpm.CalcRGBLeds(data.engineRate);
                serial.Write(StructUtils.structToBytes(arduino_data));
            }
        }

        public bool CanSend()
        {
            now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            if (now > last_date + rate)
            {
                last_date = now;
                return true;
            }
            return false;
        }
    }
}
