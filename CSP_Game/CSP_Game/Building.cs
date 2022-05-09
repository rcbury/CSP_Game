using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public class Building : AnyObject
    {
    }
    public class Capital : Building
    {
        public Capital(Player player, Tuple<int, int> coords)
        {
            Position = coords;
            Border = 4;
            HP = 500;
            FullHP = 500;
            Color = player.Color;
            Name = "Столица державы " + player.Name;
            Icon = Image.FromFile("capital.png");
        }
    }
    public class MiningCamp : Building // Вынесение объектов увеличивающих GPS позволит добавлять другие объекты связанные с GPS'ом (Например, таможня или банк)
    {
        public int GPS { get; set; }
        public MiningCamp(Player player, Tuple<int,int> coords = null)
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

    public class Tower : Building // Наследование этого типа и типов юнитов от типа "Armored" позволило бы не писать код несколько раз
    {
        public double Armor { get; set; }
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
    }
}
