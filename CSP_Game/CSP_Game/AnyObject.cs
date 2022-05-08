using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

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
    }
}
