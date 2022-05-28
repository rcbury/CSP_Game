using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CSP_Game
{
    public class Action // Является контроллером действий игрока, поскольку осуществляет проверку данных и вызывает соответствующие методы игрока.
    {
        public void Build(int x, int y, Tuple<int, int> position, Game form)
        {
            var dist = FindDistanceToClosestBuilding(form.buildingObject, form);
            if (dist < 10)
            {
                Drawer.DrawObject(form.currentPlayer.Color, form.buildingObject.Border, x, y, form.map);
                PlayerTurn.Build(form.currentPlayer, form.buildingObject, new Tuple<int, int>(x, y));
                Notifier.AddPlayerAction(ref form.historyBox, form.currentPlayer.Name + " построил " + form.buildingObject.Name + ", \nкоординаты: " + position.ToString());
                form.bIsBuilding = false;
            }
        }

        public void Move(int x, int y, Tuple<int, int> position, Unit unit, Game form)
        {
            Drawer.ClearArea(form.selectedUnit.Border, form.selectedUnit.Position.Item1, form.selectedUnit.Position.Item2, form.map);
            Drawer.DrawObject(form.currentPlayer.Color, form.selectedUnit.Border, x, y, form.map);
            PlayerTurn.MoveSelectedUnit(form.currentPlayer, unit, position);
            Notifier.AddPlayerAction(ref form.historyBox, form.currentPlayer.Name + " переместил " + form.selectedUnit.Name + " \nв точку " + position.ToString());
        }

        public void Attack(Tuple<int, int> position, Game form)
        {
            var attackedObj = PlayerTurn.Attack(form.attackedPlayer, form.selectedUnit as Unit, position);
            if (attackedObj != null)
            {
                if (attackedObj.HP <= 0)
                {
                    Drawer.ClearArea(attackedObj.Border, attackedObj.Position.Item1, attackedObj.Position.Item2, form.map);
                    Notifier.AddPlayerAction(ref form.historyBox, form.currentPlayer.Name + " уничтожил\n" + attackedObj.Name);
                }
                else
                {
                    Notifier.AddPlayerAction(ref form.historyBox,
                        form.currentPlayer.Name +
                        " попадает по " +
                        attackedObj.Name +
                        " и наносит " +
                        (form.selectedUnit as Unit).Damage +
                        "\nурона");
                }
            }
            else
            {
                Notifier.AddPlayerAction(ref form.historyBox,
                    form.currentPlayer.Name +
                    " атакует клетку " +
                    position.ToString() +
                    " и промахивается");
            }
        }

        public int FindDistanceToClosestBuilding(AnyObject obj, Game form) // Использует алгоритм поиска в ширину для нахождения ближайщей постройки и проверки возможности постройки
        {
            Queue<Point> queue = new Queue<Point>();
            HashSet<Point> visited = new HashSet<Point>();
            HashSet<Point> buildingSet = new HashSet<Point>();
            for (int i = 0; i < form.map.width; i++)
                for (int j = 0; j < form.map.height; j++)
                {
                    var point = new Tuple<int, int>(i, j);
                    if (form.currentPlayer.Mastery.ContainsKey(point))
                        if (form.currentPlayer.Mastery[point] is Building)
                            buildingSet.Add(new Point(form.currentPlayer.Mastery[point].Position.Item1,
                                form.currentPlayer.Mastery[point].Position.Item2));
                }
            queue.Enqueue(new Point(obj.Position.Item1, obj.Position.Item2));
            visited.Add(new Point(obj.Position.Item1, obj.Position.Item2));
            while (queue.Count != 0)
            {
                Point currCell = queue.Dequeue();
                if (currCell.X < 0 || currCell.X >= form.map.width
                    || currCell.Y < 0 || currCell.Y >= form.map.height)
                    continue;
                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0) continue;
                        else
                        {
                            Point newCell = new Point { X = currCell.X + dx, Y = currCell.Y + dy };
                            if (visited.Contains(newCell)) continue;
                            visited.Add(newCell);
                            queue.Enqueue(newCell);
                        }
                if (form.currentPlayer.Mastery.ContainsKey(new Tuple<int, int>(currCell.X, currCell.Y)))
                    if (form.currentPlayer.Mastery[new Tuple<int, int>(currCell.X, currCell.Y)] is Unit)
                        continue;
                if (buildingSet.Contains(currCell))
                    return (Math.Abs(obj.Position.Item1 - currCell.X)
                        + Math.Abs(obj.Position.Item2 - currCell.Y));
            }
            return 0;
        }

        public void RemoveDeadUnits(List<AnyObject> deadUnits, Game form)
        {
            foreach (AnyObject destroyedObj in deadUnits)
            {
                Drawer.ClearArea(destroyedObj.Border, destroyedObj.Position.Item1, destroyedObj.Position.Item2, form.map);
            }
        }

        public void UpdateObjectInfo(Game form)
        {
            string[] info;
            Label[] labels = new Label[] { 
                form.goldLabel, 
                form.label6, 
                form.label7,
                form.label9, 
                form.label11, 
                form.label13,
                form.GPTLabel 
            };
            if (form.selectedUnit != null)
            {
                info = new string[7] {
                        form.currentPlayer.Treasure.ToString(),
                        form.selectedUnit.Name,
                        form.selectedUnit.Position.ToString(),
                        "","","",""
                };

                if (form.selectedUnit is Unit unit)
                {
                    info[3] = unit.Damage.ToString();
                    info[4] = unit.bMovedThisTurn == false ? "Да" : "Нет";
                    info[5] = unit.bAttackedThisTurn == false ? "Да" : "Нет";
                    info[6] = form.currentPlayer.TotalGPT.ToString();
                }
                else
                {
                    info[3] = "0";
                    info[4] = "Нет";
                    info[5] = "Нет";
                    info[6] = form.currentPlayer.TotalGPT.ToString();
                }
            }
            else
            {
                info = new string[] { form.currentPlayer.Treasure.ToString(), "", "", "", "", "", form.GPTLabel.Text = form.currentPlayer.TotalGPT.ToString() };
            }
            Notifier.UpdatePlayerInfo(form.selectedUnit, info, labels);
        }
    }
}
