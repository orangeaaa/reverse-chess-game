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
        const string beacon_msg = "ReversiClient";

        Timer bcn_tm = new Timer();

        UdpClient cli = new UdpClient();
        IPEndPoint bcn_ip = new IPEndPoint(IPAddress.Broadcast, game_port);

        UdpClient cli_bcn_rcv;
        IPEndPoint bcn_rcv_ip = new IPEndPoint(IPAddress.Any, game_port);
        private System.Threading.Thread t_bcn_rcv;
        private bool t_bcn_rcv_exit;
        private Object bcn_rcv_lock = new object();
        public List<Tuple<IPAddress, long>> host_list = new List<Tuple<IPAddress, long>>(); // Host ip and current time

        public comm() {
            bcn_tm.Elapsed += new ElapsedEventHandler(beaconEvent);
            bcn_tm.Interval = 1000;
            bcn_tm.Enabled = false;
        }
        public void Dispose() {
            cli.Close();
            cli_bcn_rcv.Close();
        }

        // Beacon frame
        public void send_beacon(bool send = true) {
            bcn_tm.Enabled = send;
        }
        void beaconEvent(object source, ElapsedEventArgs e) {
            byte[] bytes = Encoding.ASCII.GetBytes(beacon_msg);
            cli.Send(bytes, bytes.Length, bcn_ip);
        }

        // Receiving beacon
        public void beacon_receiving() {
            cli_bcn_rcv = new UdpClient();
            cli_bcn_rcv.Client.Bind(bcn_rcv_ip);
            t_bcn_rcv_exit = false;
            t_bcn_rcv = new System.Threading.Thread(beacon_receiving_action);
            t_bcn_rcv.IsBackground = true;
            t_bcn_rcv.Start();
        }
        public void beacon_receiving_stop() {
            cli_bcn_rcv.Close();
            t_bcn_rcv_exit = true;
        }
        private void beacon_receiving_action() {
            while (!t_bcn_rcv_exit) {
                try {
                    System.Diagnostics.Debug.WriteLine("Sniffing...");
                    byte[] rcv_bytes = cli_bcn_rcv.Receive(ref bcn_rcv_ip);
                    string rcv_data = Encoding.ASCII.GetString(rcv_bytes);
                    System.Diagnostics.Debug.WriteLine(rcv_data);
                    if (rcv_data == beacon_msg) {
                        System.Diagnostics.Debug.WriteLine("Beacon received.");
                        System.Diagnostics.Debug.WriteLine("From " + bcn_rcv_ip.Address.ToString());
                        

                        lock (bcn_rcv_lock) {
                            host_list.Add(new Tuple<IPAddress, long>(bcn_rcv_ip.Address, DateTime.UtcNow.Ticks));
                        }
                    }
                }
                catch (System.Net.Sockets.SocketException) {
                    // continue
                }
            }
            //System.Diagnostics.Debug.WriteLine("Beacon receiving ended.");
        }


    }
}
