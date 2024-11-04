using MenschADN.assets;
using System;
using System.Collections.Generic;
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
                board.homeButtons[color, position - 40].BackgroundImage = ImageLoader.playerArr[color];
            }
        }
        public void Capture()
        {
            RemFromBoard();
            canMove = false;
            AddToBoard();
        }
        public void Move(int spaces)
        {
            RemFromBoard();
            if (!canMove)
            {
                if (spaces == 6)
                {
                    position = 0;
                    GamePiece gp = board.PlayerAtPos(realPos);
                    if (gp != null)
                        gp.Capture();
                    canMove = true;
                }
            }
            else
            {
                GamePiece gp = board.PlayerAtPos((position + position + color * 10) % 40);
                if (gp != null)
                    gp.Capture();
                position += spaces;
            }
            AddToBoard();
        }
        public bool IsInHouse() { return false; }
    }
}
