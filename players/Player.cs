using MenschADN.game;
using MenschADN.screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.players
{
    public abstract class Player
    {
        // Object gamePiece;
        // IDK

        internal int diceNumber = 0;
        internal GameScreen screen;
        internal int currentColor = 0;

        public Player(GameScreen screen, int currentColor)
        {
            this.screen = screen;
            this.currentColor = currentColor;
        }
        public bool HasWon()
        {
            bool allWinning = true;
            foreach (GamePiece pc in screen.board.allPieces)
            {
                if (pc.color == currentColor)
                    allWinning &= pc.isWinning;
            }
            return allWinning;
        }
        public abstract void StartTurn();

        public abstract void ThrowDie();
        public abstract bool HandelTurn(GamePiece selectedGamePiece); // return if the turn should end
    }
}
