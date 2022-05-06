using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public class PlayerTurn
    {
        public void Build(Player player, AnyObject x, Tuple<int, int> position)
        {
            player.AddMastery(position, x);
        }
        public void Attack()
        {
            
        }
    }
}
