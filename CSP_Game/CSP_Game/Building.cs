using System;
using System.Drawing;

namespace CSP_Game
{
    public class Building : AnyObject // является простой сущностью, с которой в дальнейшем могут взаимодействовать модель и контроллер
    {
    }
    public class Capital : Building
    {
        public Capital(Player player, Tuple<int, int> coords = null)
        {
            Position = coords;
            Border = 4;
            HP = 500;
            FullHP = 500;
            Color = player.Color;
            Price = 0;
            Name = "Столица " + player.Name;
            Icon = Image.FromFile("capital.png");
            Rent = 0;
        }
    }
    public class MiningCamp : Building // Вынесение объектов увеличивающих GPT позволит добавлять другие объекты связанные с GPT'ом (Например, таможня или банк)
    {
        public int GPT { get; set; }
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
            GPT = 5;
        }
    }
    public class Tower : Building, IArmored
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
