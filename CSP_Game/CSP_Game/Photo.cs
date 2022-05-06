using System;
using MyPhotoshop.Data;

namespace MyPhotoshop
{
	public class Photo
	{
		public readonly int width;
		public readonly int height;
		public readonly int pixelWidth;
		public readonly int pixelHeight;
		private readonly Pixel[,] data;

		public Pixel this[int x, int y]
		{
			get { return data[x, y]; }
			set { data[x, y] = value; }
		}

		public Photo(int width, int height)
		{
			this.width = width;
			this.height = height;
			this.pixelWidth = 15;
			this.pixelHeight = 15;
			data = new Pixel[width, height];
		}
	}
}