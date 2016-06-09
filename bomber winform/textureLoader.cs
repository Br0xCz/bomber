using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace bomber_winform
{
    class TextureLoader
    {
        public TextureLoader(short x, short y)
        {
            int size = 16;
            Texture texture=  new Texture("resources/tileset.png",new IntRect(new Vector2i(x,y),new Vector2i(size,size) ));
        }
    }
}
