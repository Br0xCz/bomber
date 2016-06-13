using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace bomber_winform
{
    static class Program
    {


        static int[,] mapArray = {
            {1,1,2,2,2,2,2,2, 2,2,2,2,2,2,1,1},
            {1,2,2,0,2,2,0,2, 2,0,2,2,0,2,2,1},
            {2,2,2,2,2,2,2,2, 2,2,2,2,2,2,2,2},
            {2,0,2,0,2,2,0,2, 2,0,2,2,0,2,0,2},
            {2,2,2,2,2,2,2,2, 2,2,2,2,2,2,2,2},
            {2,0,2,0,2,0,0,2, 2,0,0,2,0,2,0,2},
            {2,2,2,2,2,2,2,2, 2,2,2,2,2,2,2,2},
            {2,0,2,0,2,2,0,0, 0,0,2,2,0,2,0,2},

            {2,0,2,0,2,2,0,7, 7,0,2,2,0,2,0,2},
            {2,2,2,2,2,2,2,2, 2,2,2,2,2,2,2,2},
            {2,0,2,0,2,0,0,2, 2,0,0,2,0,2,0,2},
            {2,2,2,2,2,2,2,2, 2,2,2,2,2,2,2,2},
            {2,0,2,0,2,2,0,2, 2,0,2,2,0,2,0,2},
            {2,2,2,2,2,2,2,2, 2,2,2,2,2,2,2,2},
            {1,2,2,0,2,2,0,2, 2,0,2,2,0,2,2,1},
            {1,1,2,2,2,2,2,2, 2,2,2,2,2,2,1,1}
        };
        /*
            {2,2,3,3,3,3,3,3, 3,3,3,3,3,3,2,2},
            {2,3,3,1,3,3,1,3, 3,1,3,3,1,3,3,2},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,3,1,3, 3,1,3,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,1,1,3, 3,1,1,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,3,1,6, 6,1,3,3,1,3,1,3},

            {3,1,3,1,3,3,1,7, 7,1,3,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,1,1,3, 3,1,1,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,3,1,3, 3,1,3,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {2,3,3,1,3,3,1,3, 3,1,3,3,1,3,3,2},
            {2,2,3,3,3,3,3,3, 3,3,3,3,3,3,2,2}
            */
        static int[,] gameState = new int[16, 16];
        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static void makeGameState()
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    gameState[j, i] = 0;
                }
            }
        }
        static void Main()
        {

            long clock = 0;
            int clockPeriod = 0;
            List<Bomb> bombs = new List<Bomb>();

            makeGameState();

            Keyboard.Key[] player1controls = { Keyboard.Key.Up, Keyboard.Key.Down, Keyboard.Key.Left, Keyboard.Key.Right, Keyboard.Key.Space };
            Keyboard.Key[] player2controls = { Keyboard.Key.W, Keyboard.Key.S, Keyboard.Key.A, Keyboard.Key.D, Keyboard.Key.E };
            Keyboard.Key[] player3controls = { Keyboard.Key.I, Keyboard.Key.K, Keyboard.Key.J, Keyboard.Key.L, Keyboard.Key.O };
            Keyboard.Key[] player4controls = { Keyboard.Key.T, Keyboard.Key.G, Keyboard.Key.F, Keyboard.Key.H, Keyboard.Key.Z };

            RenderWindow window = new RenderWindow(new VideoMode(768, 512), "Bomber");
            window.Closed += new EventHandler(OnClose);
            window.SetFramerateLimit(16);

            List<Player> players = new List<Player>();
            players.Add(new Player(0, 0, mapArray, player1controls, 0, "Petko"));
            players.Add(new Player(15, 15, mapArray, player2controls, 1, "The Zola Men"));
            players.Add(new Player(15, 0, mapArray, player3controls, 2, "Muni"));
            players.Add(new Player(0, 15, mapArray, player4controls, 3, "Jarda"));

            Map map = new Map();
            mapArray = map.generateBonus(mapArray);
            map.generateMap(mapArray);

            Color windowColor = new Color(0, 0, 0);

            Info info = new Info(players);

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear(windowColor);

                map.generateBombMap(gameState);


                window.Draw(map);

                foreach (var player in players)
                {
                    gameState = player.input(gameState, ref bombs, clock);
                }


                foreach (var player in players)
                {
                    if (!player.dead)
                    {
                        window.Draw(player.player);
                    }
                }
                window.Draw(info);
                window.Display();

                clockPeriod++;
                if (clockPeriod == 4)
                {
                    clock++;
                    clockPeriod = 0;
                    int index = 0;
                    foreach (var bomb in bombs)
                    {
                        bomb.checkExplosion(clock, ref mapArray, ref gameState);
                        if (bomb.destroyBomb)
                        {
                            foreach (var player in players)
                            {
                                if (bomb.owner == player.id)
                                {
                                    player.bombRange += bomb.increaseRange;
                                    player.maxBombCount += bomb.increaseBombCount;
                                    player.penetration = bomb.penetration;
                                    player.bombCount--;
                                }
                            }

                            bomb.destroyBomb = false;
                        }
                        //bombs.RemoveAt(index);
                        index++;
                    }
                    map.generateMap(mapArray);


                }

            }
        }
    }
}