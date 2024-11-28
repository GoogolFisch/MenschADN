using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MenschADN.game;
using MenschADN.players;
using System.Diagnostics;

namespace MenschADN.screens
{
    public class ServerGameScreen : GameScreen
    {
        internal System.Windows.Forms.Timer messageTimer;
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
                    for(int i = 0; i < currentPlayers.Length;i++)
                    {
                        if (currentPlayers[i].GetType() != typeof(ServerPlayer)) continue;
                        var cpl = (ServerPlayer)currentPlayers[i];
                        if (cpl == null) continue;
                        if (cpl.specificClient != null && cpl.specificClient.IsActive()) continue;
                        cpl.specificClient = netreq;
                        byte[] send = new byte[]{ (byte)i,(byte)currentPlayerIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,
                        40,40,0,0};
                        netreq.SendStream(send);
                        for (int ei = 0; ei < send.Length; ei++)
                            Debug.Write(send[ei] + ",");
                        Debug.WriteLine("start-send");
                        netreq.playersColor = i;
                        netreq.isHandelt = true;
                        break;
                    }
                    if (!netreq.isHandelt)
                    {
                        byte[] send = new byte[]{ 200,(byte)currentPlayerIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,
                        40,40,0,0};
                        netreq.SendStream(send);
                        for (int ei = 0; ei < send.Length; ei++)
                            Debug.Write(send[ei] + ",");
                        Debug.WriteLine("over-start");
                    }
                }
                else
                {
                    for (int ei = 0; ei < data.Length; ei++)
                        Debug.Write(data[ei] + ",");
                    Debug.WriteLine("recv from"+overClient);

                    if (netreq.playersColor != currentPlayerIndex)
                    {
                        continue;
                    }
                    for (int i = 0; i < board.allPieces.Length; i++)
                    {
                        GamePiece gp = board.allPieces[i];
                        if (gp.color == data[0] && gp.localIndex == data[1])
                        {
                            //gp.position = data[3];
                            int oldDieNum = currentPlayers[currentPlayerIndex].diceNumber;
                            int oldPiecePos;
                            if (gp.canMove)
                                oldPiecePos = gp.position;
                            else oldPiecePos = -1;
                                
                            if (currentPlayers[currentPlayerIndex].HandelTurn(gp))
                            {
                                if (currentPlayers[currentPlayerIndex].HasWon())
                                {
                                    ShowWinner();
                                    break; // do the winning thing!
                                }
                                MoveToNextPlayer();
                            }
                            if(oldPiecePos == gp.position) { break; }
                            byte[] send = {
                                (byte)currentPlayerIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,
                                data[0],data[1],(byte)oldDieNum,(byte)oldPiecePos
                            };
                            for (int ei = 0; ei < send.Length; ei++)
                                Debug.Write(send[ei] + ",");
                            Debug.WriteLine("publish");
                            PublishData(send);
                            UpdateCurrentDisplay();
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
                    Debug.WriteLine($"servClintList:{servClient.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Something went wrong with the client accepting!");
                    Debug.WriteLine(ex.ToString());
                    Debug.WriteLine("Something went wrong with the client accepting!");
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
            if (currentGamePiece == null) return;
            int prevPos = currentGamePiece.position;
            int dieNum = currentPlayers[currentPlayerIndex].diceNumber;
            base.SlectPiece(currentGamePiece);

            byte[] send = {
                (byte)currentPlayerIndex, (byte)currentPlayers[currentPlayerIndex].diceNumber,
                (byte)currentGamePiece.color,(byte)currentGamePiece.localIndex,(byte)dieNum,(byte)prevPos
            };
            PublishData(send);
        }
        public void Stop()
        {
            tcpListen.Stop();
        }
    }
}
