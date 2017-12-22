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

        bool usingJetPack;
        public bool hasKey = false;

        public float currentJetPackFuel;
        private float maxJetPackFuel = 100;

        public Player(RigidBody2D rb, Texture2D texture,
            SpriteBatch spriteBatch, float friction) : base (rb.Position, rb.Tag)
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
            currentJetPackFuel = maxJetPackFuel;
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
            //check which object we are
            RigidBody2D other = man.B == this.ObjRB ? man.A : man.B;

            if (man.Normal.Y <= 0 && !usingJetPack)
            {
                if (other.Tag == "movingPlatform")
                    ObjRB.Position.Y = other.Position.Y - 50;
            }

            if (other.Tag == "key")
            {
                hasKey = true;
                other.Position = Position;
            }

            base.OnCollision(man);
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
            Centre = new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
            PlayerNode.worldPosition = Center;
            if (!usingJetPack && currentJetPackFuel < maxJetPackFuel)
            {
                currentJetPackFuel += 1f;
            }
            HandleInput(gameTime);
            
        }

        public void HandleInput(GameTime gameTime)
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
                    if (currentJetPackFuel > 0)
                    {
                        usingJetPack = true;
                        ObjRB.Velocity.Y += -10f;
                        currentJetPackFuel -= 4f;
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.W))
            {
                usingJetPack = false;
            }
        }
    }
}
