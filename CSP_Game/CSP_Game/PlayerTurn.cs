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
        public static List<AnyObject> OnTurnStart(Player player)
        {
            var destroyed = player.UpdateTreasure();
            player.SetUnitsHaveRested();
            return destroyed;
        }
        public static void Build(Player player, AnyObject x, Tuple<int, int> position)
        {
            x.Position = position;
            player.AddMastery(position, x);
            /*            MessageBox.Show(newObject.ToString());*/
        }
        public static AnyObject Attack(Player attackedPlayer, Unit x, Tuple<int, int> position)
        {
            x.bAttackedThisTurn = true;
            return attackedPlayer.TakeDamage(x.Damage, position);

        }
        public static AnyObject ReturnSelectedUnit(Player player, Tuple<int, int> position)
        {
            return player.ReturnSelectedUnit(position);
        }
        public static void MoveSelectedUnit(Player player, Unit x, Tuple<int, int> position)
        {
            x.bMovedThisTurn = true;
            player.MoveSelectedUnit(x.Position, position);
        }
    }
}
