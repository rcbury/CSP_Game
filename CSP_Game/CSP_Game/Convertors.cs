using System;
using System.Drawing;

namespace CSP_Game
{
    public static class Convertors // является моделью, поскольку реализует одну из главных задач - конвертирует картинку в массив пикселей заданного размера
    {
        public static Photo Bitmap2Photo(Bitmap bmp)
        {
            var photo = new Photo(bmp.Width, bmp.Height);
            int width = bmp.Width / photo.pixelWidth;
            int height = bmp.Height / photo.pixelHeight;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    var pixel = bmp.GetPixel(x * photo.pixelWidth + 1, y * photo.pixelHeight + 1);
                    photo[x, y] = new Pixel((double)pixel.R / 255, (double)pixel.G / 255,
                        (double)pixel.B / 255);
                }
            return photo;
        }

        static int ToChannel(double val)
        {
            if (val < 0 || val > 1)
                throw new Exception(string.Format("Wrong channel value {0} (the value must be between 0 and 1", val));
            return (int)(val * 255);
        }

        public static Bitmap Photo2Bitmap(Photo photo)
        {
            var bmp = new Bitmap(photo.width * photo.pixelWidth, photo.height * photo.pixelHeight);
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                    if (x % photo.pixelWidth == 0 || y % photo.pixelHeight == 0)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, Color.FromArgb(
                        ToChannel(photo[(int)Math.Floor((double)x / photo.pixelWidth),
                        (int)Math.Floor((double)y / photo.pixelHeight)].R),
                        ToChannel(photo[(int)Math.Floor((double)x / photo.pixelWidth),
                        (int)Math.Floor((double)y / photo.pixelHeight)].G),
                        ToChannel(photo[(int)Math.Floor((double)x / photo.pixelWidth),
                        (int)Math.Floor((double)y / photo.pixelHeight)].B)));
                    }
            return bmp;
        }
    }
}

