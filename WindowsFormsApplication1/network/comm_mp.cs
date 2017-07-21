using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ReversiClient.network
{
    partial class comm:IDisposable
    {
        // Client side
        TcpClient game_cli;
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
