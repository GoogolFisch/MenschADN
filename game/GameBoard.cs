using MenschADN.players;
using MenschADN.screens;
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
        internal Button[] tileButton; // do it like this? // or with another class?
        internal Button[,] homeButtons; // change with above
        internal Button[,] startFields; // change with above
        private Panel parentPannel;
        private GameScreen gameScreen;
        public GameBoard(GameScreen gmsc)
        {
            this.gameScreen = gmsc;
        }
        public void CreateTiles(Panel pan)
        {
            parentPannel = pan;

            // the walk around field.
            tileButton = new Button[40];
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
                    Tag = overBut,
                    Text = overBut.ToString(), // remove if seen fit XXX
                    BackgroundImageLayout = ImageLayout.Zoom,
                };
                int keepThisPos = overBut; // I hate how this works! why is everything on the heap!
                tileButton[overBut].Click += (o, e) => {
                    foreach (GamePiece pc in allPieces)
                    {
                        if (pc.projectedPos == keepThisPos && pc.color == gameScreen.currentColor && pc.canMove)
                        {
                            gameScreen.SlectPiece(pc);
                            return;
                        }
                    }
                    gameScreen.SlectPiece(null);
                };
                if (overBut % 10 == 0)
                    tileButton[overBut].BackColor = Apearence.playerColors[overBut / 10];
                pan.Controls.Add(tileButton[overBut]);
            }
            // the home area
            homeButtons = new Button[4, 4];
            Point[,] homePos = {
                { new Point(5,1), new Point(5,2), new Point(5,3), new Point(5,4) },
                { new Point(9,5), new Point(8,5), new Point(7,5), new Point(6,5) },
                { new Point(5,9), new Point(5,8), new Point(5,7), new Point(5,6) },
                { new Point(1,5), new Point(2,5), new Point(3,5), new Point(4,5) },
            };
            for (int overHome = 0; overHome < 4; overHome++)
            {
                for (int overBut = 0; overBut < 4; overBut++)
                {
                    homeButtons[overHome, overBut] = new Button()
                    {
                        Location = new Point(homePos[overHome,overBut].X * tileSize, homePos[overHome, overBut].Y * tileSize),
                        Size = new Size(tileSize, tileSize),
                        Tag = overBut,
                        Text = $"{overHome}:{overBut}", // remove if seen fit XXX
                        BackColor = Apearence.playerColors[overHome],
                        BackgroundImageLayout = ImageLayout.Zoom,
                    };
                    int keepThisPos = overBut + 40;
                    int keepThisColor = overHome;
                    homeButtons[overHome, overBut].Click += (o, e) => {
                        foreach (GamePiece pc in allPieces)
                        {
                            if (pc.projectedPos == keepThisPos &&
                            pc.color == keepThisColor && pc.color == gameScreen.currentColor)
                            {
                                gameScreen.SlectPiece(pc);
                                return;
                            }
                        }
                        gameScreen.SlectPiece(null);
                    };
                    pan.Controls.Add(homeButtons[overHome,overBut]);
                }
            }
            // the start area
            startFields = new Button[4, 4];
            Point[,] startPos = {
                { new Point(10,0), new Point(9,0), new Point(9,1), new Point(10,1) },
                { new Point(10,10), new Point(10,9), new Point(9,9), new Point(9,10) },
                { new Point(0,10), new Point(0,9), new Point(1,9), new Point(1,10) },
                { new Point(0,0), new Point(1,0), new Point(1,1), new Point(0,1) },
            };
            for (int overHome = 0; overHome < 4; overHome++)
            {
                for (int overBut = 0; overBut < 4; overBut++)
                {
                    startFields[overHome, overBut] = new Button()
                    {
                        Location = new Point(startPos[overHome, overBut].X * tileSize, startPos[overHome, overBut].Y * tileSize),
                        Size = new Size(tileSize, tileSize),
                        Tag = overBut,
                        Text = $"{overHome}:{overBut}", // remove if seen fit XXX
                        BackColor = Apearence.playerColors[overHome],
                        BackgroundImageLayout = ImageLayout.Zoom,
                    };
                    int keepThisPos = overBut;
                    int keepThisColor = overHome;
                    startFields[overHome, overBut].Click += (o, e) => {
                        foreach (GamePiece pc in allPieces)
                        {
                            if (
                            pc.projectedPos == keepThisPos &&
                            pc.color == gameScreen.currentColor &&
                            pc.color == keepThisColor &&
                            !pc.canMove
                            )
                            {
                                gameScreen.SlectPiece(pc);
                                return;
                            }
                        }
                        gameScreen.SlectPiece(null);
                    };
                    pan.Controls.Add(startFields[overHome, overBut]);
                }
            }
            // create players
            int index = 0;
            allPieces = new GamePiece[16];
            for (int overHome = 0; overHome < 4; overHome++)
            {
                for (int overPlayer = 0; overPlayer < 4; overPlayer++,index++)
                {
                    allPieces[index] = new GamePiece(this,overPlayer,overHome);
                    allPieces[index].Move(0);
                }
            }
        }
        public void DestroyTiles()
        {
            foreach (Button b in tileButton)
            {
                parentPannel.Controls.Remove(b);
                b.Dispose();
            }
            foreach (Button b in homeButtons)
            {
                parentPannel.Controls.Remove(b);
                b.Dispose();
            }
            foreach (Button b in startFields)
            {
                parentPannel.Controls.Remove(b);
                b.Dispose();
            }
        }

        internal GamePiece PlayerAtPos(int pos)
        {
            for (int i = 0; i < allPieces.Length; i++)
            {
                if (allPieces[i].projectedPos == pos && allPieces[i].canMove)
                {
                    return allPieces[i];
                }
            }
            return null;
        }
        internal GamePiece PlayerInHome(int pos,int color)
        {
            for (int i = 0; i < allPieces.Length; i++)
            {
                if (allPieces[i].projectedPos == pos && allPieces[i].canMove && allPieces[i].color == color)
                {
                    return allPieces[i];
                }
            }
            return null;
        }
    }
}
