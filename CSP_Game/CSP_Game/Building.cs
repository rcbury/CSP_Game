using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public class Building: AnyObject
    {
        public readonly Tuple<int, int> Position;
    }

    public class MiningCamp : Building
    {
        public int GPS { get; set; }
    }

    public class Tower : Building
    {
        public double Armor { get; set; }
    }
}
