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
        public const int PORT = 25555;
        public const string LOCAL = "127.0.0.1";
        public bool isHandelt;
        public bool inGame;
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
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);


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
    }
}
