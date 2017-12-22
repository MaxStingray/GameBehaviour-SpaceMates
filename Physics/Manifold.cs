using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameBehaviour
{
    public class Manifold
    {
        public RigidBody2D A, B;//two objects to be tested
        public bool WillIntersect;
        public bool Intersect;
        public Vector2 MinTranslation;
        public float Penetration; //depth of penetration ;)
        public Vector2 Normal; //the normal
        public Vector2 Direction;
    }
}
