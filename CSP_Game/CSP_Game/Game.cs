using MyPhotoshop;
using MyPhotoshop.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CSP_Game
{
    public partial class Game : Form // является контроллером, поскольку отвечает за связь модели и отображения.
                                      // реализует логику, позволяющую пользователю обращаться к модели посредством отображения
                                      // выводит всю информацию посредством класса Drawer (обращение к выводу)
                                      // выводит все уведомления посредством класса Notifier
    {
        public Photo map;
        public int playerIndex;
        public List<Player> players;
        public Player currentPlayer;
        public bool bIsBuilding = false; // Check if user wants to build smth
        public AnyObject selectedUnit = null;   // Contains selected object
        public AnyObject buildingObject; // Contains object for building
        public Player attackedPlayer;    // Contains attacked player for one turn
        public Action act = new Action(); // Contains methods which perform player actions and drawin it
        public Checker check = new Checker(); // Contains methods which returning player ability status to perform player's actions later

        public void InitializeMap()
        {
            map = new Photo(29, 26);
            mapBox.Height = map.height * map.pixelHeight;
            mapBox.Width = map.width * map.pixelWidth;
            for (int x = 0; x < map.width; x++)
                for (int y = 0; y < map.height; y++)
                {
                    map[x, y] = new Pixel(1, 1, 1);
                }
        }

        public void InitializePlayers()
        {
            players = new List<Player>
            {
                new Player("Andrew", Color.Green),
                new Player("Roman", Color.Crimson)
            };
            Tuple<int, int>[] playersCapitals = new Tuple<int, int>[]
            {
                new Tuple<int, int>(4, 4),
                new Tuple<int, int>(24, 21),
            };
            foreach (var player in players)
            {
                var capitalCoords = playersCapitals[players.IndexOf(player)];
                PlayerTurn.Build(player, new Capital(player, capitalCoords), capitalCoords);
                Drawer.DrawObject(player.Color, 4, capitalCoords.Item1, capitalCoords.Item2, map);
            }
            playerIndex = 0;
            currentPlayer = players[0];
            Text = currentPlayer.Name;
        }

        public Game()
        {
            InitializeComponent();
            InitializeMap();
            InitializePlayers();
            WindowState = FormWindowState.Maximized;
            Type[] masterySelector = new Type[]
            {
                typeof(Tank),
                typeof(RifleMan),
                typeof(Tower),
                typeof(MiningCamp)
            };
            string[] masteryNames = new string[]
            {
                "Танк",
                "Снайпер",
                "Башня",
                "Рудник"
            };
            DataSet masteryds = new DataSet();
            masteryds.Tables.Add();
            masteryds.Tables[0].Columns.Add("Type");
            masteryds.Tables[0].Columns.Add("Name");
            for (int i = 0; i < masterySelector.Length; i++) 
            {
                masteryds.Tables[0].Rows.Add();
                masteryds.Tables[0].Rows[i]["Type"] = masterySelector[i];
                masteryds.Tables[0].Rows[i]["Name"] = masteryNames[i];
            }
            buildingComboBox.DataSource = masteryds.Tables[0];
            buildingComboBox.DisplayMember = "Name";
            buildingComboBox.ValueMember = "Type";
            mapBox.Image = Drawer.DrawMapWithIcons(players,Convertors.Photo2Bitmap(map), map.pixelHeight);
            PlayerTurn.OnTurnStart(currentPlayer);
            act.UpdateObjectInfo(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (players.Where(player => player.IsAlive).Count() != 1)
            {
                playerIndex++;
                if (playerIndex > players.Count - 1)
                    playerIndex = 0;
                currentPlayer = players[playerIndex];
                Text = currentPlayer.Name;
                var deadUnits = PlayerTurn.OnTurnStart(currentPlayer);
                if (deadUnits != null)
                {
                    act.RemoveDeadUnits(deadUnits,this);
                    Bitmap mapImage = Convertors.Photo2Bitmap(map);
                    mapBox.Image = Drawer.DrawMapWithIcons(players,Convertors.Photo2Bitmap(map),
                        map.pixelHeight);
                }

                selectedUnit = null;
                attackedPlayer = null;
                buildingObject = null;
                label2.Text = currentPlayer.Treasure.ToString();
            }
            else
            {
                Notifier.NotifyPlayer("Игра окончена! Война привела " + players
                    .Where(player => player.IsAlive == true)
                    .First().Name + " к победе");
            }
            act.UpdateObjectInfo(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bIsBuilding = true;
            selectedUnit = null;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var x = (int)Math.Floor((double)e.X / map.pixelWidth); // X relatively form
            var y = (int)Math.Floor((double)e.Y / map.pixelHeight);// Y relatively form
            var position = new Tuple<int, int>(x, y);
            var unit = selectedUnit as Unit;
            if (unit != null)
            {
                if (check.AbleToAttack(x, y, unit,this) && !unit.bAttackedThisTurn)
                {
                    act.Attack(position,this);
                }
                else if (check.AbleToMoveOrCreate(x, y, selectedUnit.Border, unit, this) 
                    && !unit.bMovedThisTurn)
                {
                    act.Move(x, y, position, unit, this);
                }
                check.TrySelect(position,this);
            }
            else
            {
                if (bIsBuilding && check.TryBuild(x, y, this))
                {
                    act.Build(x, y, position,this);
                }
                else
                {
                    check.TrySelect(position,this);
                }
            }
            act.UpdateObjectInfo(this);
            mapBox.Image = Drawer.DrawMapWithIcons(players, 
                Convertors.Photo2Bitmap(map), map.pixelHeight);
        }
    }
}
