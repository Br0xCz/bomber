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

        PlayerInfo[] players;

        public Info(List<Player> players)
        {
            /*this.text = new Text("...", this.fontFace);
            this.text.CharacterSize = 16;
            this.text.Scale = new Vector2f(1.5f, 1.5f);*/
            //this.text.Color = new Color(255,100,10);
            //this.text.Style = Text.Styles.Bold;

            this.players = new PlayerInfo[players.Count];
            for (int k = 0; k < this.players.Length; k++)
            {
                this.players[k] = new PlayerInfo();
            }

            int i = 0;
            foreach (var player in players)
            {
                this.setData(player, i);
                this.update(players[i], i);
                //this.text.DisplayedString = Convert.ToString(i);
                i++;

            }


        }
        public void updateInfo(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                
                
                this.update(players[i],i);
            }

        }

        Vector2f startPosition = new Vector2f(512, 0);
        private void update(Player player, int i)
        {
            

            int[,] maxBombs = new int[1, player.maxBombCount];
            for (int k = 0; k < player.maxBombCount; k++)
            {
                maxBombs[0, k] = 4;
            }
            
            this.players[i].bombLimit.generateMap(maxBombs, this.startPosition + new Vector2f(128, 32+i*128));

            int[,] explosionRange = new int[1, player.bombRange - 1];
            for (int k = 0; k < player.bombRange - 1; k++)
            {
                explosionRange[0, k] = 5;
            }
           
            this.players[i].explosionReach.generateMap(explosionRange, this.startPosition + new Vector2f(128,128*i+ 48));

        }
        private void setData(Player player, int i)
        {
            Vector2f startPosition = new Vector2f(512, 128 * i);

            this.players[i].name = player.name;
            this.players[i].maxBombCount = player.bombCount;
            this.players[i].explosionRange = player.bombRange;
            this.players[i].id = player.id;
            this.players[i].display = new Map();
            this.players[i].display.generateMap(this.infoMap, startPosition);

            Texture playerTexture = new Texture("resources/tileset.png",new IntRect(new Vector2i(16*player.id,32),new Vector2i(16,16)));
            this.players[i].playerSkin = new Sprite(playerTexture);
            this.players[i].playerSkin.Position = new Vector2f(32+8,32+8) + startPosition;
            this.players[i].playerSkin.Scale = new Vector2f(3.0f, 3.0f);

            this.players[i].nickName = new Text();
            this.players[i].nickName.Font = this.fontFace;
            this.players[i].nickName.DisplayedString = player.name;
            this.players[i].nickName.Position = new Vector2f(40,90) + startPosition;
            this.players[i].nickName.Color = new Color(130, 58, 18);

            this.players[i].bombLimit = new Map();
            this.players[i].bombLimit.scale = new Vector2f(8, 8);

            this.players[i].explosionReach = new Map();
            this.players[i].explosionReach.scale = new Vector2f(8, 8);
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
