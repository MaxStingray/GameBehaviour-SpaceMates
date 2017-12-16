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
    public class Player : GameObject
    {
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public RigidBody2D ObjRB { get; set; }
        public Vector2 Center;
        public Node PlayerNode;
        public Drone drone;
        public float maxVelocityX = 100f;
        public float maxVelocityY = 80f;

        public bool hasKey = false;

        public Player(RigidBody2D rb, Texture2D texture,
            SpriteBatch spriteBatch, float friction) : base (rb.Position, rb.Rotation, rb.Scale, rb.Tag)
        {
            
            Texture = texture;
            SpriteBatch = spriteBatch;
            ObjRB = rb;
            rb.spr = spriteBatch;
            ObjRB.boxColl = new BoxCollider(new Vector2(Position.X, Position.Y),
               new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height), texture.Width, texture.Height);
            ObjRB.polygonColl = new PolygonCollider();
            ObjRB.Friction = friction;
            ObjRB.parent = this;
            SetPolygonPoints(ObjRB.polygonColl);
            PlayerNode = new Node();
        }

        void SetPolygonPoints(PolygonCollider p)
        {
            p.points.Clear();
            ///top of head
            p.points.Add(new Vector2(Position.X + (Texture.Width / 2), Position.Y));
            //right side of head
            p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y + (Texture.Height / 4)));
            //bottom right corner
            p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height));
            //bottom left corner
            p.points.Add(new Vector2(Position.X, Position.Y + Texture.Height));
            //left side of head
            p.points.Add(new Vector2(Position.X, Position.Y + (Texture.Height / 4)));

            //p.points.Add(Position);
            //p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y));
            //p.points.Add(new Vector2(Position.X, Position.Y + (Texture.Height / 2)));

            //create it
            p.BuildEdges();
        }

        public override void OnCollision(Manifold man)
        {
            /*RigidBody2D collisionObj = man.A == (RigidBody2D)this ? man.B : man.A;//check which object we are

            if (collisionObj.Tag == "key")
            {
                hasKey = true;
                Console.WriteLine("hit key");
            }

            if (collisionObj.Tag == "drone")
            {
                Console.WriteLine("hit drone");
                if (!hasKey)
                    return;
                else
                    if (!drone.hasKey)
                {
                    drone.hasKey = true;
                    hasKey = false;
                }
            }*/
        }

        public override void Draw(SpriteBatch spr)
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            ObjRB.boxColl.topLeft = new Vector2(Position.X, Position.Y);
            ObjRB.boxColl.bottomRight = new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height);
            SetPolygonPoints(ObjRB.polygonColl);
            Position = ObjRB.Position;
            Center = new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
            PlayerNode.worldPosition = Center;
            HandleInput();
            
        }

        public void HandleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))//temporary. Eventually we will use resistance etc
            {
                if (ObjRB.Velocity.X <= maxVelocityX)
                {
                    ObjRB.Velocity.X += 10f;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (ObjRB.Velocity.X >= -maxVelocityX)
                {
                    ObjRB.Velocity.X -= 10f;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (ObjRB.Velocity.Y >= -maxVelocityY)
                {
                    ObjRB.Velocity.Y += -10f;
                }
            }
            //else
            //{
                //ObjRB.Velocity.X = 0f;//not accurate, needs a calculation to slow velocity
                                      //in real life velocity does not stop on a dime
            //}

            //input handling logic goes here
        }
    }
}
