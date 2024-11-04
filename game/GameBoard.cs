﻿using MenschADN.players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.game
{
    public class GameBoard
    {
        internal GamePiece[] allPieces;
        internal Player[] players;
        static int tileSize = 64;
        int diceNumber;
        Button[] tileButton; // do it like this? // or with another class?
        Button[,] homeButtons; // change with above
        private Panel parentPannel;
        public void CreateTiles(Panel pan)
        {
            parentPannel = pan;
            // 48?
            int counter = 0;
            tileButton = new Button[40];
            /*
            tileButton[counter++] = new Button()
            {
                Location = new Point(4 * tileSize, 0),
            };
            tileButton[counter++] = new Button()
            {
                Location = new Point(5 * tileSize, 0),
            };
            /**/
            Point[] loc = {
                // start of a
                new Point(6,0),new Point(6,1),new Point(6,2),new Point(6,3),
                //corner A
                new Point(6,4),new Point(7,4),new Point(8,4),new Point(9,4),new Point(10,4),new Point(10,5),

                // start of b
                new Point(10,6),new Point(9,6),new Point(8,6),new Point(7,6),
                //corner B
                new Point(6,6),new Point(6,7),new Point(6,8),new Point(6,9),new Point(6,10),new Point(5,10),

                // start of c
                new Point(4,10),new Point(4,9),new Point(4,8),new Point(4,7),
                //corner C
                new Point(4,6),new Point(3,6),new Point(2,6),new Point(1,6),new Point(0,6),new Point(0,5),

                // start of d
                new Point(0,4),new Point(1,4),new Point(2,4),new Point(3,4),
                //corner D
                new Point(4,4),new Point(4,3),new Point(4,2),new Point(4,1),new Point(4,0),new Point(5,0),
            };
            for(int overBut = 0;overBut < tileButton.Length;overBut++)
            {
                tileButton[overBut] = new Button()
                {
                    Location = new Point(loc[overBut].X * tileSize, loc[overBut].Y * tileSize),
                    Size = new Size(tileSize,tileSize),
                    Text = overBut.ToString(), // remove if seen fit XXX
                };
                pan.Controls.Add(tileButton[overBut]);
            }
            Point[,] homePos = {
                { new Point(5,1), new Point(5,2), new Point(5,3), new Point(5,4) },
                { new Point(9,5), new Point(8,5), new Point(7,5), new Point(6,5) },
                { new Point(5,9), new Point(5,8), new Point(5,7), new Point(5,6) },
                { new Point(1,5), new Point(2,5), new Point(3,5), new Point(4,5) },
            };
            homeButtons = new Button[4, 4];
            for (int overHome = 0; overHome < 4; overHome++)
            {
                for (int overBut = 0; overBut < 4; overBut++)
                {
                    homeButtons[overHome, overBut] = new Button()
                    {
                        Location = new Point(homePos[overHome,overBut].X * tileSize, homePos[overHome, overBut].Y * tileSize),
                        Size = new Size(tileSize, tileSize),
                        Text = $"{overHome}:{overBut}", // remove if seen fit XXX
                    };
                    pan.Controls.Add(homeButtons[overHome,overBut]);
                }
            }
        }
        public void DestroyTiles()
        {
            foreach (Button b in tileButton)
            {
                parentPannel.Controls.Add(b);
                b.Dispose();
            }
        }
    }
}
