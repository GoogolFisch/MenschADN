using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public class ServerGameScreen : GameScreen
    {
        public ServerGameScreen(Displayer parent, Screen parentScreen) : base(parent, parentScreen)
        {
        }
        public override void Create()
        {
            base.Create();
            tcpListen = new TcpListener(IPAddress.Parse("0.0.0.0"), network.NetworkReq.PORT);
            tcpListen.Start();
            willAccept = true;
            servClient = new();
        }
        public TcpListener tcpListen;
        public List<network.NetworkReq> servClient;
        public bool willAccept = false;

        public void ClientBabysitting()
        {
            if (tcpListen.Pending() && willAccept)
            {
                try
                {
                    network.NetworkReq clcn = new network.NetworkReq(tcpListen.AcceptTcpClient());
                    servClient.Add(clcn);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Something went wrong with the client accepting!");
                }
            }
        }

        public void Stop()
        {
            tcpListen.Stop();
        }
    }
}
