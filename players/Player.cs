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

        internal int escapeTrys;

        public Player(GameScreen screen, int currentColor)
        {
            escapeTrys = 0;
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
        public abstract bool IsBot();
        public void StartTurn()
        {
            escapeTrys = 0;
            ThrowDie();
        }

        public void ThrowDie()
        {
            diceNumber = Random.Shared.Next(1, 7);
        }
        public abstract bool HandelTurn(GamePiece selectedGamePiece); // return if the turn should end

        public bool EveryAtStart(GamePiece piece)
        {
            escapeTrys++;
            if (diceNumber == 6)
            {
                piece.Move(6);
                ThrowDie();
                return false;
            }
            ThrowDie();
            if (escapeTrys >= 3) return true;
            return false;
        }
        public bool PlayRace(GamePiece piece, int atStart)
        {
            GamePiece infrontPiece = screen.board.PlayerAtPos(10 * currentColor + diceNumber);
            GamePiece entryPoint = screen.board.PlayerAtPos(10 * currentColor);
            bool succ = false;
            bool isMagic = diceNumber == 6;
            if (isMagic && !piece.canMove)
            {
                succ = piece.Move(6);
                if (succ) ThrowDie();
                return false;
            }
            // do stuff herre!
            else if (
                entryPoint != null && entryPoint.color == currentColor &&
                infrontPiece != null && infrontPiece.color == currentColor && atStart > 0
                )
            {
                succ = piece.Move(diceNumber);
                if (succ) ThrowDie();
            }
            else if (piece.position == 0)
            {
                succ = piece.Move(diceNumber);
                if (succ) ThrowDie();
            }
            else if (atStart > 0 && entryPoint != null && entryPoint.color == currentColor)
            {
                return false;
            }
            else if (isMagic && atStart > 0)
            {
                return false;
            }
            else if (isMagic && atStart == 0)
            {
                succ = piece.Move(6);
                if (succ) ThrowDie();
            }
            else
            {
                succ = piece.Move(diceNumber);
                if (succ) ThrowDie();
            }
            return succ && !isMagic;
        }
    }
}
