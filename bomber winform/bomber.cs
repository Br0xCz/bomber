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
        private int maxBombCount = 3;
        public int bombCount = 0;

        public int id;
        public bool dead = false;
        public Vector2i positon;
        private Texture playerTexture = new Texture("resources/player.png");
        public Sprite player;

        private bool[] collision = { true, true, false, true, true };

        public short tileSize = 16;
        private short scale = 2;

        public Keyboard.Key[] controls;

        private int activeKey = 0;
        private bool waitKey = false;

        private int[,] map;
        private int[,] gameState;
        public Player(int posX, int posY, int[,] map, Keyboard.Key[] controls, int id)
        {
            this.player = new Sprite(this.playerTexture);
            this.player.Scale = new Vector2f(2.0f, 2.0f);
            this.player.Position = new Vector2f(posX * this.tileSize * this.scale, posY * this.tileSize * this.scale);
            this.positon = new Vector2i(posX, posY);
            this.map = map;
            this.id = id;
            this.controls = controls;

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
                        bombs.Add(new Bomb(clock + 5, this.positon, this.id));
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
    class Bomb
    {
        public int owner;
        public long explosionTime;
        private bool exploding = false;
        private bool stopExploding = false;
        public bool destroyBomb = false;
        private Vector2i position;
        public Bomb(long explosionTime, Vector2i position, int id)
        {
            this.position = new Vector2i(Convert.ToInt32(position.X), Convert.ToInt32(position.Y));
            this.explosionTime = explosionTime;
            this.owner = id;
        }
        public void checkExplosion(long clock, ref int[,] map, ref int[,] gameState)
        {
            if (this.explosionTime == clock && this.exploding == false)
            {
                this.exploding = true;

            }
            else if (this.exploding && this.explosionTime == clock - 1)
            {
                this.exploding = false;
                this.stopExploding = true;
            }
            explosion(ref map, ref gameState, 0, 0);
            explosion(ref map, ref gameState, -1, 0);
            explosion(ref map, ref gameState, 0, -1);
            explosion(ref map, ref gameState, 1, 0);
            explosion(ref map, ref gameState, 0, 1);
            if (this.stopExploding)
            {
                this.stopExploding = false;
                this.destroyBomb = true;
            }
        }
        public void explosion(ref int[,] map, ref int[,] gameState, int j, int i)
        {


            int posY = Convert.ToInt32(this.position.X) + j;
            int posX = Convert.ToInt32(this.position.Y) + i;

            if (!(posX < 0 || posX > 15 || posY < 0 || posY > 15))
            {
                if (this.exploding)
                {
                    if (map[posX, posY] == 3)
                    {
                        map[posX, posY] = 2;
                    }
                    gameState[posX, posY] = 2;

                }
                if (this.stopExploding)
                {
                    gameState[posX, posY] = 0;
                }

            }

        }
    }
}