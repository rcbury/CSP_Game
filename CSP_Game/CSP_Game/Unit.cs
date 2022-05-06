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
        public int MovingRange { get; set; }
        public int AttackRange { get; set; }
        public double Armor { get; set; }
        public double Damage { get; set; }
    }
    public class Tank : Unit
    {
        
        public Tank(Player player, Tuple<int, int> coords = null)
        {
            Name = "Танк";
            MovingRange = 3;
            AttackRange = 5;
            Armor = 0.6;
            HP = 100;
            Position = coords;
            Color = player.Color;
            Price = 50;
            Damage = 20;
        }
    }
    public class RifleMan : Unit
    {
        public RifleMan(Player player, Tuple<int, int> coords = null)
        {
            Name = "Снайпер";
            MovingRange = 5;
            AttackRange = 6;
            Armor = 0.9;
            HP = 60;
            Position = coords;
            Color = player.Color;
            Price = 20;
            Damage = 10;
        }
    }
}
