using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public class Unit : AnyObject
    {
        public int MovingRange { get; set; }
        public bool bMovedThisTurn { get; set; }
        public int AttackRange { get; set; }
        public bool bAttackedThisTurn { get; set; }
        public double Armor { get; set; }
        public double Damage { get; set; }
    }
    public class Tank : Unit
    {
        public Tank(Player player, Tuple<int, int> coords = null)
        {
            Position = coords;
            Border = 1;
            HP = 100;
            Color = player.Color;
            Price = 50;
            Name = "Танк";

            MovingRange = 3;
            bMovedThisTurn = true;
            AttackRange = 5;
            bAttackedThisTurn = true;
            Armor = 0.6;
            Damage = 20;
        }
    }
    public class RifleMan : Unit
    {
        public RifleMan(Player player, Tuple<int, int> coords = null)
        {
            Position = coords;
            Border = 1;
            HP = 60;
            Color = player.Color;
            Price = 20;
            Name = "Снайпер";

            MovingRange = 5;
            bMovedThisTurn = true;
            AttackRange = 6;
            bAttackedThisTurn = true;
            Armor = 0.9;
            Damage = 10;

        }
    }
}
