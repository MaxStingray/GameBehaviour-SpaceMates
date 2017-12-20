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

        float speed = 50f;
        Vector2 target;
        Vector2 ogTarget;
        Vector2 start;
        bool flipTarget = false;
        bool timeToMove = false;

        //TODO: set a list of nodes on each part of this sprite and set them to be intraversible
        float delta;
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
            ogTarget = target;
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
            Vector2 distanceFromTarget = new Vector2(Math.Abs(target.X - Position.X), Math.Abs(target.Y - Position.Y));
            delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeToMove)
            {
                if (distanceFromTarget.Y <= 10)
                {
                    if (flipTarget)
                        flipTarget = false;
                    else
                        flipTarget = true;
                }
                if (!flipTarget)
                {
                    target = ogTarget;
                }
                else
                {
                    target = start;
                }
                Move(target);
            }
            ObjRB.boxColl.topLeft = new Vector2(Position.X, Position.Y);
            ObjRB.boxColl.bottomRight = new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height);
            SetPolygonPoints(ObjRB.polygonColl);
            Position = new Vector2(start.X, ObjRB.Position.Y);
        }

        void Move(Vector2 target)
        {
            Vector2 dir = Vector2.Normalize(target - Position);
            dir *= speed * delta;
            ObjRB.Position += dir;
        }

        public override void OnCollision(Manifold man)
        {
            RigidBody2D other = man.B == this.ObjRB ? man.A : man.B;
            if(other.Tag == "key")
                timeToMove = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!timeToMove)
                SpriteBatch.Draw(Texture, Position, Color.Red);
            else
                SpriteBatch.Draw(Texture, Position, Color.White);
        }

    }
}
