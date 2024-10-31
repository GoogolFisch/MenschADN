using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.game
{
    public class GamePiece
    {
        internal int position;
        internal bool canMove;
        internal int color;
        public void Capture() { }
        public void Move(int spaces) { }
        public bool IsInHouse() { return false; }
    }
}
