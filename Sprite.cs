using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameBehaviour
{
    public class Sprite : GameObject
    {
        public Texture2D tex;
        public SpriteBatch spr;


        public Sprite(Vector2 position, Vector2 rotation, float scale, string tag, Texture2D texture, SpriteBatch spriteBatch)
            :base (position, rotation, scale, tag)
        {
            tex = texture;
            spr = spriteBatch;
        }

        public virtual void Draw()
        {
            spr.Draw(tex, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
