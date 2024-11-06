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
        Player[] currentPlayers;
        GameBoard board;
        Panel gameBoardPan;
        Label diceNumber;

        public GameScreen(Displayer parent,Screen parentScreen) : base(parent,parentScreen)
        {
        }

        public override void Create()
        {
            board = new GameBoard();
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
            gameBoardPan.Controls.Add(diceNumber);
        }

        public override void Destroy()
        {
            gameBoardPan.Controls.Remove(diceNumber);
            diceNumber.Dispose();
            board.DestroyTiles();
            // at the end--
            parentForm.Controls.Remove(gameBoardPan);
            gameBoardPan.Dispose();
        }

        public override void Resize(object? sender, EventArgs e)
        {
            gameBoardPan.Location = new Point((parentForm.Width - gameBoardPan.Width) / 2, (parentForm.Height - gameBoardPan.Height) / 2);
        }
    }
}
