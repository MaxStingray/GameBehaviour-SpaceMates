using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameBehaviour
{
    class Menu : GameObject
    {
        SpriteBatch spr;
        Texture2D tex;

        public bool startButtonPushed = false;

        public Menu(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, string tag) : base(position, tag)
        {
            spr = spriteBatch;
            tex = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spr.Draw(tex, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
