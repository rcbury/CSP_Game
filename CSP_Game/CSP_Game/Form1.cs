using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyPhotoshop.Data;
using MyPhotoshop;

namespace CSP_Game
{
    public partial class Form1 : Form
    {
        Photo map;
        int playerIndex;
        List<Player> players;
        Player currentPlayer;
        bool bIsBuilding = false; // Check if user wants to build smth
        Unit selectedUnit;        // Contains selected unit;

        public void InitializeMap()
        {
            map = new Photo(80, 80);
            pictureBox1.Height = map.height * map.pixelHeight;
            pictureBox1.Width = map.width * map.pixelWidth;
            for (int x = 0; x < map.width; x++)
                for (int y = 0; y < map.height; y++)
                {
                    map[x, y] = new Pixel(1, 1, 1);
                }
        }

        public void InitializePlayer()
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
            InitializePlayer();
            pictureBox1.Image = Convertors.Photo2Bitmap(map);
            AnyObject[] masterySelector = new AnyObject[]
            {
                new Tank(currentPlayer),
                new RifleMan(currentPlayer),
                new MiningCamp(currentPlayer),
                new Tower(currentPlayer),
            };
            comboBox1.DataSource = masterySelector;
            comboBox1.DisplayMember = "Name";
            PlayerTurn.OnTurnStart(currentPlayer);
            label2.Text = currentPlayer.Treasure.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            int x = (int)Math.Floor((double)Cursor.Position.X / map.pixelWidth);
            int y = (int)Math.Floor((double)(Cursor.Position.Y - 30) / map.pixelHeight);

            /* Весь код ниже. Это надо либо выделить в отдельную сущность, которая будет отвечать за соответствие карты
               всем объектам которые лежат в словарях владений игроков, либо оставить как есть но всё равно выделить некоторые части,
            закрашивающие карту, поскольку они дублируются.
            Основная проблема заключается в том, что фактически карта, которая отображается на форме, никак не связана логически с
            владениями игроков. Это можно решить, вызывая каждый раз функцию обновления карты после какого либо хода пользователя
            (поскольку на каждом ходу игроки так или иначе будут менять карту)
            Также есть проблема с дебаггером - если поставить брейкпоинт куда угодно, то при попытке банально создать объект выдаёт 
            исключение выхода за границы массива пикселей (из которых карта и состоит)

            Проблема с координатами - юнит нормально строится и помещается на карту. После его первого перемещения он нормально перемещается
            и его координаты в словаре владений игрока меняются. Но при попытке переместить его с новой точки в другое место выдается 
            исключение, говорящее что элемента с такими координатами в словаре нет.
            Все эти проблемы я списываю на  массив пикселей, он не работает как надо потому что мы натянули фотку на сетку и получили
            в рот пипетку.

            Также в будущем после завершения логики перемещений юнитов будет ещё одна проблема - если юнит стоит на краю другого юнита
            (или строения), то после его перемещения нужно будет закрасить область, на которой его больше нет (чтобы не возникало границ, которые
            по факту никто не занимает). Это заставит каждый юнит после перемещения "откусывать" границу у другого юнита или строения.
            Пока я не знаю как это можно решить.

             */

            if (selectedUnit != null)
            {
                /*selectedUnit.Position = new Tuple<int, int>(x, y);*/
                for (int i = x - selectedUnit.Border; i <= x + selectedUnit.Border; i++)
                {
                    for (int j = y - selectedUnit.Border; j <= y + selectedUnit.Border; j++)
                    {
                        map[i, j] = new Pixel((double)currentPlayer.Color.R / 255,
                                              (double)currentPlayer.Color.G / 255,
                                              (double)currentPlayer.Color.B / 255);
                    }
                }
                PlayerTurn.MoveSelectedUnit(currentPlayer, selectedUnit, new Tuple<int, int>(x, y));
                selectedUnit = null;
            }
            else
            {
                selectedUnit = PlayerTurn.ReturnSelectedUnit(currentPlayer, new Tuple<int, int>(x, y));
            }

            if (bIsBuilding)
            {
                var objectToBuild = (AnyObject)comboBox1.SelectedValue;
                int border = objectToBuild.Border; 
                PlayerTurn.Build(currentPlayer, objectToBuild, new Tuple<int, int>(x, y));
                for (int i = x - objectToBuild.Border; i <= x + objectToBuild.Border; i++)
                {
                    for (int j = y - objectToBuild.Border; j <= y + objectToBuild.Border; j++)
                    {
                        map[i, j] = new Pixel((double)currentPlayer.Color.R / 255,
                                              (double)currentPlayer.Color.G / 255,
                                              (double)currentPlayer.Color.B / 255);
                    }
                }
                bIsBuilding = false;
            }
            pictureBox1.Image = Convertors.Photo2Bitmap(map);
            /*   MessageBox.Show(comboBox1.SelectedValue.ToString());*/
            /*     map[x, y] = new Pixel((double)currentPlayer.Color.R / 255,
                     (double)currentPlayer.Color.G / 255,
                     (double)currentPlayer.Color.B / 255);
              */
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bIsBuilding = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
