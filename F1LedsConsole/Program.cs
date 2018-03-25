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
            serial.Connect(ArduinoSerial.GetArdunioPort());
            serial.Write(new byte[] { 255, 1 });
            udp_thread.Start();
        }

        static void OnPacketReceived(byte[] packet)
        {
            data_interface.Send(new F1Data(packet));
        }
    }
}
