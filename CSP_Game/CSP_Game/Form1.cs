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
    public partial class Form1 : Form // является контроллером, поскольку отвечает за связь модели и отображения.
                                      // реализует логику, позволяющую пользователю обращаться к модели посредством отображения
                                      // выводит всю информацию посредством класса Drawer (обращение к выводу)
                                      // выводит все уведомления посредством класса Notifier
    {
        Photo map;
        int playerIndex;
        List<Player> players;
        Player currentPlayer;
        bool bIsBuilding = false; // Check if user wants to build smth
        AnyObject selectedUnit;   // Contains selected object
        AnyObject buildingObject; // Contains object for building
        Player attackedPlayer;    // Contains attacked player for one turn

        public void InitializeMap()
        {
            map = new Photo(30, 30);
            pictureBox1.Height = map.height * map.pixelHeight;
            pictureBox1.Width = map.width * map.pixelWidth;
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
                new Tuple<int, int>(25, 25),
            };
            foreach (var player in players)
            {
                var capitalCoords = playersCapitals[players.IndexOf(player)];
                PlayerTurn.Build(player, new Capital(player, capitalCoords), capitalCoords);
                Drawer.DrawObject(player.Color, 4, capitalCoords.Item1, capitalCoords.Item2, map);
            }
            playerIndex = 0;
            currentPlayer = players[playerIndex];
            Text = currentPlayer.Name;
        }

        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            InitializeMap();
            InitializePlayers();
            Type[] masterySelector = new Type[]
            {
                typeof(Tank),
                typeof(RifleMan),
                typeof(Tower),
                typeof(MiningCamp)
            };
            comboBox1.DataSource = masterySelector;
            comboBox1.DisplayMember = "Name";
            PlayerTurn.OnTurnStart(currentPlayer);
            pictureBox1.Image = Drawer.DrawMapWithIcons(players,Convertors.Photo2Bitmap(map), map.pixelHeight);
            selectedUnit = null;
            UpdateObjectInfo();
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
                    RemoveDeadUnits(deadUnits);
                    Bitmap mapImage = Convertors.Photo2Bitmap(map);
                    pictureBox1.Image = Drawer.DrawMapWithIcons(players,Convertors.Photo2Bitmap(map), map.pixelHeight);
                }

                selectedUnit = null;
                attackedPlayer = null;
                buildingObject = null;
                label2.Text = currentPlayer.Treasure.ToString();
            }
            else
            {
                Notifier.NotifyPlayer("Игра окончена! Война привела " + players.Where(player => player.IsAlive == true).First().Name + " к победе");
            }
            UpdateObjectInfo();
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
            if (selectedUnit is Unit)
            {
                if (AbleToAttack(x, y) && !(selectedUnit as Unit).bAttackedThisTurn)
                {
                    var attackedObj = PlayerTurn.Attack(attackedPlayer, selectedUnit as Unit, position);
                    if (attackedObj != null)
                    {
                        if (attackedObj.HP <= 0)
                        {
                            Drawer.ClearArea(attackedObj.Border, attackedObj.Position.Item1, attackedObj.Position.Item2, map);
                            Notifier.AddPlayerAction(ref listBox1, currentPlayer.Name + " уничтожил\n" + attackedObj.Name);
                        }
                        else
                        {
                            Notifier.AddPlayerAction(ref listBox1,
                                currentPlayer.Name +
                                " попадает по " +
                                attackedObj.Name +
                                " и наносит " +
                                (selectedUnit as Unit).Damage +
                                "\nурона");
                        }
                    }
                    else
                    {
                        Notifier.AddPlayerAction(ref listBox1,
                            currentPlayer.Name +
                            " атакует клетку " +
                            position.ToString() +
                            " и промахивается");
                    }
                }
                else if (AbleToMoveOrCreate(x, y, selectedUnit.Border) && !(selectedUnit as Unit).bMovedThisTurn)
                {
                    Drawer.ClearArea(selectedUnit.Border, selectedUnit.Position.Item1, selectedUnit.Position.Item2, map);
                    Drawer.DrawObject(currentPlayer.Color, selectedUnit.Border, x, y, map);
                    PlayerTurn.MoveSelectedUnit(currentPlayer, selectedUnit as Unit, position);
                    Notifier.AddPlayerAction(ref listBox1, currentPlayer.Name + " переместил " + selectedUnit.Name + "\nв точку " + position.ToString());
                }
                TrySelect(position);
            }
            else
            {
                if (bIsBuilding && TryBuild(x, y))
                {
                    Drawer.DrawObject(currentPlayer.Color, buildingObject.Border, x, y, map);
                    PlayerTurn.Build(currentPlayer, buildingObject, new Tuple<int, int>(x, y));
                    Notifier.AddPlayerAction(ref listBox1, currentPlayer.Name + " построил " + buildingObject.Name + ",\nкоординаты: " + position.ToString());
                    bIsBuilding = false;
                }
                else
                {
                    TrySelect(position);
                }
            }
            UpdateObjectInfo();
            pictureBox1.Image = Drawer.DrawMapWithIcons(players, Convertors.Photo2Bitmap(map), map.pixelHeight);
        }

        private bool TryBuild(int x, int y)
        {
            bool able = false;
            Type selectedObject = (Type)comboBox1.SelectedValue;
            AnyObject objectToBuild = (AnyObject)selectedObject
                .GetConstructor(new Type[] { typeof(Player), typeof(Tuple<int, int>) })
                .Invoke(new object[] { currentPlayer, new Tuple<int, int>(x, y) });
            if (AbleToMoveOrCreate(x, y, objectToBuild.Border))
            {
                if (objectToBuild.Price <= currentPlayer.Treasure)
                {
                    buildingObject = objectToBuild;
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

        private bool AbleToMoveOrCreate(int x, int y, int offset)
        {
            bool able = true;
            for (int i = x - offset; i <= x + offset; i++)
            {
                for (int j = y - offset; j <= y + offset; j++)
                {
                    if (!(map[i, j].R * 255 == Color.White.R &&
                        map[i, j].G * 255 == Color.White.G &&
                        map[i, j].B * 255 == Color.White.B))
                    {
                        able = false;
                        break;
                    }
                }
            }
            return able;
        }

        private bool AbleToAttack(int x, int y)
        {
            if (map[x, y].R * 255 != currentPlayer.Color.R && map[x, y].G * 255 != currentPlayer.Color.G && map[x, y].B * 255 != currentPlayer.Color.B && !(selectedUnit as Unit).bAttackedThisTurn)
            {
                var attackedPlayer = players.Where(player => player.Color.R == map[x, y].R * 255 && player.Color.G == map[x, y].G * 255 && player.Color.B == map[x, y].B * 255);
                if (attackedPlayer.Count() != 0)
                {
                    this.attackedPlayer = attackedPlayer.First();
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

        private void TrySelect(Tuple<int, int> coords)
        {
            if (map[coords.Item1, coords.Item2].R * 255 == currentPlayer.Color.R && map[coords.Item1, coords.Item2].G * 255 == currentPlayer.Color.G && map[coords.Item1, coords.Item2].B * 255 == currentPlayer.Color.B)
            {
                var selected = PlayerTurn.ReturnSelectedUnit(currentPlayer, new Tuple<int, int>(coords.Item1, coords.Item2));
                if (selected != null)
                {
                    selectedUnit = selected;
                    progressBar1.Value = (int)(selectedUnit.HP / selectedUnit.FullHP * 100);
                }
                else
                {
                    selectedUnit = null;
                    progressBar1.Value = 0;
                }
            }
        }

        private void RemoveDeadUnits(List<AnyObject> deadUnits)
        {
            foreach (AnyObject destroyedObj in deadUnits)
            {
                Drawer.ClearArea(destroyedObj.Border, destroyedObj.Position.Item1, destroyedObj.Position.Item2, map);
            }
        }

        private void UpdateObjectInfo()
        {
            string[] info;
            Label[] labels = new Label[] { label2, label6, label7, label9, label11, label13, label16 };
            if (selectedUnit != null)
            {
                if (selectedUnit is Unit)
                {
                    info = new string[] {
                        currentPlayer.Treasure.ToString(),
                        selectedUnit.Name,
                        selectedUnit.Position.ToString(), 
                        (selectedUnit as Unit).Damage.ToString(), 
                        (selectedUnit as Unit).bMovedThisTurn == false ? "Да" : "Нет",
                        (selectedUnit as Unit).bAttackedThisTurn == false ? "Да" : "Нет",
                        currentPlayer.TotalGPT.ToString()
                    };
                }
                else
                {
                    info = new string[] {
                        currentPlayer.Treasure.ToString(),
                        selectedUnit.Name,
                        selectedUnit.Position.ToString(),
                        "0", 
                        "Нет", 
                        "Нет",
                        currentPlayer.TotalGPT.ToString()
                    };
                }
            }
            else
            {
                info = new string[] { currentPlayer.Treasure.ToString(), "", "", "", "", "", label16.Text = currentPlayer.TotalGPT.ToString() };
            }
            Notifier.UpdatePlayerInfo(selectedUnit, info, labels);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        /* 
        1. Все проверки перемещения и атаки юнитов
        2. Реализовать более точную логику перемещения юнитов с учётом того, что путь не всегда чист
        (Возможные варианты = забить, алгоритм поиска кратчайшего пути, etc...)
        3. Если предыдущий пункт слишком сложен, можно его не делать, хватит и проверки
        */
    }
}
