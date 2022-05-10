using MyPhotoshop.Data;

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
            get { return data[x, y]; }
            set { data[x, y] = value; }
        }

        public Photo(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.pixelWidth = 20;
            this.pixelHeight = 20;
            data = new Pixel[width, height];
        }
    }
}