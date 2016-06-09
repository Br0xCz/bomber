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
            {2,2,3,3,3,3,3,3, 3,3,3,3,3,3,2,2},
            {2,3,3,1,3,3,1,3, 3,1,3,3,1,3,3,2},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,3,1,3, 3,1,3,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,1,1,3, 3,1,1,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,3,1,2, 2,1,3,3,1,3,1,3},

            {3,1,3,1,3,3,1,2, 2,1,3,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,1,1,3, 3,1,1,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {3,1,3,1,3,3,1,3, 3,1,3,3,1,3,1,3},
            {3,3,3,3,3,3,3,3, 3,3,3,3,3,3,3,3},
            {2,3,3,1,3,3,1,3, 3,1,3,3,1,3,3,2},
            {2,2,3,3,3,3,3,3, 3,3,3,3,3,3,2,2}
        };
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
            Keyboard.Key[] player4controls = { Keyboard.Key.Num8, Keyboard.Key.Num5, Keyboard.Key.Num4, Keyboard.Key.Num6, Keyboard.Key.Num9 };

            RenderWindow window = new RenderWindow(new VideoMode(512, 512), "Bomber");
            window.Closed += new EventHandler(OnClose);
            window.SetFramerateLimit(16);


            Player player1 = new Player(0, 0, mapArray, player1controls, 0);
            Player player2 = new Player(15, 15, mapArray, player2controls, 1);
            Player player3 = new Player(15, 0, mapArray, player3controls, 2);
            Player player4 = new Player(0, 15, mapArray, player4controls, 3);

            Map map = new Map();
            map.generateMap(mapArray);

            Color windowColor = new Color(0, 0, 0);

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear(windowColor);

                map.generateBombMap(gameState);
                window.Draw(map);


                gameState = player1.input(gameState, ref bombs, clock);
                gameState = player2.input(gameState, ref bombs, clock);
                gameState = player3.input(gameState, ref bombs, clock);
                gameState = player4.input(gameState, ref bombs, clock);

                if (!player1.dead)
                { window.Draw(player1.player); }
                if (!player2.dead)
                { window.Draw(player2.player); }
                if (!player3.dead)
                { window.Draw(player3.player); }
                if (!player4.dead)
                { window.Draw(player4.player); }

                window.Display();

                clockPeriod++;
                if (clockPeriod == 16)
                {
                    clock++;
                    clockPeriod = 0;
                    int index = 0;
                    foreach (var bomb in bombs)
                    {
                        bomb.checkExplosion(clock, ref mapArray, ref gameState);
                        if (bomb.destroyBomb)
                        {
                            if (bomb.owner == player1.id)
                            {
                                player1.bombCount--;
                            }
                            if (bomb.owner == player2.id)
                            {
                                player2.bombCount--;
                            }
                            if (bomb.owner == player3.id)
                            {
                                player3.bombCount--;
                            }
                            if (bomb.owner == player4.id)
                            {
                                player4.bombCount--;
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