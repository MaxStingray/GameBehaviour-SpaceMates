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
    public class Player : RigidBody2D//player class will inherit from this with input handling etc
    {
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public RigidBody2D ObjRB { get; set; }
        public Vector2 Center;
        public Node PlayerNode;
        public float maxVelocityX = 100f;
        public float maxVelocityY = 80f;


        public Player(RigidBody2D rb, Vector2 position, Vector2 rotation, float scale, string tag, bool isStatic, float mass, Texture2D texture,
            SpriteBatch spriteBatch) : base (position, rotation, scale, tag, isStatic)
        {
            
            Texture = texture;
            SpriteBatch = spriteBatch;
            ObjRB = rb;
            ObjRB.Mass = mass;
            rb.spr = spriteBatch;
            ObjRB.boxColl = new BoxCollider(new Vector2(Position.X, Position.Y),
               new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height), texture.Width, texture.Height);
            ObjRB.polygonColl = new PolygonCollider();
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
