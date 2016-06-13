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
    class Bomb
    {
        public int owner;
        public long explosionTime;
        private bool exploding = false;
        private bool stopExploding = false;
        public bool destroyBomb = false;
        public bool penetration;
        public int bombRange;

        private Vector2i position;

        public int increaseRange = 0;
        public int increaseBombCount = 0;
        public Bomb(long explosionTime, Vector2i position, int id, int explosionRange,bool penetration)
        {
            this.position = new Vector2i(Convert.ToInt32(position.X), Convert.ToInt32(position.Y));
            this.explosionTime = explosionTime;
            this.owner = id;
            this.bombRange = explosionRange;
            this.penetration = penetration;
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

            //
            for (int up = 1; up < this.bombRange; up++)
            {

                if (explosion(ref map, ref gameState, -up, 0))
                { break; }
            }
            //
            for (int down = 1; down < this.bombRange; down++)
            {
                if (explosion(ref map, ref gameState, down, 0))
                { break; }
            }
            //
            for (int left = 1; left < this.bombRange; left++)
            {
                if (explosion(ref map, ref gameState, 0, -left))
                { break; }
            }
            //
            for (int right = 1; right < this.bombRange; right++)
            {
                if (explosion(ref map, ref gameState, 0, right))
                { break; }
            }

            explosion(ref map, ref gameState, 0, 0);
            /*explosion(ref map, ref gameState, -1, 0);
            explosion(ref map, ref gameState, 0, -1);
            explosion(ref map, ref gameState, 1, 0);
            explosion(ref map, ref gameState, 0, 1);*/

            if (this.stopExploding)
            {
                this.stopExploding = false;
                this.destroyBomb = true;
            }
        }
        public bool explosion(ref int[,] map, ref int[,] gameState, int j, int i)
        {


            int posY = Convert.ToInt32(this.position.X) + j;
            int posX = Convert.ToInt32(this.position.Y) + i;

            bool hitWall = false;

            if (!(posX < 0 || posX > 15 || posY < 0 || posY > 15))
            {
                if (this.exploding)
                {
                    if (map[posX, posY] == 2 || map[posX, posY] == 3 || map[posX, posY] == 6 || map[posX, posY] == 7)
                    {
                        if (map[posX, posY] == 6)
                        { this.increaseBombCount++; }

                        else if (map[posX, posY] == 7)
                        { this.increaseRange++; }

                        else if (map[posX, posY]== 3)
                        { this.penetration = true; }

                        map[posX, posY] = 1;
                        hitWall = true;
                        if(this.penetration)
                        { hitWall = false; }
                        


                    }
                    else if (map[posX, posY] != 1)
                    {
                        hitWall = true;
                    }
                    if (map[posX, posY] != 0)
                        gameState[posX, posY] = 2;

                }
                if (this.stopExploding)
                {
                    gameState[posX, posY] = 0;
                    hitWall = false;
                }

            }

            return hitWall;

        }
    }
}
