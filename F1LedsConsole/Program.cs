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
        }

        static void OnPacketReceived(byte[] packet)
        {
            data_interface.Send(F1DataStruct.Convert(packet));
        }
    }
}
