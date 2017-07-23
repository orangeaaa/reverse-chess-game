using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ReversiClient.network
{
    partial class comm : IDisposable {
        // Client side
        TcpClient game_cli;
        string server_ip;
        System.Threading.Thread t_game_cli_conn;
        public event EventHandler<GameClientConnectEventArgs> RaiseGameClientConnectEvent;

        public void game_cli_connect(string what_ip) {
            server_ip = what_ip;
            game_cli = new TcpClient();
            t_game_cli_conn = new System.Threading.Thread(game_cli_connect_act);
            t_game_cli_conn.IsBackground = true;
            t_game_cli_conn.Start();
        }
        public void game_cli_connect_stop() {
            game_cli.Close();
        }
        protected void game_cli_connect_act() {
            bool success = true;
            try {
                if (!game_cli.ConnectAsync(server_ip, game_port).Wait(3000)) {
                    throw new TimeoutException();
                }
            }
            catch (TimeoutException) {
                success = false;
                GameClientConnectEventArgs args = new GameClientConnectEventArgs() { result = 1 };
                OnGameClientConnect(args);
            }
            catch (AggregateException) {
                success = false;
                // Probably closed connection.
                // Do nothing
            }
            if (success) {
                GameClientConnectEventArgs args = new GameClientConnectEventArgs() { result = 0 };
                OnGameClientConnect(args);
            }
        }
        protected void OnGameClientConnect(GameClientConnectEventArgs e) {
            EventHandler<GameClientConnectEventArgs> handler = RaiseGameClientConnectEvent;
            if (handler != null) {
                handler(this, e);
            }
        }

        // Server side
        TcpListener game_srv;
        System.Threading.Thread t_game_srv_listen;
        TcpClient game_srv_cli;
        public void game_srv_listen() {
            t_game_srv_listen = new System.Threading.Thread(game_srv_listen_act);
            t_game_srv_listen.IsBackground = true;
            t_game_srv_listen.Start();
        }
        public void game_srv_listen_stop() {
            game_srv.Stop();
        }
        void game_srv_listen_act() {
            game_srv = new TcpListener(IPAddress.Any, game_port);
            game_srv.Start();
            try {
                game_srv_cli = game_srv.AcceptTcpClient();
            }
            catch (SocketException) {
                // The connection has been canceled.
                return;
            }
            // Connected
            game_srv.Stop();

        }
    }

    internal class GameClientConnectEventArgs : EventArgs {
        public int result { get; set; } // 0: success 1: fail
    }

    abstract class rgp { // Reversi Game Protocol
        public rgp() {
            IsValidMsg = true;
            Message = null;
        }

        // Flag
        protected bool IsValidMsg;

        // Header
        protected abstract int HeaderLength { get; } // in bytes

        // Remaining Msg
        protected rgp Message;

        // Interface
        protected abstract byte[] HeaderToBytes();
        public abstract bool FromBytes(byte[] raw);
        public byte[] ToBytes() {
            if (Message != null) {
                return HeaderToBytes().Concat(Message.ToBytes()).ToArray();
            }
            else {
                return HeaderToBytes();
            }
        }

    }
    class reversi_msg : rgp { // The Message Structure that Reversi is going to use
        /* Header
            ----------------
            | type (8 bit) |
            ----------------
        */
        protected override int HeaderLength {
            get { return 1; }
        }
        public int type; // 0: Game conn, 1: Game proc, 2: Chat msg

        // Interface
        protected override byte[] HeaderToBytes() {
            byte[] header = new byte[] { (byte)(type) };
            return header;
        }
        public override bool FromBytes(byte[] raw) {
            if (raw.Length < HeaderLength) {
                IsValidMsg = false;
                return false;
            }
            // Get Header
            type = raw[0];
            // Create higher level msg according to header info
            switch (type) {
                case 0:
                    Message = new reversi_msg_rawData();
                    break;
                case 1:
                    Message = new reversi_msg_rawData();
                    break;
                case 2:
                    Message = new reversi_msg_rawData();
                    break;
                default:
                    IsValidMsg = false;
                    Message = null;
                    break;
            }
            // Get remaining info
            byte[] raw_remain = new byte[raw.Length - HeaderLength];
            Array.Copy(raw, HeaderLength, raw_remain, 0, raw_remain.Length);
            // Deal with remaining info
            if (Message != null) {
                bool MessageValid = Message.FromBytes(raw_remain);
                if (!MessageValid) IsValidMsg = false;
            }
            return IsValidMsg;
        }
    }
    class reversi_msg_rawData : rgp {
        // No actual header. Always raw data
        protected override int HeaderLength {
            get { return Data.Length; }
        }
        public byte[] Data;

        // Interface
        protected override byte[] HeaderToBytes() {
            return Data;
        }
        public override bool FromBytes(byte[] raw) {
            Data = (byte[])raw.Clone();
            Message = null;
            return IsValidMsg;
        }
    }
}
