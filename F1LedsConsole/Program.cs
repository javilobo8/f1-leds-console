using System.Threading;

namespace F1LedsConsole
{
    delegate void ConvertMethod(byte[] packet);

    class Program
    {
        static UDPServer udp_server;
        static ConvertMethod OnPacketReceivedMethod;
        static Thread udp_thread;
        static ArduinoSerial serial;
        static F1DataInterface data_interface;


        static void Main(string[] args)
        {
            OnPacketReceivedMethod = OnPacketReceived;
            udp_server = new UDPServer(20776, "127.0.0.1", OnPacketReceivedMethod);
            udp_thread = new Thread(() => udp_server.ListenLoop());

            serial = new ArduinoSerial();
            data_interface = new F1DataInterface(serial);
            Init();
        }

        static void Init()
        {
            udp_thread.Start();
            serial.Connect(ArduinoSerial.GetArdunioPort());
            WelcomeMessage();
        }

        static void WelcomeMessage()
        {
            int delay = 100;
            serial.Write(new byte[] { 0b11111111, 0b00000001 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00000000, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00000001, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00000010, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00000101, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00001010, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00010101, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00101010, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b01010101, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b10101010, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b10101011, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b10101111, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b10111111, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b11111111, 0b00000000 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b11111111, 0b00000001 });
            Thread.Sleep(delay);
            serial.Write(new byte[] { 0b00000000, 0b00000000 });
            Thread.Sleep(delay);
        }

        static void OnPacketReceived(byte[] packet)
        {
            data_interface.Send(new F1Data(packet));
        }
    }
}
