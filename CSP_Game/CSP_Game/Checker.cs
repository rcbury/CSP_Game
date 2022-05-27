using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Game
{
    public class Checker
    {
        public bool TryBuild(int x, int y, Game form)
        {
            bool able = false;
            Type selectedObject = Type.GetType(form.buildingComboBox.SelectedValue.ToString());
            AnyObject objectToBuild = (AnyObject)selectedObject
                .GetConstructor(new Type[] { typeof(Player), typeof(Tuple<int, int>) })
                .Invoke(new object[] { form.currentPlayer, new Tuple<int, int>(x, y) });
            if (AbleToMoveOrCreate(x, y, objectToBuild.Border,form))
            {
                if (objectToBuild.Price <= form.currentPlayer.Treasure)
                {
                    form.buildingObject = objectToBuild;
                    able = true;
                }
                else
                {
                    Notifier.NotifyPlayer("Не хватает средств!");
                }
            }
            else
            {
                Notifier.NotifyPlayer("Постройка здесь невозможна!");
                able = false;
            }
            return able;
        }

        public bool AbleToMoveOrCreate(int x, int y, int offset, Game form)
        {
            bool able = true;
            for (int i = x - offset; i <= x + offset; i++)
            {
                for (int j = y - offset; j <= y + offset; j++)
                {
                    if (!(form.map[i, j].R * 255 == Color.White.R &&
                        form.map[i, j].G * 255 == Color.White.G &&
                        form.map[i, j].B * 255 == Color.White.B))
                    {
                        able = false;
                        break;
                    }
                }
            }
            return able;
        }

        public bool AbleToMoveOrCreate(int x, int y, int offset, Unit unit, Game form)
        {
            bool able = true;
            for (int i = x - offset; i <= x + offset; i++)
            {
                for (int j = y - offset; j <= y + offset; j++)
                {
                    if (!(form.map[i, j].R * 255 == Color.White.R &&
                        form.map[i, j].G * 255 == Color.White.G &&
                        form.map[i, j].B * 255 == Color.White.B) ||
                        (Math.Abs(unit.Position.Item1 - x) + Math.Abs(unit.Position.Item2 - y)) > unit.MovingRange)
                    {
                        able = false;
                        break;
                    }
                }
            }
            return able;
        }

        public bool AbleToAttack(int x, int y, Unit unit, Game form)
        {
            if ((form.map[x, y].R * 255 != form.currentPlayer.Color.R
                && form.map[x, y].G * 255 != form.currentPlayer.Color.G
                && form.map[x, y].B * 255 != form.currentPlayer.Color.B)
                && !unit.bAttackedThisTurn
                && (Math.Abs(unit.Position.Item1 - x) + Math.Abs(unit.Position.Item2 - y)) <= unit.AttackRange)
            {
                var attackedPlayer = form.players.Where(player => player.Color.R == form.map[x, y].R * 255 
                && player.Color.G == form.map[x, y].G * 255 
                && player.Color.B == form.map[x, y].B * 255);
                if (attackedPlayer.Count() != 0)
                {
                    form.attackedPlayer = attackedPlayer.First();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void TrySelect(Tuple<int, int> coords, Game form)
        {
            if (form.map[coords.Item1, coords.Item2].R * 255 == form.currentPlayer.Color.R 
                && form.map[coords.Item1, coords.Item2].G * 255 == form.currentPlayer.Color.G 
                && form.map[coords.Item1, coords.Item2].B * 255 == form.currentPlayer.Color.B)
            {
                var selected = PlayerTurn.ReturnSelectedUnit(form.currentPlayer, 
                    new Tuple<int, int>(coords.Item1, coords.Item2));
                if (selected != null)
                {
                    form.selectedUnit = selected;
                    form.progressBar1.Value = (int)(form.selectedUnit.HP / form.selectedUnit.FullHP * 100);
                }
                else
                {
                    form.selectedUnit = null;
                    form.progressBar1.Value = 0;
                }
            }
        }
    }
}
