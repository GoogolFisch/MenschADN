using MenschADN.game;
using MenschADN.screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.players
{
    public class LocalPlayer : Player
    {
        int escapeTrys;
        public LocalPlayer(GameScreen screen,int currentColor) : base(screen, currentColor)
        {
            escapeTrys = 0;
        }
        public override void StartTurn()
        {
            escapeTrys = 0;
            ThrowDie();
        }
        public override void ThrowDie()
        {
            diceNumber = Random.Shared.Next(1, 7);
        }
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
        public bool PlayRace(GamePiece piece,int atStart)
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
                infrontPiece != null && infrontPiece.color == currentColor && atStart >0
                )
            {
                succ = piece.Move(diceNumber);
                if(succ) ThrowDie();
            }
            else if (piece.position == 0)
            {
                succ = piece.Move(diceNumber);
                if (succ) ThrowDie();
            }
            else if(atStart > 0 && entryPoint != null && entryPoint.color == currentColor)
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
                if(succ) ThrowDie();
            }
            return succ && !isMagic;
        }
        public override bool HandelTurn(GamePiece selectedGamePiece)
        {
            int atStartCount = 0;
            bool isStuck = true;
            foreach (GamePiece overGamePiece in screen.board.allPieces)
            {
                if (overGamePiece.color == currentColor)
                {
                    if (!overGamePiece.canMove)
                        atStartCount++;
                    isStuck &= overGamePiece.IsStuck(diceNumber);
                }
            }
            if (atStartCount == 4) { return EveryAtStart(selectedGamePiece); }
            else if (isStuck) { return true; }
            else return PlayRace(selectedGamePiece, atStartCount);
            //return true;
        }
    }
}
