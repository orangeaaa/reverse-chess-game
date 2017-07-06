using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ReversiClient {
    class comm : IDisposable {

        const int game_port = 10000;

        Timer bcn_tm = new Timer();

        UdpClient cli = new UdpClient();
        IPEndPoint bcn_ip = new IPEndPoint(IPAddress.Broadcast, game_port);

        public comm() {
            bcn_tm.Elapsed += new ElapsedEventHandler(beaconEvent);
            bcn_tm.Interval = 1000;
            bcn_tm.Enabled = false;
        }
        public void Dispose() {
            cli.Close();
        }

        public void send_beacon(bool send = true) {
            bcn_tm.Enabled = send;
        }
        void beaconEvent(object source, ElapsedEventArgs e) {
            byte[] bytes = Encoding.ASCII.GetBytes("ReversiClient");
            cli.Send(bytes, bytes.Length, bcn_ip);
        }


    }
}
