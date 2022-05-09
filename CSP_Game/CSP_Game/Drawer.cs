using MyPhotoshop;
using MyPhotoshop.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

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
        public static void ClearArea(int offset, int x, int y, Photo map)
        {
            var col = Color.FromArgb(255, 255, 255);
            for (int i = x - offset; i <= x + offset; i++)
                for (int j = y - offset; j <= y + offset; j++)
                    map[i, j] = new Pixel((double)col.R / 255, (double)col.G / 255, (double)col.B / 255);
        }
        public static Bitmap DrawMapWithIcons(List<Player> players, Bitmap image)
        {
            Bitmap icon;
            List<Tuple<Tuple<int, int>, AnyObject>> mapObjects = new List<Tuple<Tuple<int, int>, AnyObject>>();
            foreach (var item in players)
                foreach (var value in item.Mastery)
                    mapObjects.Add(new Tuple<Tuple<int, int>, AnyObject>(value.Key, value.Value));
            foreach (var item in mapObjects)
            {
                var x = item.Item1.Item1;
                var y = item.Item1.Item2;
                icon = new Bitmap(item.Item2.Icon);
                icon.MakeTransparent(Color.White);
                var xs = x * 20;
                var ys = y * 20;
                for (int i = 0; i < 20; i++)
                    for (int j = 0; j < 20; j++)
                    {
                        var px = icon.GetPixel(i, j);
                        if (px.R == 0 && px.G == 0 && px.B == 0)
                            px = image.GetPixel(xs + i, ys + j);
                        image.SetPixel(xs + i, ys + j, Color.FromArgb(px.R, px.G, px.B));
                    }
            }
            return image;
        }
    }
}
