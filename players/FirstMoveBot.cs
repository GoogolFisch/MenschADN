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
        public FirstMoveBot(GameScreen screen, int currentColor, string name)
            : base(screen, currentColor, name)
        {
            escapeTrys = 0;
        }
        public override GamePiece SelectGmPiece(GamePiece selectedGamePiece)
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
            if (atStartCount == 4)
                return bestPiece[0];
            for (int i = 0; i < bestPiece.Length; i++)
            {
                if(!bestPiece[i].IsStuck(diceNumber) && bestPiece[i].position == 0)
                    return bestPiece[i];
                if (!bestPiece[i].IsStuck(diceNumber) && !bestPiece[i].canMove)
                    return bestPiece[i];
            }
            for (int i = 0; i < bestPiece.Length; i++)
            {
                if (!bestPiece[i].IsStuck(diceNumber))
                    return bestPiece[i];
            }
            return bestPiece[0];
        }
        public override bool HandelTurn(GamePiece selectedGamePiece)
        {
            int atStartCount = 0;
            bool isStuck = true;
            GamePiece bestPiece = SelectGmPiece(selectedGamePiece);
            foreach (GamePiece overGamePiece in screen.board.allPieces)
            {
                if (overGamePiece.color == currentColor)
                {
                    if (!overGamePiece.canMove)
                        atStartCount++;
                    isStuck &= overGamePiece.IsStuck(diceNumber);
                }
            }
            if (atStartCount == 4)
            {
                if(EveryAtStart(bestPiece))
                    return true;
                if (bestPiece.canMove)
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
                if (PlayRace(bestPiece, atStartCount))
                    return true;
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
