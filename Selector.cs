using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameBehaviour
{
    public class Selector: GameObject
    {
        
        private Board Board;
        SpriteBatch Spr;
        Texture2D Texture;
        
        public Selector(Vector2 position, Vector2 rotation, float scale, string tag, Board board, SpriteBatch spr, Texture2D tex) : base (position, rotation, scale, tag)
        {
            Position = position;
            Board = board;
            Spr = spr;
            Texture = tex;
        }
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
