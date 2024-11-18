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
        internal GameBoard board;
        Panel gameBoardPan;
        Label diceNumber;

        int totalTrys = 0;

        public int currentPlayerIndex = -1;
        public int currentColor { get { return currentPlayers[currentPlayerIndex].currentColor; } }

        public GameScreen(Displayer parent,Screen parentScreen) : base(parent,parentScreen)
        {
            currentPlayers = new LocalPlayer[4];
            currentPlayers[0] = new LocalPlayer(this, 0);
            currentPlayers[1] = new LocalPlayer(this, 1);
            currentPlayers[2] = new LocalPlayer(this, 2);
            currentPlayers[3] = new LocalPlayer(this, 3);
            this.MoveToNetPlayer();
        }

        public override void Create()
        {
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
            // before the game starts!
            UpdateCurrentDisplay();
        }

        public override void Destroy()
        {
            parentForm.Controls.Remove(diceNumber);
            diceNumber.Dispose();
            board.DestroyTiles();
            // at the end--
            parentForm.Controls.Remove(gameBoardPan);
            gameBoardPan.Dispose();
        }
        public void UpdateCurrentDisplay()
        {
            diceNumber.Text = $"{currentPlayers[currentPlayerIndex].diceNumber}-{currentPlayerIndex}";
        }
        public override void Resize(object? sender, EventArgs e)
        {
            gameBoardPan.Location = new Point((parentForm.Width - gameBoardPan.Width) / 2, (parentForm.Height - gameBoardPan.Height) / 2);
            diceNumber.Location = new Point((parentForm.Width - diceNumber.Width) / 2, (parentForm.Height - gameBoardPan.Height) / 2 - diceNumber.Height);
        }
        public void MoveToNetPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            while (currentPlayers[currentPlayerIndex % 4] == null)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            }
            currentPlayers[currentPlayerIndex].StartTurn();
        }
        public void SlectPiece(GamePiece currentGamePiece)
        {
            if (currentGamePiece == null)
                return;

            if (currentPlayers[currentPlayerIndex].HandelTurn(currentGamePiece))
            {
                if (currentPlayers[currentPlayerIndex].HasWon())
                {
                    throw new Exception("you shall win!");
                }
                else
                    MoveToNetPlayer();
            }
            UpdateCurrentDisplay();
            //currentGamePiece.Move(6);
            /* // this needs to be bullet-proof...
            // test if any can move?
            bool canOtherPiecesMove = false;
            foreach(GamePiece overGamePiece in board.allPieces)
            {
                if(overGamePiece.color == currentColor)
                    canOtherPiecesMove |= overGamePiece.canMove;
            }
            // test the move
            if (canOtherPiecesMove && !currentGamePiece.canMove && rolledNumber != 6)
                // you keep your turn...?
                return;
            totalTrys++;
            if (!currentGamePiece.Move(rolledNumber))
            {
                //reroll if nothing can move!
                rolledNumber = Random.Shared.Next(1, 7);
                if(totalTrys < 4 && !canOtherPiecesMove && !currentGamePiece.canMove)
                    return;
            }
            if (rolledNumber == 6 || (canOtherPiecesMove && totalTrys <= 3))
            {
                totalTrys = 0;
            }
            else
            {
                currentColor = (currentColor + 1) % 4;
            }
            rolledNumber = Random.Shared.Next(1, 7);
            diceNumber.Text = $"{rolledNumber}-{currentColor}"; // XXX please change
            */
        }
    }
}
