using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.network
{
    public class NetworkReq
    {
        public const int HELLO_MSG = 127465389;
        public const int CLOSE_MSG = 255;
        public const int PORT = 25555;
        public const string LOCAL = "127.0.0.1";
        public bool isHandelt;
        public bool inGame;
        public int playersColor = -1;
        static int idCounter = 0;
        public int id;
        public TcpClient conn;
        public NetworkStream stream;

        public NetworkReq( TcpClient conn)
        {
            this.stream = conn.GetStream();
            this.conn = conn;
            isHandelt = false;
            inGame = false;
            id = idCounter++;
        }
        public byte[] ReadStream()
        {
            if(!stream.DataAvailable)return new byte[0];
            byte[] data = new byte[256];
            int i = 0;
            while (stream.DataAvailable)
            {
                i += stream.Read(data, i, 1);
            }


            return data;
        }
        public bool SendStream(byte[] data)
        {

            try
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
            catch { return false; }
            return true;
        }

        internal void Close()
        {
            stream.Close();
        }
    }
}
