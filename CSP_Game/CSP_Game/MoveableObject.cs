using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public class MoveableObject : AnyObject
    {
        public readonly Tuple<int, int> Position;
    }

    public class MiningCamp : MoveableObject
    {
        public int GPS { get; set; }
    }

    public class Tower : MoveableObject
    {
        public double Armor { get; set; }
    }
}
