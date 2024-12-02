using MenschADN.assets;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.game
{
    public class GamePiece
    {
        internal int localIndex;
        internal int position = -1;
        public int projectedPos { get{ if (!canMove) { return color * 10; } else if (position < 40) return (position + color * 10) % 40; else return position; } }
        internal bool canMove;
        internal int color;
        internal bool isWinning { get { return position >= 40; } }
        internal GameBoard board;
        public GamePiece(GameBoard board,int startPos,int color)
        {
            this.localIndex = startPos;
            this.board = board;
            this.color = color;
        }
        internal void RemFromBoard()
        {
            if (!canMove)
            {
                // homepos
                board.startFields[color, localIndex].BackgroundImage = null;
                return;
            }
            if (position < 40)
            {
                board.tileButton[projectedPos].BackgroundImage = null;
                return;
            }
            else
            {
                board.homeButtons[color, position - 40].BackgroundImage = null;
            }
        }
        private void AddToBoard()
        {
            if (!canMove)
            {
                // homepos
                board.startFields[color, localIndex].BackgroundImage = ImageLoader.playerArr[color];
                return;
            }
            if (position < 40)
            {
                board.tileButton[projectedPos].BackgroundImage = ImageLoader.playerArr[color];
                return;
            }
            else
            {
                board.homeButtons[color, position - 40].BackgroundImage = ImageLoader.playerArr[color];
            }
        }
        public void Capture()
        {
            RemFromBoard();
            canMove = false;
            AddToBoard();
        }
        public bool Move(int spaces)
        {
            bool success = false;
            RemFromBoard();
            if (!canMove)
            {
                if (spaces == 6)
                {
                    position = 0;
                    canMove = true;
                    int ccpM = projectedPos;
                    canMove = false;
                    GamePiece gp = board.PlayerAtPos(ccpM);
                    if(gp != null && gp.color != this.color)
                    {
                        gp.Capture();
                        gp = null;
                    }
                    if(gp == null)
                    {
                        success = true;
                        canMove = true;
                    }
                }
            }
            else
            {
                position += spaces;
                int keepPos = projectedPos;
                position -= spaces;
                GamePiece gp = board.PlayerAtPos(keepPos);
                if (position + spaces >= 40) gp = null;
                if (gp != null && gp.color != this.color)
                {
                    gp.Capture();
                    gp = null;
                    success = true;
                }
                if (gp == null)
                {
                    if (position + spaces < 40)
                    {
                        success = true;
                        position += spaces;
                    }
                    else if(position + spaces < 44) 
                    {
                        int predictedPos = position + spaces - 40 ;
                        bool anyPieceInFront = false;
                        for (int over = Math.Max(40,position+1); over <= predictedPos+40; over++)
                        {
                            gp = board.PlayerInHome(over,color);
                            anyPieceInFront |= gp != null;
                        }
                        if (!anyPieceInFront)
                        {
                            success = true;
                            position += spaces;
                        }
                    }
                }
            }
            AddToBoard();
            return success;
        }
        public bool IsInHouse() { return false; }

        internal bool IsStuck(int diceNumber)
        {
            GamePiece gp;
            if (!canMove && diceNumber != 6)
                return true;
            if (!canMove && diceNumber == 6)
            {
                gp = board.PlayerAtPos(this.projectedPos);
                if (gp == null || gp.color != this.color) return false;
                return true;
            }
            if (position + diceNumber >= 44)
                return true;
            for (int over = Math.Max(40,position + 1); over <= diceNumber + position; over++)
            {
                gp = board.PlayerInHome(over, color);
                if (gp != null && gp.color == color)
                    return true;
            }
            position += diceNumber;
            int savePos = projectedPos;
            position -= diceNumber;
            gp = board.PlayerAtPos(savePos);
            if (gp == null || gp.color != this.color)
                return false;
            return true;
        }
        public bool ShowCanMove(int diceNumber)
        {
            if (!canMove && diceNumber != 6)
                return true;
            if (canMove && diceNumber == 6)
                return false;
            if (position + diceNumber >= 44)
                return true;
            for (int over = position + 1; over <= diceNumber + position; over++)
            {
                GamePiece gp = board.PlayerInHome(over, color);
                if (gp != null && gp.color == color)
                    return true;
            }
            return false;
        }
    }
}
