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
    public class BoxCollider
    {
        public Vector2 topLeft { get; set; }
        public Vector2 bottomRight { get; set; }
        public Vector2 topRight;
        public Vector2 bottomLeft;

        public float width { get; set; }
        public float height { get; set; }

        public BoxCollider(Vector2 Tleft, Vector2 Bright, float W, float H)
        {
            topLeft = Tleft;
            bottomRight = Bright;
            width = W;
            height = H;
            topRight = new Vector2(Tleft.X + W, Bright.Y);
            bottomLeft = new Vector2(Tleft.X, Bright.Y);
        }

    }
}
