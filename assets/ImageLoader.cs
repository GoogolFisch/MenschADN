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
        public static Image redPlayer = Image.FromFile("red.png");
        public static Image yellowPlayer = Image.FromFile("yellow.png");
        public static Image greenPlayer = Image.FromFile("green.png");
        public static Image bluePlayer = Image.FromFile("blue.png");
    }
}
