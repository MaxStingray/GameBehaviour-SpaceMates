using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameBehaviour
{
    class Exit : GameObject
    {
        public SpriteBatch SpriteBatch;
        public Texture2D Texture;

        public Player playerRef;
        public bool levelOver;

        float distance;

        public Exit(SpriteBatch spr, Texture2D tex, Player player, Vector2 position, Vector2 rotation, float scale, string tag)
            : base(position, rotation, scale, tag)
        {
            SpriteBatch = spr;
            Texture = tex;
            playerRef = player;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            distance = Position.X - playerRef.Position.X;

            if (distance <= 0)
                levelOver = true;
        }
    }
}
