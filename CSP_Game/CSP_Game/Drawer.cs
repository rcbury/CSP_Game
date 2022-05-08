using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MyPhotoshop;
using MyPhotoshop.Data;

namespace CSP_Game
{
    public class Drawer
    {
        public void DrawObject(Color col, int offset, int x, int y, Photo map) 
        {
            for (int i = x - offset; i <= x + offset; i++)
                for (int j = y - offset; j <= y + offset; j++)
                    map[i, j] = new Pixel((double)col.R / 255, (double)col.G / 255, (double)col.B / 255);
        }
    }
}
