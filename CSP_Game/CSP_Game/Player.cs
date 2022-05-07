using System.Drawing;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CSP_Game
{
    public class Player
    {
        public string Name { get; }
        public Color Color { get; }
        public bool IsAlive { get; }
        public int Treasure { get; private set; } // Whole money that player can spend
        public int TotalGPS { get; private set; } // GPS - Gold Per Second

        private Dictionary<Tuple<int, int>, AnyObject> Mastery; // Contains coordinates as a key, Unit/Building as value
                                                                // It allows faster removing or finding different objects, which coordinates can be
                                                                // recognized via click
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
        public void UpdateTreasure(int value)
        {
            Treasure += value;
        }
        public void SetUnitsHaveRested()
        {
            foreach (var pair in Mastery)
            {
                if (pair.Value is Unit)
                {
                    (pair.Value as Unit).bAttackedThisTurn = false;
                    (pair.Value as Unit).bMovedThisTurn = false;
                    MessageBox.Show("Units have rested");
                }
            }
        }
    }
}
