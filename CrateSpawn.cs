using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameBehaviour
{
    public class CrateSpawn
    {
        private static Random randX = new Random();
        public List<Crate> crates = new List<Crate>();
        public bool timeToSpawn = false;
        SpriteBatch SpriteBatch;
        Texture2D Texture;
        //create a set of crates as physics objects with a random x and y value
        public CrateSpawn(SpriteBatch spr, Texture2D tex)
        {
            SpriteBatch = spr;
            Texture = tex;

            for (int i = 0; i <= 10; i++)
            {
                Crate crate = new Crate(new RigidBody2D(new Vector2(randX.Next(1310, 2500), randX.Next(100, 500)), 
                                        "crate", false, 2, 3),
                    SpriteBatch, Texture);
                crates.Add(crate);
            }
        }

        public void Draw(SpriteBatch _spr)
        {
            if (timeToSpawn)
            {
                foreach (Crate crate in crates)
                {
                    crate.Draw(_spr);
                }
            }
        }
    }
}
