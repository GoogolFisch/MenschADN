﻿using MenschADN.players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public class StartScreen : Screen
    {
        Font titleFont;
        Label title;
        Button startGame;
        Button startAsServer;
        Button startAsClient;
        TextBox serverAddress;
        Button helpMe;
        FlowLayoutPanel playerSelect;
        static PlayerCreator[] plCreate = new PlayerCreator[4];
        static string serverAddressString = network.NetworkReq.LOCAL;
        public StartScreen(Displayer parent,Screen parentScreen) : base(parent, parentScreen)
        {
            for (int i = 0; i < plCreate.Length; i++)
            {
                plCreate[i] = new PlayerCreator();
            }
        }

        public override void Create()
        {
            titleFont = new Font(FontFamily.GenericSerif, 24, FontStyle.Bold);
            title = new Label
            {
                Font = titleFont,
                AutoSize = true,
                Text = "Mensch Ärger Dich Nicht"
            };
            this.parentForm.Controls.Add(title);
            startGame = new Button()
            {
                AutoSize = true,
                Text = "Start",
                Font = titleFont,
                //Location = new Point(15,15),
            };
            startGame.Click += ChangeToGame;
            parentForm.Controls.Add(startGame);
            helpMe = new Button()
            {
                AutoSize = true,
                Text = "Help",
                Font = titleFont,
            };
            helpMe.Click += GotoHelpScreen;
            parentForm.Controls.Add(helpMe);
            // server
            startAsServer = new Button()
            {
                AutoSize = true,
                Text = "Start Server",
                Font = titleFont,
            };
            startAsServer.Click += ChangeToServerGame;
            parentForm.Controls.Add(startAsServer);
            // client
            startAsClient = new Button()
            {
                AutoSize = true,
                Text = "Connect to Server",
                Font = titleFont,
            };
            startAsClient.Click += ChangeToClientGame;
            parentForm.Controls.Add(startAsClient);
            serverAddress = new TextBox()
            {
                Size = new Size(80,20),
                Text = serverAddressString
            };
            parentForm.Controls.Add(serverAddress);
            // player stuff
            playerSelect = new FlowLayoutPanel()
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Location = new Point(0,50),
            };
            parentForm.Controls.Add(playerSelect);
            for (int i = 0; i < plCreate.Length; i++)
            {
                playerSelect.Controls.Add(plCreate[i].GetPanel(i));
            }
            //this.parentForm.ResizeEnd += ;
        }
        private void ChangeToGame(object sender, EventArgs e)
        {
            Player[] plList = new Player[plCreate.Length];
            screens.GameScreen sc = new GameScreen(parentForm, this);
            for (int i = 0; i < plList.Length; i++)
            {
                plList[i] = plCreate[i].MakePlayer(sc);
            }
            sc.GivePlayers(plList);
            parentForm.ChangeScreen(sc);
        }
        private void GotoHelpScreen(object sender, EventArgs e)
        {
            HelpScreen sc = new HelpScreen(parentForm,this);
            parentForm.ChangeScreen(sc);
        }
        private void ChangeToServerGame(object sender, EventArgs e)
        {
            Player[] plList = new Player[plCreate.Length];
            screens.ServerGameScreen sc = new ServerGameScreen(parentForm, this);
            for (int i = 0; i < plList.Length; i++)
            {
                plList[i] = plCreate[i].MakeServerPlayer(sc);
            }
            parentForm.ChangeScreen(sc);
            sc.GivePlayers(plList);
        }
        private void ChangeToClientGame(object sender, EventArgs e)
        {
            screens.ClientGameScreen sc = new ClientGameScreen(parentForm, this);
            //sc.address = "blab";
            serverAddressString = serverAddress.Text;
            sc.SetServerAddress(serverAddress.Text);
            parentForm.ChangeScreen(sc);
        }

        public override void Destroy()
        {
            parentForm.Controls.Remove(title);
            title.Dispose();
            parentForm.Controls.Remove(startGame);
            startGame.Dispose();
            parentForm.Controls.Remove(helpMe);
            helpMe.Dispose();

            parentForm.Controls.Remove(startAsClient);
            startAsClient.Dispose();
            parentForm.Controls.Remove(serverAddress);
            serverAddress.Dispose();

            parentForm.Controls.Remove(startAsServer);
            startAsServer.Dispose();

            parentForm.Controls.Remove(playerSelect);
            playerSelect.Dispose();
            // remove font!
            titleFont.Dispose();
            for (int i = 0; i < plCreate.Length; i++)
            {
                playerSelect.Controls.Remove(plCreate[i].GetPanel());
            }
        }

        public override void Resize(object? sender, EventArgs e)
        {
            title.Location = new Point((parentForm.Width - title.Width) / 2, 15);
            startGame.Location = new Point((parentForm.Width - startGame.Width) / 2, parentForm.Height / 5);
            helpMe.Location = new Point((parentForm.Width - helpMe.Width) / 2, startGame.Location.Y + (int)(startGame.Height * 1.25));
            startAsClient.Location = new Point((parentForm.Width - startAsClient.Width) / 2, (parentForm.Height / 5) * 3);
            serverAddress.Location = new Point((parentForm.Width - startAsClient.Width) / 2, startAsClient.Location.Y - (int)(serverAddress.Height * 1.25));
            startAsServer.Location = new Point((parentForm.Width - startAsServer.Width) / 2, startAsClient.Location.Y + (int)(startAsClient.Height * 1.25));
        }
        public void AddScore(int color)
        {
            plCreate[color].AddScore();
        }
    }
}
