﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace GameBehaviour
{
    public abstract class GameObject
    {
        public Vector2 Position; //{ get; set; }
        public Vector2 Rotation { get; set; }
        public float Scale { get; set; }
        public string Tag { get; set; }//unsure if tag should be a required variable, may create duplicate code
        public Vector2 Centre;
        public GameObject(Vector2 position, string tag)            
        {
            Position = position;
            Tag = tag;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch) {
            //empty
        }

        public virtual void OnCollision(Manifold man)
        {

        }
        
    }
}
