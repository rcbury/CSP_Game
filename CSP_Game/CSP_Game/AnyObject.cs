using System;
using System.Drawing;

namespace CSP_Game
{
    public class AnyObject
    {
        public Tuple<int, int> Position;
        public int Border { get; set; }
        public double HP { get; set; }
        public double FullHP { get; set; }
        public Color Color { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public Image Icon { get; set; }
        public int Rent { get; set; }
    }
}
