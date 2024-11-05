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
        public int realPos { get{ if (position < 40) return (position + color * 10) % 40; else return position; } }
        internal bool canMove;
        internal int color;
        internal bool isWinning;
        internal GameBoard board;
        public GamePiece(GameBoard board,int startPos,int color)
        {
            this.startPos = startPos;
            this.board = board;
            this.color = color;
            isWinning = false;
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
                board.tileButton[realPos].BackgroundImage = null;
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
                board.tileButton[realPos].BackgroundImage = ImageLoader.playerArr[color];
                return;
            }
            else
            {
                isWinning = false;
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
                    GamePiece gp = board.PlayerAtPos(realPos);
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
                GamePiece gp = board.PlayerAtPos((position + spaces + color * 10) % 40);
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
                        for (int over = position; over < predictedPos; over++)
                        {
                            gp = board.PlayerInHome(predictedPos,color);
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
    }
}
