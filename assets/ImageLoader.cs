using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.assets
{
    public class ImageLoader
    {
        static string filePath = "..\\..\\..\\assets\\";
        private static Image LoadImg(string name) => Image.FromFile(filePath + name);
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
