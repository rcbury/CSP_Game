﻿using System;
using System.Collections.Generic;

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
            Color = player.Color;
            Name = "Столица державы " + player.Name;
        }
    }
    public class MiningCamp : Building
    {
        public int GPS { get; set; }
        public MiningCamp(Player player, Tuple<int,int> coords = null)
        {
            Name = "Рудник";
            GPS = 5;
            Position = coords;
            HP = 40;
            Border = 1;
            Color = player.Color;
            Price = 20;
        }
    }

    public class Tower : Building
    {
        public double Armor { get; set; }
        public Tower(Player player, Tuple<int, int> coords = null)
        {
            Name = "Башня";
            Armor = 0.5;
            Position = coords;
            HP = 80;
            Border = 2;
            Color = player.Color;
            Price = 50;
        }
    }
}
