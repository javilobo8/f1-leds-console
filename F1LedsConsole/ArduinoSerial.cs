using System;
using System.IO.Ports;

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

        public void Connect(String _port)
        {
            Port = _port;
            sp = new SerialPort(Port, BAUD_RATE);
            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            sp.Open();
            isConnected = true;
        }

        public void printBytes(byte[] bytes)
        {
            //Console.WriteLine("NEW BYTE");
            for (int i = 0; i < bytes.Length; i++)
            {
                Console.WriteLine("{0} => {1}", i, Convert.ToString(bytes[i], 2).PadLeft(8, '0'));
            }
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
            //Console.WriteLine("Sent {0} bytes", bytes.Length);
        }

        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Console.Write(sp.ReadExisting());
        }
    }
}
