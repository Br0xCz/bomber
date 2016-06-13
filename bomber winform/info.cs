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
            {12,1,1,12,12,12,12,12},
            {12,1,1,12,12,12,12,12},
            {12,12,12,12,12,12,12,12},

        };

        struct PlayerInfo
        {
            public string name;
            public int maxBombCount;
            public int explosionRange;
            public int id;
        }

        Font fontFace = new Font("resources/pixelFont.ttf");
        Text text;

        List<PlayerInfo> players = new List<PlayerInfo>();
        List<Map> display=new List<Map>();
        public Info(List<Player> players)
        {
            this.text = new Text("name: Kadinko\nmax bomb count: 3\nexplosion range: 4\npenetration: no", this.fontFace);
            this.text.CharacterSize = 16;
            this.text.Scale = new Vector2f(1.5f, 1.5f);
            //this.text.Color = new Color(255,100,10);
            //this.text.Style = Text.Styles.Bold;
            int i = 0;
            foreach (var player in players)
            {
                PlayerInfo temp = new PlayerInfo();
                temp.name = player.name;
                temp.maxBombCount = player.bombCount;
                temp.explosionRange = player.bombRange;
                temp.id = player.id;
                this.players.Add(temp);

                Map tempMap = new Map();
                tempMap.generateMap(this.infoMap);
                tempMap.tranform.Translate(new Vector2f(512, 128 * i));
                i++;
                this.display.Add(tempMap);
            }


        }
        public void updateInfo(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                PlayerInfo temp = new PlayerInfo();
                temp.name = players[i].name;
                temp.maxBombCount = players[i].bombCount;
                temp.explosionRange = players[i].bombRange;
                temp.id = players[i].id;
                this.players[i] = temp;
            }

        }
        virtual public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var map in display)
            {
                target.Draw(map);
            }

            target.Draw(this.text);

        }
    }
}
