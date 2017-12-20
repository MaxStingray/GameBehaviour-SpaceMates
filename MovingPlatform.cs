using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameBehaviour
{
    class MovingPlatform : GameObject
    {
        Texture2D Texture;
        SpriteBatch SpriteBatch;
        public RigidBody2D ObjRB;

        float startYpos;
        float targetYpos;

        float speed = 100f;
        Vector2 target;
        Vector2 start;
        public MovingPlatform(RigidBody2D rb, SpriteBatch spr, Texture2D tex, float targetY) : base(rb.Position, rb.Rotation, rb.Scale, rb.Tag)
        {
            Texture = tex;
            SpriteBatch = spr;
            ObjRB = rb;
            ObjRB.parent = this;
            ObjRB.boxColl = new BoxCollider(Position, new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height),
                Texture.Width, Texture.Height);
            ObjRB.polygonColl = new PolygonCollider();
            SetPolygonPoints(ObjRB.polygonColl);
            startYpos = Position.Y;
            targetYpos = targetY;
            target = new Vector2(Position.X, targetYpos);
            start = Position;
        }

        void SetPolygonPoints(PolygonCollider p)
        {
            p.points.Clear();
            ///top left of platform
            p.points.Add(new Vector2(Position.X, Position.Y + (Texture.Height / 3)));
            //top right of platform
            p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y + (Texture.Height / 3)));
            //bottom right of platform
            p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y + (Texture.Height)));
            //bottom left of platform
            p.points.Add(new Vector2(Position.X, Position.Y + (Texture.Height)));
            //create it
            p.BuildEdges();
        }
        public override void Update(GameTime gameTime)
        {
            if (target.Y - Position.Y <= 0)
            {
                Vector2 dir = Vector2.Normalize(target - Position);
                dir *= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Console.WriteLine(target.Y - Position.Y);
                ObjRB.Position += dir;
            }
            else
            {
                target = start;
                Vector2 dir = Vector2.Normalize(target - Position);
                dir *= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Console.WriteLine(target.Y - Position.Y);
                ObjRB.Position += dir;
            }

            ObjRB.boxColl.topLeft = new Vector2(Position.X, Position.Y);
            ObjRB.boxColl.bottomRight = new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height);
            SetPolygonPoints(ObjRB.polygonColl);
            Position = ObjRB.Position;
        }

        public override void OnCollision(Manifold man)
        {
            //base.OnCollision(man);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

    }
}
