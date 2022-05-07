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
        public static void Build(Player player, AnyObject x, Tuple<int, int> position, Photo map)
        {
            x.Position = position;
            player.AddMastery(position, x);
            for (int i = position.Item1 - x.Border; i <= position.Item1 + x.Border; i++)
            {
                for (int j = position.Item2 - x.Border; j <= position.Item2 + x.Border; j++)
                {
                    map[i, j] = new Pixel((double)player.Color.R / 255,
                                          (double)player.Color.G / 255,
                                          (double)player.Color.B / 255);
                }
            }
            /*            MessageBox.Show(newObject.ToString());*/
        }
        public static void Attack()
        {
            
        }
    }
}
