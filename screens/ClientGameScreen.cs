using MenschADN.game;
using MenschADN.players;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string address;
        public int playingColor;
        internal System.Windows.Forms.Timer messageTimer;
        public ClientGameScreen(Displayer parent, Screen screenParent) : base(parent, screenParent)
        {
            address = network.NetworkReq.LOCAL;

            currentPlayers = new LocalPlayer[4];
            currentPlayers[0] = new LocalPlayer(this, 0, "a");
            currentPlayers[1] = new LocalPlayer(this, 1, "a");
            currentPlayers[2] = new LocalPlayer(this, 2, "a");
            currentPlayers[3] = new LocalPlayer(this, 3, "a");
        }
        public override void Create()
        {
            base.Create();
            messageTimer = new System.Windows.Forms.Timer();
            messageTimer.Interval = 100;
            messageTimer.Tick += HandelMessageTick;
            messageTimer.Start();
            try
            {
                TcpClient cl = new TcpClient(address, network.NetworkReq.PORT);
                clientConn = new network.NetworkReq(cl);
                clientConn.SendStream(BitConverter.GetBytes(network.NetworkReq.HELLO_MSG));
                Debug.WriteLine("send-data!");
            }
            catch { base.GetChangeBackScreen(null, EventArgs.Empty); }
        }

        private void HandelMessageTick(object? sender, EventArgs e)
        {
            byte[] recv = clientConn.ReadStream();
            if (recv.Length < 2) return;
            if (!clientConn.isHandelt)
            {
                int color = recv[0];
                if (color == network.NetworkReq.CLOSE_MSG)
                    base.GetChangeBackScreen(sender,e);
                clientConn.isHandelt = true;
                Debug.WriteLine(color);
                playingColor = color;
                // shift down! everything inside of ercv
                for (int i = 0; i < 10; recv[i++] = recv[i]);
            }

            for (int ei = 0; ei < 6; ei++)
                Debug.Write(recv[ei] + ",");
            Debug.WriteLine("recv");
            if (recv[1] != 40)
            {
                currentPlayerIndex = recv[0];
                currentPlayers[currentPlayerIndex].diceNumber = recv[1];
                UpdateCurrentDisplay();
                for (int i = 0; i < board.allPieces.Length; i++)
                {
                    if (board.allPieces[i].color == recv[2] && board.allPieces[i].localIndex == recv[3])
                    {
                        board.allPieces[i].canMove = false;
                        if (recv[5] < 100)
                        {
                            board.allPieces[i].canMove = true;
                            board.allPieces[i].position = Math.Max(0, (int)recv[5]);
                        }
                        board.allPieces[i].Move(recv[4]);
                        break;
                    }
                }
            }


            // recv data!
        }

        public override void SlectPiece(GamePiece currentGamePiece)
        {
            if (currentGamePiece == null || currentColor != currentGamePiece.color || currentPlayerIndex != playingColor) { return; }
            byte[] data = { (byte)currentPlayerIndex, (byte)currentGamePiece.localIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,(byte)currentGamePiece.position };

            for (int ei = 0; ei < data.Length; ei++)
                Debug.Write(data[ei] + ",");
            Debug.WriteLine("send!");
            if (!clientConn.SendStream(data))
            {
                GetChangeBackScreen(this,EventArgs.Empty);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            messageTimer.Dispose();
            if(clientConn != null)clientConn.Close();
        }
    }
}
