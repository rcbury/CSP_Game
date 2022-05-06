using System.Drawing;

namespace CSP_Game
{
    class Player
    {
        public string Name { get; }
        public Color Color { get; }
        public bool IsAlive { get; }

        public Player(string name, Color col) 
        {
            Name = name;
            Color = col;
        }
    }
}
