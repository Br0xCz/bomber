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
            {0,0,0,0,0,0,0,0},
            {0,13,13,12,12,12,12,0},
            {0,13,13,12,12,12,12,0},
            {0,12,12,12,12,12,12,0},

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

        List<PlayerInfo> players = new List<PlayerInfo>();


        public Info(List<Player> players)
        {
            /*this.text = new Text("...", this.fontFace);
            this.text.CharacterSize = 16;
            this.text.Scale = new Vector2f(1.5f, 1.5f);*/
            //this.text.Color = new Color(255,100,10);
            //this.text.Style = Text.Styles.Bold;
            int i = 0;
            foreach (var player in players)
            {
                PlayerInfo temp= this.setData(player, i);
                this.players.Add(temp);
                //this.text.DisplayedString = Convert.ToString(i);
                i++;

            }


        }
        public void updateInfo(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                PlayerInfo temp= this.setData(players[i], i);
                this.players[i] = temp;
            }

        }

        private PlayerInfo setData(Player player, int i)
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

            Texture playerTexture = new Texture("resources/tileset.png",new IntRect(new Vector2i(16*player.id,32),new Vector2i(16,16)));
            temp.playerSkin = new Sprite(playerTexture);
            temp.playerSkin.Position = new Vector2f(32+8,32+8) + startPosition;
            temp.playerSkin.Scale = new Vector2f(3.0f, 3.0f);

            temp.nickName = new Text();
            temp.nickName.Font = this.fontFace;
            temp.nickName.DisplayedString = player.name;
            temp.nickName.Position = new Vector2f(40,90) + startPosition;

            return temp;
        }

        virtual public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var player in this.players)
            {
                target.Draw(player.display);
                target.Draw(player.bombLimit);
                target.Draw(player.explosionReach);
                target.Draw(player.playerSkin);
                target.Draw(player.nickName);
            }


        }
    }
}
