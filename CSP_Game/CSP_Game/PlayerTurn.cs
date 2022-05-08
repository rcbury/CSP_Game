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
        public static void OnTurnStart(Player player)
        {
            player.UpdateTreasure(5);
            player.SetUnitsHaveRested();
        }
        public static void Build(Player player, AnyObject x, Tuple<int, int> position)
        {
            x.Position = position;
            player.AddMastery(position, x);
            /*            MessageBox.Show(newObject.ToString());*/
        }
        public static void Attack()
        {
            
        }
        public static Unit ReturnSelectedUnit(Player player, Tuple<int, int> position)
        {
            return player.ReturnSelectedUnit(position);
        }
        public static void MoveSelectedUnit(Player player, Unit x, Tuple<int, int> position)
        {
            player.MoveSelectedUnit(x.Position, position);
        }
    }
}
