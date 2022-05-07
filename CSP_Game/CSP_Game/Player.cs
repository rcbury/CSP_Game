using System.Drawing;
using System;
using System.Collections.Generic;
namespace CSP_Game
{
    public class Player
    {
        public string Name { get; }
        public Color Color { get; }
        public bool IsAlive { get; }

        private Dictionary<Tuple<int, int>, AnyObject> Mastery; // Содержит координаты в качестве ключа, и юнита\строение в качестве значения

        public Player(string name, Color col) 
        {
            Name = name;
            Color = col;
            Mastery = new Dictionary<Tuple<int, int>, AnyObject>();
        }
        public void AddMastery(Tuple<int, int> coords, AnyObject obj)
        {
            Mastery.Add(coords, obj);
        }
        public void RemoveMastery(Tuple<int, int> coords)
        {
            Mastery.Remove(coords);
        }
    }
}
