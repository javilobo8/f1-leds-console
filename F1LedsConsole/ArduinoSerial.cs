using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;

namespace F1LedsConsole
{
    class ArduinoSerial
    {
        public static int BAUD_RATE = 115200;
        public String Port { get; set; }
        private SerialPort sp;
        public bool isConnected = false;

        public static string[] GetAvailablePorts() { return SerialPort.GetPortNames(); }
        public static string GetArdunioPort()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                throw new Exception("No COM ports");
            }

            if (ports.Length == 1)
            {
                Console.WriteLine("{0} selected", ports[0]);
                return ports[0];
            }

            Console.WriteLine("Select COM port:");
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine("  {0}) {1}", i + 1, ports[i]);
            }

            Console.Write("Port: ");
            ConsoleKeyInfo UserInput = Console.ReadKey(false);

            if (!char.IsDigit(UserInput.KeyChar))
            {
                throw new Exception("Wrong selection");
            }

            int selected = int.Parse(UserInput.KeyChar.ToString()) - 1;
            Console.WriteLine("\n{0} selected", ports[selected]);
            return ports[selected];
        }

        public ArduinoSerial() { }

        [StructLayout(LayoutKind.Explicit)]
        public struct ColorData
        {
            [FieldOffset(0)] public byte led;
            [FieldOffset(1)] public UInt32 led_color;
        }

        public void Connect(String _port)
        {
            Port = _port;
            sp = new SerialPort(Port, BAUD_RATE);
            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            sp.Open();
            isConnected = true;


            ColorData cd = new ColorData { led = 0, led_color = NeoPixelRPM.C_BLUE};

            int sleep = 100;
            while(true)
            {
                for (int i = 0; i < 16; i++)
                {
                    Write(StructUtils.ToByteArray(new ColorData { led = (byte)i, led_color = NeoPixelRPM.C_GREEN }));
                    Thread.Sleep(sleep);
                }

                for (int i = 0; i < 16; i++)
                {
                    Write(StructUtils.ToByteArray(new ColorData
                    {
                        led = (byte)i,
                        led_color = NeoPixelRPM.C_RED
                    }));
                    Thread.Sleep(sleep);
                }

                for (int i = 0; i < 16; i++)
                {
                    Write(StructUtils.ToByteArray(new ColorData
                    {
                        led = (byte)i,
                        led_color = NeoPixelRPM.C_BLUE
                    }));
                    Thread.Sleep(sleep);
                }
            }


            //sp.WriteLine("HL1,0,255,0");
            //sp.WriteLine("HL2,0,0,255");
            //Write(StructUtils.ToByteArray(cd));
            //cd.pos++;
            //Thread.Sleep(1000);

            //sp.WriteLine("L");
            //Write(StructUtils.ToByteArray(cd));
            //cd.pos++;
            //Thread.Sleep(1000);

            //sp.WriteLine("L");
            //Write(StructUtils.ToByteArray(cd));
            //cd.pos++;
            //Thread.Sleep(1000);

            //sp.WriteLine("L");
            //Write(StructUtils.ToByteArray(cd));
            //cd.pos++;
            //Thread.Sleep(1000);

            //Thread.Sleep(1000);
            //sp.Write("L");
            //sp.Write("1");
            //sp.Write("0");
            //sp.Write("255");
            //sp.Write("0");
            //Thread.Sleep(1000);
            //sp.Write("L");
            //sp.Write("2");
            //sp.Write("0");
            //sp.Write("0");
            //sp.Write("255");

        }

        public void Disconnect()
        {
            if (!isConnected) return;
            sp.Close();
            isConnected = false;
        }

        public void Write(byte[] bytes)
        {
            if (!isConnected) return;
            sp.Write(bytes, 0, bytes.Length);
        }

        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.Write(sp.ReadExisting());
        }
    }
}
