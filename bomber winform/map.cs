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
    class Map : Drawable
    {

        private Texture tilesetTexture = new Texture("resources/tileset.png");

        public short tileSize = 16;
        public short textureScale = 2;
        public Vector2f scale = new Vector2f(16.0f, 16.0f);

        public VertexArray tileMap = new VertexArray(PrimitiveType.Quads, 1024);
        public VertexArray bombMap = new VertexArray(PrimitiveType.Quads, 1024);

        public Transform tranform = new Transform();

        public Map()
        {
        }

        public void generateMap(int[,] map, Vector2f offset)
        {


            uint index = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    uint indexMod = index * 4;

                    float positionX = (j) * (this.scale.X * this.textureScale) + offset.X;
                    float positionY = (i) * (this.scale.Y * this.textureScale) + offset.Y;

                    float tu = (map[i, j] % 4) * this.tileSize;
                    float tv = ((map[i, j] - map[i, j] % 4) / 4) * this.tileSize;

                    this.tileMap[indexMod + 0] = new Vertex(new Vector2f(positionX, positionY), new Vector2f(tu, tv));
                    this.tileMap[indexMod + 1] = new Vertex(new Vector2f(positionX + (this.scale.X * this.textureScale), positionY), new Vector2f(tu + this.tileSize, tv));
                    this.tileMap[indexMod + 2] = new Vertex(new Vector2f(positionX + (this.scale.X * this.textureScale), positionY + (this.scale.Y * this.textureScale)), new Vector2f(tu + this.tileSize, tv + this.tileSize));
                    this.tileMap[indexMod + 3] = new Vertex(new Vector2f(positionX, positionY + (this.scale.Y * this.textureScale)), new Vector2f(tu, tv + this.tileSize));
                    index++;
                }
            }
        }
        public int[,] generateBonus(int[,] map)
        {
            Random rnd = new Random();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (map[i, j] == 2)
                    {
                        if (rnd.Next(20) > 16)
                        {
                            map[i, j] = rnd.Next(2) + 6;
                            if (rnd.Next(5) == 2)
                                map[i, j] = 3;
                        }
                    }
                }
            }
            return map;
        }

        public void generateBombMap(int[,] map)
        {
            uint index = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    uint indexMod = index * 4;

                    float positionX = (j) * (this.tileSize * this.textureScale);
                    float positionY = (i) * (this.tileSize * this.textureScale);

                    float tu, tv;


                    if (map[i, j] == 1)
                    {
                        tu = 0 * this.tileSize;
                        tv = 1 * this.tileSize;
                    }
                    else if (map[i, j] == 2)
                    {
                        tu = 1 * this.tileSize;
                        tv = 1 * this.tileSize;
                    }
                    else
                    {
                        tu = 3 * this.tileSize;
                        tv = 3 * this.tileSize;

                    }
                    this.bombMap[indexMod + 0] = new Vertex(new Vector2f(positionX, positionY), new Vector2f(tu, tv));
                    this.bombMap[indexMod + 1] = new Vertex(new Vector2f(positionX + (this.tileSize * this.textureScale), positionY), new Vector2f(tu + this.tileSize, tv));
                    this.bombMap[indexMod + 2] = new Vertex(new Vector2f(positionX + (this.tileSize * this.textureScale), positionY + (this.tileSize * this.textureScale)), new Vector2f(tu + this.tileSize, tv + this.tileSize));
                    this.bombMap[indexMod + 3] = new Vertex(new Vector2f(positionX, positionY + (this.tileSize * this.textureScale)), new Vector2f(tu, tv + this.tileSize));
                    index++;
                }
            }
        }
        virtual public void Draw(RenderTarget target, RenderStates states)
        {
            states.Texture = this.tilesetTexture;
            //states.Transform = this.tranform;
            target.Draw(this.tileMap, states);
            target.Draw(this.bombMap, states);
        }
    }
}