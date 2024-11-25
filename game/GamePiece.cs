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
        internal int startPos;
        internal int position;
        public int projectedPos { get{ if (!canMove) { return startPos; } else if (position < 40) return (position + color * 10) % 40; else return position; } }
        internal bool canMove;
        internal int color;
        internal bool isWinning { get { return position >= 40; } }
        internal GameBoard board;
        public GamePiece(GameBoard board,int startPos,int color)
        {
            this.startPos = startPos;
            this.board = board;
            this.color = color;
        }
        private void RemFromBoard()
        {
            if (!canMove)
            {
                // homepos
                board.startFields[color, startPos].BackgroundImage = null;
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
                board.startFields[color, startPos].BackgroundImage = ImageLoader.playerArr[color];
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
            if (!canMove && diceNumber != 6)
                return true;
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
