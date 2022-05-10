using System;
using System.Collections.Generic;

namespace CSP_Game
{
    public class PlayerTurn // является контроллером, поскольку передаёт данные модели после обработки и возвращает главному контроллеру результат работы
                            // также выполняет подготовку данных перед передачей модели
    {
        public static List<AnyObject> OnTurnStart(Player player)
        {
            var destroyed = player.UpdateTreasure();
            player.SetUnitsHaveRested();
            return destroyed;
        }
        public static void Build(Player player, AnyObject x, Tuple<int, int> position)
        {
            x.Position = position;
            player.AddMastery(position, x);
        }
        public static AnyObject Attack(Player attackedPlayer, Unit x, Tuple<int, int> position)
        {
            x.bAttackedThisTurn = true;
            return attackedPlayer.TakeDamage(x.Damage, position);

        }
        public static AnyObject ReturnSelectedUnit(Player player, Tuple<int, int> position)
        {
            return player.ReturnSelectedUnit(position);
        }
        public static void MoveSelectedUnit(Player player, Unit x, Tuple<int, int> position)
        {
            x.bMovedThisTurn = true;
            player.MoveSelectedUnit(x.Position, position);
        }
    }
}
