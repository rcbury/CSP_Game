﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace CSP_Game
{
    public class Player // является моделью, поскольку реализует всю логику, связанную с пользователем, его владениями и прочим
    {
        public string Name { get; }
        public Color Color { get; }
        public bool IsAlive { get; private set; } = true;
        public int Treasure { get; private set; } // Whole money that player can spend
        public int TotalGPT { get; private set; } // GPT - Gold Per Turn

        public Dictionary<Tuple<int, int>, AnyObject> Mastery; // Contains coordinates as a key, Unit/Building as value
                                                               // It allows faster removing or finding different objects, which coordinates can be
                                                               // recognized via click
        public Player(string name, Color col)
        {
            Name = name;
            Color = col;
            Mastery = new Dictionary<Tuple<int, int>, AnyObject>();
            TotalGPT = 100;
        }
        public void AddMastery(Tuple<int, int> coords, AnyObject obj)
        {
            if (!Mastery.ContainsKey(coords))
            {
                Mastery.Add(coords, obj);
                Treasure -= obj.Price;
                if (obj is MiningCamp)
                {
                    TotalGPT += (obj as MiningCamp).GPT;
                }
                else
                {
                    TotalGPT -= obj.Rent;
                }
            }
        }
        public void RemoveMastery(Tuple<int, int> coords)
        {
            Mastery.Remove(coords);
        }
        public AnyObject ReturnSelectedUnit(Tuple<int, int> coords)
        {
            if (Mastery.ContainsKey(coords))
            {
                return Mastery[coords];
            }
            else
            {
                return null;
            }
        }
        public void MoveSelectedUnit(Tuple<int, int> coordsStart, Tuple<int, int> coordsEnd)
        {
            if (!coordsEnd.Equals(coordsStart))
            {
                /*  AddMastery(coordsEnd, Mastery[coordsStart]); */
                Mastery.Add(coordsEnd, Mastery[coordsStart]);
                Mastery[coordsEnd].Position = coordsEnd;
                RemoveMastery(coordsStart);
            }
        }
        public List<AnyObject> UpdateTreasure()
        {
            Treasure = Treasure + TotalGPT;
            List<AnyObject> destroyed = new List<AnyObject>();
            if (Treasure < 0)
            {
                foreach (var pair in Mastery)
                {
                    if (pair.Value.Rent != 0)
                    {
                        if (TotalGPT + pair.Value.Rent <= 0)
                        {
                            TotalGPT += pair.Value.Rent;
                            Treasure += pair.Value.Rent;
                            destroyed.Add(pair.Value);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                foreach (var unit in destroyed)
                {
                    RemoveMastery(unit.Position);
                }
            }
            return destroyed.Count > 0 ? destroyed : null;
        }
        public AnyObject TakeDamage(double damage, Tuple<int, int> coords)
        {
            if (Mastery.ContainsKey(coords))
            {
                if (Mastery[coords] is IArmored)
                {
                    Mastery[coords].HP = Mastery[coords].HP - damage * (Mastery[coords] as IArmored).Armor;
                }
                else
                {
                    Mastery[coords].HP = Mastery[coords].HP - damage;
                }
                if (Mastery[coords].HP <= 0)
                {
                    var destroyedObj = Mastery[coords];
                    if (destroyedObj is Capital)
                    {
                        IsAlive = false;
                    }
                    TotalGPT += destroyedObj.Rent;
                    RemoveMastery(coords);
                    return destroyedObj;
                }
                else
                {
                    return Mastery[coords];
                }
            }
            else
            {
                return null;
            }
        }
            public void SetUnitsHaveRested()
        {
            foreach (var pair in Mastery)
            {
                if (pair.Value is Unit)
                {
                    (pair.Value as Unit).bAttackedThisTurn = false;
                    (pair.Value as Unit).bMovedThisTurn = false;
                }
            }
        }
    }
}
