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
    public class PlayerTurn
    {
        public static void Build(Player player, AnyObject x, Tuple<int, int> position)
        {
            x.Position = position;
            player.AddMastery(position, x);
/*            MessageBox.Show(newObject.ToString());*/
        }
        public static void Attack()
        {
            
        }
    }
}
