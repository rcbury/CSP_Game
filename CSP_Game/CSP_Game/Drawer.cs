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
        public static void DrawObject(Color col, int offset, int x, int y, Photo map) 
        {
            for (int i = x - offset; i <= x + offset; i++)
                for (int j = y - offset; j <= y + offset; j++)
                    map[i, j] = new Pixel((double)col.R / 255, (double)col.G / 255, (double)col.B / 255);
        }

        public static void DrawObject(Color col, int offsetX, int offsetY, int x, int y, Photo map) 
        {
            for (int i = x - offsetX; i <= x + offsetX; i++)
                for (int j = y - offsetY; j <= y + offsetY; j++)
                    map[i, j] = new Pixel((double)col.R / 255, (double)col.G / 255, (double)col.B / 255);
        }

        public static Bitmap DrawMapWithIcons(List<Player> players, Bitmap image, int cellSize) 
        {
            Bitmap icon;
            List<Tuple<Tuple<int, int>,AnyObject,Color>> mapObjects = new List<Tuple<Tuple<int, int>, AnyObject, Color>>();
            foreach (var item in players)
                foreach (var value in item.Mastery)
                    mapObjects.Add(new Tuple<Tuple<int,int>, AnyObject, Color>(value.Key,value.Value,item.Color));
            foreach (var item in mapObjects) 
            {
                var x = item.Item1.Item1;
                var y = item.Item1.Item2;
                var col = item.Item3;
                icon = new Bitmap(item.Item2.Icon);
                var xs = x * cellSize;
                var ys = y * cellSize;
                for (int i = 0; i < cellSize; i++)
                    for (int j = 0; j < cellSize; j++)
                    {
                        var px = icon.GetPixel(i, j);
                        if (px.R == 255 && px.G == 255 && px.B == 255)
                            px = image.GetPixel(xs + i, ys + j);
                        else
                            px = col;
                        image.SetPixel(xs + i, ys + j, Color.FromArgb(px.R, px.G, px.B));
                    }
            }
            return image;
        }
    }
}
