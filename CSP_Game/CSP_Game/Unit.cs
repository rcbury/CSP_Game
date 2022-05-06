using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public interface IMovable
    {
        Tuple<int,int> MoveTo();
    }
    public class Unit : AnyObject, IMovable
    {
        public Tuple<int, int> Position;

        public Tuple<int, int> MoveTo()
        {
            throw new Exception();
        }
    }
    public class Tank : Unit
    {

    }
}
