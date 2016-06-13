using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace bomber_winform
{
    class Player
    {
        public int maxBombCount = 3;
        public int bombCount = 0;

        public bool penetration = false;

        public int bombRange = 2;
        public int bombClock = 8;

        public int id;
        public bool dead = false;
        public Vector2i positon;
        private Texture playerTexture;
        public Sprite player;

        private bool[] collision = { true, false, true, true, true, true, true, true, true };

        public short tileSize = 16;
        private short scale = 2;

        public Keyboard.Key[] controls;

        private int activeKey = 0;
        private bool waitKey = false;

        private int[,] map;
        private int[,] gameState;

        public string name;

        public Player(int posX, int posY, int[,] map, Keyboard.Key[] controls, int id, string name)
        {
            this.playerTexture = new Texture("resources/tileset.png", new IntRect(new Vector2i(this.tileSize * id, this.tileSize * 2), new Vector2i(this.tileSize, this.tileSize)));
            this.player = new Sprite(this.playerTexture);
            this.player.Scale = new Vector2f(2.0f, 2.0f);
            this.player.Position = new Vector2f(posX * this.tileSize * this.scale, posY * this.tileSize * this.scale);
            this.positon = new Vector2i(posX, posY);
            this.map = map;
            this.id = id;
            this.controls = controls;
            this.name = name;

        }
        private bool checkCollision(int translateX, int translateY)
        {
            int x = this.positon.X + translateX;
            int y = this.positon.Y + translateY;

            if ((x < 0 || x > 15) || (y < 0 || y > 15))
                return false;
            if (this.collision[this.map[y, x]])
                return false;
            if (this.gameState[y, x] != 0)
                return false;

            return true;
        }
        private void action(int state, ref List<Bomb> bombs, long clock)
        {
            switch (state)
            {
                case 0:
                    if (checkCollision(0, -1))
                        this.positon.Y -= 1;
                    break;
                case 1:
                    if (checkCollision(0, 1))
                        this.positon.Y += 1;
                    break;
                case 2:
                    if (checkCollision(-1, 0))
                        this.positon.X -= 1;
                    break;
                case 3:
                    if (checkCollision(+1, 0))
                        this.positon.X += 1;
                    break;
                case 4:
                    if (this.gameState[this.positon.Y, this.positon.X] != 1 && this.bombCount < this.maxBombCount)
                    {
                        this.gameState[this.positon.Y, this.positon.X] = 1;
                        this.bombCount++;
                        bombs.Add(new Bomb(clock + this.bombClock, this.positon, this.id, this.bombRange,this.penetration));
                    }

                    break;
            }
            this.player.Position = new Vector2f(this.positon.X * this.tileSize * this.scale, this.positon.Y * this.tileSize * this.scale);
        }
        public int[,] input(int[,] gameState, ref List<Bomb> bombs, long clock)
        {
            if (gameState[this.positon.Y, this.positon.X] == 2)
            {
                this.dead = true;

            }
            if (!dead)
            {
                this.gameState = gameState;
                int state = 0;
                foreach (var i in this.controls)
                {

                    if (Keyboard.IsKeyPressed(i) && this.activeKey == 0)
                    {
                        this.activeKey = state + 1;
                    }

                    state++;
                }
                if (this.activeKey != 0 && this.waitKey == false)
                {
                    this.action(this.activeKey - 1, ref bombs, clock);
                    this.waitKey = true;
                }
                else if (this.waitKey)
                {
                    this.waitKey = false;
                    this.activeKey = 0;
                }
                return this.gameState;
            }
            return gameState;

        }
    }
    
}