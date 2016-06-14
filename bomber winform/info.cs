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
    class Info : Drawable
    {
        int[,] infoMap = {
            {12,12,12,12,12,12,12,12},
            {12,2,2,12,12,12,12,12},
            {12,2,2,12,12,12,12,12},
            {12,12,12,12,12,12,12,12},

        };

        struct PlayerInfo
        {
            public string name;
            public int maxBombCount;
            public int explosionRange;
            public int id;

            public Map display;
            public Map bombLimit;
            public Map explosionReach;
            public Sprite playerSkin;
            public Text nickName;
            public Text penetrative;
        }

        Font fontFace = new Font("resources/pixelFont.ttf");
        Text text;

        List<PlayerInfo> players = new List<PlayerInfo>();



        public Info(List<Player> players)
        {
            this.text = new Text("ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz", this.fontFace);
            this.text.CharacterSize = 16;
            this.text.Scale = new Vector2f(1.5f, 1.5f);
            //this.text.Color = new Color(255,100,10);
            //this.text.Style = Text.Styles.Bold;
            int i = 0;
            foreach (var player in players)
            {
                this.setData(player, i);
                i++;

            }


        }
        public void updateInfo(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                this.setData(players[i], i);
            }

        }

        private void setData(Player player, int i)
        {

            Vector2f startPosition = new Vector2f(512,128*i);

            PlayerInfo temp = new PlayerInfo();
            temp.name = player.name;
            temp.maxBombCount = player.bombCount;
            temp.explosionRange = player.bombRange;
            temp.id = player.id;

            temp.display = new Map();
            temp.display.generateMap(this.infoMap, startPosition);

            int[,] maxBombs = new int[1,player.maxBombCount];
            for (int k = 0; k < player.maxBombCount; k++)
            {
                maxBombs[0,k] = 4;
            }
            temp.bombLimit = new Map();
            temp.bombLimit.scale = new Vector2f(8,8);
            temp.bombLimit.generateMap(maxBombs,startPosition + new Vector2f(128,32));

            int[,] explosionRange = new int[1, player.bombRange-1];
            for (int k = 0; k < player.bombRange-1; k++)
            {
                explosionRange[0, k] = 5;
            }
            temp.explosionReach = new Map();
            temp.explosionReach.scale = new Vector2f(8, 8);
            temp.explosionReach.generateMap(explosionRange, startPosition + new Vector2f(128, 48));


            this.players.Add(temp);
        }

        virtual public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var player in this.players)
            {
                target.Draw(player.display);
                target.Draw(player.bombLimit);
                target.Draw(player.explosionReach);
            }

            target.Draw(this.text);

        }
    }
}
