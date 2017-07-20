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
        comm_lobby_list LobbyList = new comm_lobby_list();

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
                        System.Diagnostics.Debug.WriteLine("Beacon received from " + bcn_rcv_ip.Address.ToString());
                        LobbyList.add_ip_to_list(bcn_rcv_ip.Address);
                    }
                }
                catch (System.Net.Sockets.SocketException) {
                    // continue
                }
            }
            //System.Diagnostics.Debug.WriteLine("Beacon receiving ended.");
        }
    }

    internal class comm_lobby_list {
        private const int item_lifespan = 5000;
        private Object list_lock = new object();
        public List<Tuple<IPAddress, long, Timer>> host_list = new List<Tuple<IPAddress, long, Timer>>(); // Host ip and current time
        public List<Tuple<IPAddress, long, Timer>> HostList {
            get {
                lock (list_lock) {
                    return this.host_list;
                }
            }
        }

        public void add_ip_to_list(IPAddress some_ip) {
            bool something_to_add = false;
            lock (list_lock) {
                int existing_item_index = host_list.FindIndex(each_ip => each_ip.Item1.Equals(some_ip));
                if (existing_item_index >= 0) {
                    Timer item_expire = host_list[existing_item_index].Item3;
                    item_expire.Stop();
                    item_expire.Start();
                    host_list[existing_item_index] = new Tuple<IPAddress, long, Timer>(some_ip, DateTime.UtcNow.Ticks, item_expire);
                }
                else {
                    Timer item_expire = new Timer();
                    item_expire.Elapsed += list_item_expire;
                    item_expire.Interval = item_lifespan;
                    item_expire.Enabled = true;
                    host_list.Add(new Tuple<IPAddress, long, Timer>(some_ip, DateTime.UtcNow.Ticks, item_expire));
                    something_to_add = true;
                }
            }
            if (something_to_add) {
                ListChangeEventArgs args = new ListChangeEventArgs();
                args.ip = some_ip;
                args.mode = 1;
                OnListChange(args);
            }
        }
        private void list_item_expire(object source, ElapsedEventArgs e) {
            bool something_to_remove = false;
            int existing_item_index;
            IPAddress ip_to_remove = null;
            lock (list_lock) {
                existing_item_index = host_list.FindIndex(each_ip => each_ip.Item3 == (Timer)source);
                if (existing_item_index >= 0) {
                    host_list[existing_item_index].Item3.Dispose();
                    ip_to_remove = host_list[existing_item_index].Item1;
                    host_list.RemoveAt(existing_item_index);
                    something_to_remove = true;
                }
            }
            if (something_to_remove) {
                ListChangeEventArgs args = new ListChangeEventArgs();
                args.ip = ip_to_remove;
                args.mode = 2;
                OnListChange(args);
            }
        }

        public event EventHandler<ListChangeEventArgs> RaiseListChangeEvent;

        protected void OnListChange(ListChangeEventArgs e) {
            EventHandler<ListChangeEventArgs> handler = RaiseListChangeEvent;
            if (handler != null) {
                handler(this, e);
            }
        }
    }
    internal class ListChangeEventArgs : EventArgs {
        public int mode { get; set; } // 1: add, 2: remove
        public IPAddress ip { get; set; }
    }
}
