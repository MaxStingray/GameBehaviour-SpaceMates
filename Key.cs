﻿using System;
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
    public class Key : RigidBody2D
    {
        public RigidBody2D ObjRB;
        public bool heldByDrone = false;
        public bool heldByPlayer = false;
        Texture2D Texture;
        SpriteBatch SpriteBatch;

        public Key(RigidBody2D rb, Texture2D tex, SpriteBatch spriteBatch, Vector2 position, Vector2 rotation, float scale, string tag, bool isStatic, float friction) :
            base(position, rotation, scale, tag, isStatic, friction)
        {
            Texture = tex;
            SpriteBatch = spriteBatch;
            ObjRB = rb;
            ObjRB.Mass = 4;
            ObjRB.boxColl = new BoxCollider(Position, new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height), Texture.Width, Texture.Height);
            ObjRB.polygonColl = new PolygonCollider();
            SetPolygonPoints(ObjRB.polygonColl);
        }

        public override void OnCollision(Manifold man)
        {
            RigidBody2D collisionObj = man.A == (RigidBody2D)this ? man.B : man.A;//check which object we are

            if (collisionObj.Tag == "player")
            {
                heldByPlayer = true;
            }

            if (collisionObj.Tag == "drone")
            {
                heldByDrone = true;
            }

            base.OnCollision(man);
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

            //create it
            p.BuildEdges();
        }

        public override void Draw(SpriteBatch spr)
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            Position = ObjRB.Position;
            ObjRB.boxColl.topLeft = new Vector2(Position.X, Position.Y);
            ObjRB.boxColl.bottomRight = new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height);
            SetPolygonPoints(ObjRB.polygonColl);
        }
    }
}