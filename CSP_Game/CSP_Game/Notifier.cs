using System.Windows.Forms;

namespace CSP_Game
{
    public class Notifier // это вывод, поскольку выводит историю действий каждого игрока и обновляет выводимую информацию после каждого действия
    {
        public static void NotifyPlayer(string msg)
        {
            MessageBox.Show(msg);
        }
        public static void AddPlayerAction(ref ListBox actions, string str)
        {
            actions.Items.Add(str);
        }
        public static void UpdatePlayerInfo(AnyObject selectedUnit, string[] info, params Label[] labels)
        {
            for(int i = 0; i< labels.Length; i++)
            {
                labels[i].Text = info[i];
            }

        }
    }
}
