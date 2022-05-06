using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public class Unit : AnyObject
    {
        public Tuple<int, int> Position;
        public int Range { get; set; }
        public double Armor { get; set; }

    }
    public class Tank : Unit
    {
        public Tank()
        {
            Range = 3;
            Armor = 0.6;
        }
    }
    public class RifleMan : Unit
    {
        public RifleMan()
        {
            Range = 5;
            Armor = 0.9;
        }
    }
}
