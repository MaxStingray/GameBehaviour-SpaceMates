using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameBehaviour
{
    public class Selector: GameObject
    {
        private Board Board;
        SpriteBatch Spr;
        Texture2D Texture;
        public bool isVisible = false;
        private Vector2 center;
        public Node targetNode;
        public bool nodeSet;
        public Selector(Vector2 position, Vector2 rotation, float scale, string tag, Board board, SpriteBatch spr, Texture2D tex) : base (position, tag)
        {
            Position = position;
            Board = board;
            Spr = spr;
            Texture = tex;
            
        }
        public override void Update(GameTime gameTime)
        {
            if (isVisible)
            {
                HandleInput(gameTime);
                center = new Vector2((Position.X + Texture.Width / 2), (Position.Y + Texture.Height / 2));
                Centre = new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
                spriteBatch.Draw(Texture, Position, Color.White);
        }

        public void HandleInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))//temporary. Eventually we will use resistance etc
            {
                Position.Y -= 200f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position.X -= 200f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X += 200f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y += 200f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                targetNode = Board.NodeFromWorldPoint(center);
                nodeSet = true;
            }
        }
    }
}
