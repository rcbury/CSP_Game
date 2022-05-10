using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MyPhotoshop.Data;
using MyPhotoshop;

namespace CSP_Game
{
    public partial class Form1 : Form
    {
        Photo map;
        Bitmap mapImage = new Bitmap("map.jpg");
        int playerIndex;
        List<Player> players;
        Player currentPlayer;
        bool bIsBuilding = false; // Check if user wants to build smth
        Unit selectedUnit;        // Contains selected unit;
        Player attackedPlayer;    // Contains attacked player for one turn;
        Player p = new Player("UnitInit", default);

        public void InitializeMap()
        {
            map = new Photo(30, 30);
            pictureBox1.Height = map.height * map.pixelHeight;
            pictureBox1.Width = map.width * map.pixelWidth;
            //Drawer.DrawObject(Color.White, map.width / 2, map.height / 2, map.width / 2, map.height / 2, map);
            for (int x = 0; x < map.width; x++)
                for (int y = 0; y < map.height; y++)
                {
                    map[x, y] = new Pixel(1, 1, 1);
                }
        }

        public void InitializePlayers()
        {
            players = new List<Player>();
            players.Add(new Player("Andrew", Color.Green));
            players.Add(new Player("Roman", Color.Crimson));
            playerIndex = 0;
            currentPlayer = players[playerIndex];
            Text = currentPlayer.Name;
        }

        public Form1()
        {
            InitializeComponent();
            InitializeMap();
            InitializePlayers();
            pictureBox1.Image = mapImage;
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
            label2.Text = currentPlayer.Treasure.ToString();
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
                PlayerTurn.OnTurnStart(currentPlayer);
                label2.Text = currentPlayer.Treasure.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bIsBuilding = true;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var x = (int)Math.Floor((double)e.X / map.pixelWidth); // X relatively form
            var y = (int)Math.Floor((double)e.Y / map.pixelHeight);// Y relatively form
            MoveOrSelectUnit(x, y);
            TryBuild(x, y);
            pictureBox1.Image = Drawer.DrawMapWithIcons(players, mapImage, map.pixelHeight);
        }

        private void TryBuild(int x, int y)
        {
            if (bIsBuilding)
            {
                Type selectedObject = (Type)comboBox1.SelectedValue;
                AnyObject objectToBuild = (AnyObject)selectedObject
                    .GetConstructor(new Type[] { typeof(Player), typeof(Tuple<int,int>) })
                    .Invoke(new object[] { currentPlayer, new Tuple<int,int>(x,y) });
                if (AbleToMoveOrCreate(x,y,objectToBuild.Border))
                {
                    PlayerTurn.Build(currentPlayer, objectToBuild, new Tuple<int, int>(x, y));
                    Drawer.DrawObject(currentPlayer.Color, objectToBuild.Border, x, y, map);
                    //for (int i = x - objectToBuild.Border; i <= x + objectToBuild.Border; i++)
                    //{
                    //    for (int j = y - objectToBuild.Border; j <= y + objectToBuild.Border; j++)
                    //    {
                    //        map[i, j] = new Pixel((double)currentPlayer.Color.R / 255,
                    //                              (double)currentPlayer.Color.G / 255,
                    //                              (double)currentPlayer.Color.B / 255);
                    //    }
                    //}
                    bIsBuilding = false;
                }
                else
                {
                    MessageBox.Show("Здесь строить нельзя!");
                }
            }
        }

        private void MoveOrSelectUnit(int x, int y)
        {
            var position = new Tuple<int, int>(x, y);
            if (selectedUnit != null)
            {
                if (AbleToMoveOrCreate(x,y,selectedUnit.Border) && !selectedUnit.bMovedThisTurn)
                {
                    Drawer.DrawObject(Color.FromArgb(255, 255, 255), selectedUnit.Border,
                        selectedUnit.Position.Item1, selectedUnit.Position.Item2, map);
                    Drawer.DrawObject(currentPlayer.Color, selectedUnit.Border, x, y, map);
                    PlayerTurn.MoveSelectedUnit(currentPlayer, selectedUnit, position);
                }
                else if (AbleToAttack(x, y, selectedUnit.Border) && !selectedUnit.bAttackedThisTurn)
                {
                    PlayerTurn.Attack(selectedUnit, attackedPlayer, position);
                    attackedPlayer = null;
                }
                selectedUnit = null;
            }
            else
            {
                selectedUnit = PlayerTurn.ReturnSelectedUnit(currentPlayer, position);
                if (selectedUnit != null)
                {
                    progressBar1.Value = (int)(selectedUnit.HP / selectedUnit.FullHP * 100);
                }
                else
                {
                    progressBar1.Value = 0;
                }
            }
        }
        private bool AbleToMoveOrCreate(int x, int y, int offset)
        {
            bool able = true;
            for (int i = x - offset; i <= x + offset; i++)
            {
                for (int j = y - offset; j <= y + offset; j++)
                {
                    if (!(map[i, j].R*255 == Color.White.R && 
                        map[i, j].G*255 == Color.White.G && 
                        map[i, j].B*255 == Color.White.B))
                    {
                        able = false;
                        break;
                    }
                }
            }
            return able;
        }
        private bool AbleToAttack(int x, int y, int offset)
        {
            if(map[x,y].R * 255 != currentPlayer.Color.R && map[x, y].G * 255 != currentPlayer.Color.G && map[x, y].B * 255 != currentPlayer.Color.B)
            {
                var attackedPlayer = players.Where(player => player.Color.R == map[x, y].R*255 && player.Color.G == map[x, y].G * 255 && player.Color.B == map[x, y].B * 255);
                if(attackedPlayer.Count() != 0)
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
