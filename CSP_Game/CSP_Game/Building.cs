using System;
using System.Drawing;

namespace CSP_Game
{
    public class Building : AnyObject
    {
    }
    public class Capital : Building
    {
        public Capital(Player player, Tuple<int, int> coords = null)
        {
            Position = coords;
            Border = 4;
            HP = 1;
            FullHP = 500;
            Color = player.Color;
            Price = 0;
            Name = "Столица державы " + player.Name;
            Icon = Image.FromFile("capital.png");
            Rent = 0;
        }
    }
    public class MiningCamp : Building // Вынесение объектов увеличивающих GPS позволит добавлять другие объекты связанные с GPS'ом (Например, таможня или банк)
    {
        public int GPS { get; set; }
        public MiningCamp(Player player, Tuple<int, int> coords = null)
        {
            Position = coords;
            Border = 1;
            HP = 40;
            FullHP = 40;
            Color = player.Color;
            Price = 20;
            Name = "Рудник";
            Icon = Image.FromFile("miningcamp.png");
            Rent = 0;

            GPS = 5;
        }
    }

    public class Tower : Building, IArmored
    {

        public Tower(Player player, Tuple<int, int> coords = null)
        {
            Position = coords;
            Border = 2;
            HP = 80;
            FullHP = 80;
            Color = player.Color;
            Price = 50;
            Name = "Башня";
            Icon = Image.FromFile("tower.png");
            Rent = 15;

            Armor = 0.5;
        }
        public double Armor { get; set; }
    }
}
