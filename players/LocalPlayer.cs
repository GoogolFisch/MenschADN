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
        public LocalPlayer(GameScreen screen,int currentColor,string name)
            : base(screen, currentColor,name)
        {
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
            if (atStartCount == 4) 
                return EveryAtStart(selectedGamePiece);
            else if (isStuck) 
                return true;
            else
                return PlayRace(selectedGamePiece, atStartCount);
            //return true;
        }
        public override bool IsBot()
        {
            return false;
        }
    }
}
