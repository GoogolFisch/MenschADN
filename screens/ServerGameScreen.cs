using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MenschADN.game;

namespace MenschADN.screens
{
    public class ServerGameScreen : GameScreen
    {
        internal System.Windows.Forms.Timer messageTimer;
        int oldColor = -1;
        public ServerGameScreen(Displayer parent, Screen parentScreen) : base(parent, parentScreen)
        {
        }
        public override void Create()
        {
            base.Create();
            messageTimer = new System.Windows.Forms.Timer();
            messageTimer.Interval = 100;
            messageTimer.Tick += HandelMessageTick;
            messageTimer.Start();
            tcpListen = new TcpListener(IPAddress.Parse("0.0.0.0"), network.NetworkReq.PORT);
            tcpListen.Start();
            willAccept = true;
            servClient = new();
        }

        private void PublishData(byte[] data)
        {
            for (int overClient = servClient.Count - 1; overClient >= 0; overClient--)
            {
                network.NetworkReq netreq = servClient[overClient];
                if (!netreq.SendStream(data))
                {
                    servClient.Remove(netreq);
                }
            }
        }

        private void HandelMessageTick(object? sender, EventArgs e)
        {
            ClientBabysitting();
            for (int overClient = servClient.Count - 1; overClient >= 0; overClient--)
            {
                network.NetworkReq netreq = servClient[overClient];
                byte[] data = netreq.ReadStream();
                if (data.Length < 2) continue;
                if (!netreq.isHandelt)
                {
                    if (BitConverter.ToInt32(data, 0) != network.NetworkReq.HELLO_MSG) {
                        netreq.SendStream(BitConverter.GetBytes(network.NetworkReq.CLOSE_MSG));
                        netreq.Close();
                        servClient.Remove(netreq);
                    }
                    else
                    {
                        if (currentPlayerIndex != oldColor)
                        {
                            if (oldColor == -1) oldColor = currentPlayerIndex;
                            netreq.playersColor = currentPlayerIndex;
                            netreq.SendStream(BitConverter.GetBytes(currentPlayerIndex));
                            MoveToNextPlayer();
                            if (currentPlayerIndex == oldColor)
                            {
                                byte[] send = { (byte)currentPlayerIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,
                            40,40,0,0};
                                PublishData(send);
                            }
                        }
                    }
                    netreq.isHandelt = true;
                }
                else
                {
                    if (netreq.playersColor != currentPlayerIndex)
                    {
                        continue;
                    }
                    for (int i = 0; i < board.allPieces.Length; i++)
                    {
                        GamePiece gp = board.allPieces[i];
                        if (gp.color == data[0] && gp.startPos == data[1])
                        {
                            gp.position = data[3];
                                
                            if (currentPlayers[currentPlayerIndex].HandelTurn(gp))
                            {
                                if (currentPlayers[currentPlayerIndex].HasWon())
                                {
                                    ShowWinner();
                                    break; // do the winning thing!
                                }
                                MoveToNextPlayer();
                            }
                            byte[] send = {
                                (byte)currentPlayerIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,
                                data[0],data[1],0,(byte)gp.position
                            };
                            PublishData(send);
                            break;
                        }
                    }
                }
            }
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

        public override void Destroy()
        {
            base.Destroy();

            messageTimer.Dispose();
            Stop();
            foreach (network.NetworkReq clcn in servClient)
                clcn.Close();
        }
        public override void SlectPiece(GamePiece currentGamePiece)
        {
            int prevPos = currentGamePiece.position;
            int dieNum = currentPlayers[currentPlayerIndex].diceNumber;
            base.SlectPiece(currentGamePiece);

            byte[] send = {
                (byte)currentPlayerIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,
                (byte)currentGamePiece.color,(byte)currentGamePiece.startPos,(byte)dieNum,(byte)prevPos
            };
            PublishData(send);
        }
        public void Stop()
        {
            tcpListen.Stop();
        }
    }
}
