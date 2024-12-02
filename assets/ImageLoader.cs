using System;
using System.Diagnostics;

namespace MenschADN.assets
{
    public class ImageLoader
    {
        static string filePath = "\\assets\\";
        static string backPath = "..\\..\\..";
        private static Image LoadImg(string name) {
            Image img;
            img = Image.FromFile(backPath + filePath + name);
            if(img == null) {
                img = Image.FromFile("." + filePath + name);
            }
            return img;
        }
        public static Image redPlayer = LoadImg("red.png");
        public static Image yellowPlayer = LoadImg("yellow.png");
        public static Image greenPlayer = LoadImg("green.png");
        public static Image bluePlayer = LoadImg("blue.png");
        public static Image[] playerArr = new Image[] { redPlayer, yellowPlayer, greenPlayer, bluePlayer };

        public ImageLoader()
        {
        }
    }
}
