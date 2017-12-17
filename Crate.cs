using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameBehaviour
{
    public class Crate : GameObject
    {
        public SpriteBatch SpriteBatch;
        public Texture2D Texture;
        public RigidBody2D ObjRB;

        public Crate(RigidBody2D rb, SpriteBatch spriteBatch, Texture2D texture) : base(rb.Position, rb.Rotation, rb.Scale, rb.Tag)
        {
            ObjRB = rb;
            SpriteBatch = spriteBatch;
            Texture = texture;
            ObjRB.boxColl = new BoxCollider(Position, new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height), Texture.Width, Texture.Height);
            ObjRB.polygonColl = new PolygonCollider();
            ObjRB.parent = this;
            SetPolygonPoints(ObjRB.polygonColl);
            ObjRB.Velocity.X += 10;
        }

        public override void Update(GameTime gameTime)
        {
            Position = ObjRB.Position;
            ObjRB.boxColl.topLeft = new Vector2(Position.X, Position.Y);
            ObjRB.boxColl.bottomRight = new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height);
            SetPolygonPoints(ObjRB.polygonColl);
        }

        void SetPolygonPoints(PolygonCollider p)
        {
            p.points.Clear();
            //top left
            p.points.Add(new Vector2(Position.X, Position.Y));
            //top right
            p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y));
            //bottom right
            p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height));
            //bottom left 
            p.points.Add(new Vector2(Position.X, Position.Y + Texture.Height));
            //create it
            p.BuildEdges();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

        public override void OnCollision(Manifold man)
        {
            //do nothing
        }
    }
}
