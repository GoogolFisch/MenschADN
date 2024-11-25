using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public class ClientGameScreen : GameScreen
    {
        public network.NetworkReq clientConn;
        public ClientGameScreen(Displayer parent, Screen? screenParent) : base(parent, screenParent)
        {

        }
        public override void Create()
        {
            base.Create();
            try
            {
                TcpClient cl = new TcpClient(network.NetworkReq.LOCAL, network.NetworkReq.PORT);
                clientConn = new network.NetworkReq(cl);
            }
            catch {  }
        }
    }
}
