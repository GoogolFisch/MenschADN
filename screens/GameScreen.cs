using MenschADN.game;
using MenschADN.players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.screens
{
    public class GameScreen : Screen
    {
        internal Player[] currentPlayers;
        internal GameBoard board;
        internal Panel gameBoardPan;
        internal Label diceNumber;
        internal Label winnerDisplay;
        internal Button returning;
        internal Font hugeFont;
        internal System.Windows.Forms.Timer ticker;

        internal int totalTrys = 0;

        public int currentPlayerIndex = -1;
        public int currentColor { get { return currentPlayers[currentPlayerIndex].currentColor; } }
        public GameScreen(Displayer parent,Screen parentScreen) : base(parent,parentScreen)
        {
            /*currentPlayers = new LocalPlayer[4];
            currentPlayers[0] = new LocalPlayer(this, 0);
            currentPlayers[1] = new LocalPlayer(this, 1);
            currentPlayers[2] = new LocalPlayer(this, 2);
            currentPlayers[3] = new LocalPlayer(this, 3);
            this.MoveToNetPlayer();/**/
        }
        public virtual void GivePlayers(Player[] pl)
        {
            currentPlayers = pl;
            currentPlayerIndex = -1;
            this.MoveToNetPlayer();
            UpdateCurrentDisplay();
        }
        public override void Create()
        {
            ticker = new System.Windows.Forms.Timer() { Interval = 750,Enabled = false };
            ticker.Tick += BotMove;
            hugeFont = new Font(FontFamily.GenericSerif, 12);
            board = new GameBoard(this);
            gameBoardPan = new Panel()
            {
                AutoSize = true,
            };
            board.CreateTiles(gameBoardPan);
            parentForm.Controls.Add(gameBoardPan);
            diceNumber = new Label()
            {
                Text = "???",
                AutoSize = true,
            };
            parentForm.Controls.Add(diceNumber);
            returning = new Button()
            {
                Text = "exit",
                Location = new Point(10, 10)
            };
            returning.Click += GetChangeBackScreen;
            parentForm.Controls.Add(returning);
            // before the game starts!
            UpdateCurrentDisplay();
        }

        public void ShowWinner()
        {
            winnerDisplay = new Label()
            {
                AutoSize = true,
                Text = $"{currentColor} has Won!",
                Font = hugeFont
            };
            parentForm.Controls.Add(winnerDisplay);
            if (oldScreen.GetType() == typeof(StartScreen))
            {
                ((StartScreen)oldScreen).AddScore(currentColor);
            }
        }


        public override void Destroy()
        {
            if (winnerDisplay != null)
            {
                parentForm.Controls.Remove(winnerDisplay);
                winnerDisplay.Dispose();
            }
            parentForm.Controls.Remove(returning);
            returning.Dispose();

            parentForm.Controls.Remove(diceNumber);
            diceNumber.Dispose();
            board.DestroyTiles();
            // at the end--
            parentForm.Controls.Remove(gameBoardPan);
            gameBoardPan.Dispose();
        }
        public void UpdateCurrentDisplay()
        {
            if(currentPlayerIndex != -1)
                diceNumber.Text = $"{currentPlayers[currentPlayerIndex].diceNumber}-{currentPlayerIndex}";
        }
        private void GetChangeBackScreen(object? sender, EventArgs e)
        {
            parentForm.ChangeScreen(oldScreen);
        }
        public override void Resize(object? sender, EventArgs e)
        {
            // center
            gameBoardPan.Location = new Point((parentForm.Width - gameBoardPan.Width) / 2, (parentForm.Height - gameBoardPan.Height) / 2);
            diceNumber.Location = new Point((parentForm.Width - diceNumber.Width) / 2, (parentForm.Height - gameBoardPan.Height) / 2 - diceNumber.Height);
            // lower right
            returning.Location = new Point(parentForm.Width - returning.Width-50, parentForm.Height - returning.Height-50);
        }
        public void MoveToNetPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            while (currentPlayers[currentPlayerIndex % 4] == null)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            }
            currentPlayers[currentPlayerIndex].StartTurn();
            if (currentPlayers[currentPlayerIndex].IsBot() && ticker != null)
            {
                ticker.Start();
            }
        }
        private void BotMove(object? sender, EventArgs e)
        {
            if (currentPlayers[currentPlayerIndex].HandelTurn(null))
            {
                ticker.Stop();
                if (currentPlayers[currentPlayerIndex].HasWon())
                {
                    ShowWinner();
                }
                else
                    MoveToNetPlayer();
            }
            UpdateCurrentDisplay();
        }
        public void SlectPiece(GamePiece currentGamePiece)
        {
            if (currentGamePiece == null)
                return;

            if (currentPlayers[currentPlayerIndex].HandelTurn(currentGamePiece))
            {
                if (currentPlayers[currentPlayerIndex].HasWon())
                {
                    ShowWinner();
                }
                else
                    MoveToNetPlayer();
            }
            UpdateCurrentDisplay();
        }
    }
}
