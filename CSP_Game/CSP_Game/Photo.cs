using MyPhotoshop.Data;
using System;

namespace MyPhotoshop
{
    public class Photo // является моделью, поскольку реализует логику, важную для вывода (отображение, View) всей информации на экран
                       // позволяет отрисовать в начале игры карту с необходимыми параметрами и позволяет обращаться к каждому пикселю через индексацию
    {
        public readonly int width;
        public readonly int height;
        public readonly int pixelWidth;
        public readonly int pixelHeight;
        private readonly Pixel[,] data;
        public Pixel this[int x, int y]
        {
            get
            {
                try
                {
                    return data[x, y];
                }
                catch (Exception)
                {
                    return data[0, 0];
                }
            }
            set { data[x, y] = value; }
        }

		public Photo(int width, int height)
		{
			this.width = width;
			this.height = height;
			this.pixelWidth = 25;
			this.pixelHeight = 25;
			data = new Pixel[width, height];
		}
	}
}