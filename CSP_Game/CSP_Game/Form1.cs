using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyPhotoshop.Data;
using MyPhotoshop;

namespace CSP_Game
{
    public partial class Form1 : Form
    {
        Photo map;

        public void InitializeMap() 
        {
            map = new Photo(50, 30);
            pictureBox1.Height = map.height * map.pixelHeight;
            pictureBox1.Width = map.width * map.pixelWidth;
            for (int x = 0; x < map.width; x++)
                for (int y = 0; y < map.height; y++) 
                {
                    map[x, y] = new Pixel(1, 1, 1);
                }
        }

        public Form1()
        {
            InitializeComponent();
            InitializeMap();
            pictureBox1.Image = Convertors.Photo2Bitmap(map);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int x = (int)Math.Floor((double)Cursor.Position.X / map.pixelWidth);
            int y = (int)Math.Floor((double)(Cursor.Position.Y - 30)/ map.pixelHeight);
            map[x, y] = new Pixel(0, 1, 0);
            pictureBox1.Image = Convertors.Photo2Bitmap(map);
        }
    }
}
