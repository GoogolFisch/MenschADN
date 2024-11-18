using MenschADN.game;
using MenschADN.screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.players
{
    public class FirstMoveBot : Player
    {
        int escapeTrys;
        public FirstMoveBot(GameScreen screen, int currentColor) : base(screen, currentColor)
        {
            escapeTrys = 0;
        }
        public override bool HandelTurn(GamePiece selectedGamePiece)
        {
            int atStartCount = 0;
            bool isStuck = true;
            GamePiece[] bestPiece = new GamePiece[4];
            int bestPieceCount = 0;
            foreach (GamePiece overGamePiece in screen.board.allPieces)
            {
                if (overGamePiece.color == currentColor)
                {
                    bestPiece[bestPieceCount++] = overGamePiece;
                    if (!overGamePiece.canMove)
                        atStartCount++;
                    isStuck &= overGamePiece.IsStuck(diceNumber);
                }
            }
            for (int i = 0; i < bestPiece.Length;i++)
            {
                GamePiece gamePiece = bestPiece[i];
                if (atStartCount == 4)
                {
                    if(EveryAtStart(gamePiece))
                        return true;
                    if (gamePiece.canMove)
                    {
                        atStartCount--;
                        return false;
                    }
                }
                else if (isStuck)
                {
                    return true;
                }
                else
                {
                    if (PlayRace(gamePiece, atStartCount))
                        return true;
                }
            }
            return false;
            //return true;
        }

        public override bool IsBot()
        {
            return true;
        }
    }
}
