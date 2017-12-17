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
    public class RigidBody2D : GameObject
    {
        public bool IsStatic;
        public Sprite[] pointMarkers;
        Texture2D colTex;
        Texture2D lineTex;
        public float Friction;
        Vector2 RBCentre;
        public Vector2 Velocity;

        public GameObject parent;
        public float Mass {get; set;} //set mass from inherited objects, default is 1
        public Vector2 Gravity = new Vector2(0, 1) * 9.81f;//more accurate, won't work without coefficients
        public BoxCollider boxColl;
        public float inverseMass;
        public SpriteBatch spr;
        public PolygonCollider polygonColl;

        public float LinearDrag = 0;

        Vector2 lastPosition;
        const float minPosChange = 0.2f;
        //rigidbody class inherits from gameobject, all physics objects are rigidbody

        public RigidBody2D(Vector2 position, Vector2 rotation, float scale, string tag, bool isStatic, float friction, float mass) 
            : base (position, rotation, scale, tag) 
        {
            IsStatic = isStatic;
            Mass = mass;
            inverseMass = 1 / Mass;
            Friction = friction;
        }

        public override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!IsStatic)
            {

                
                Velocity *= (1 - LinearDrag);
                Vector2 newPos = Position += (Velocity * delta);
                if (Math.Abs(newPos.X - lastPosition.X) > minPosChange || Math.Abs(newPos.Y - lastPosition.Y) > minPosChange && Tag != "drone")
                    Position += (Velocity * delta);
                else
                    Position = lastPosition;
                if (Tag != "drone")
                    Velocity += Gravity * 1 / Mass;
                //no gravity for drones until hovering logic is set

            }
            else
            {
                Velocity.X = 0;
                Velocity.Y = 0;
            }
            if (polygonColl != null)
            {
                polygonColl.pos = Position;
            }
            lastPosition = Position;
            //Draw();
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (polygonColl != null)
            {
                Vector2 p1;
                Vector2 p2;
                for (int i = 0; i < polygonColl.points.Count; i++)
                {
                    p1 = polygonColl.points[i];
                    if (i + 1 >= polygonColl.points.Count)
                        p2 = polygonColl.points[0];
                    else
                        p2 = polygonColl.points[i + 1];

                    DrawDebugLine(spriteBatch, p1, p2);
                }
            }
        }

        void DrawDebugLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            if (lineTex == null)
            {
                lineTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                lineTex.SetData(new Color[] { Color.White });
            }
            
            Vector2 edge = end - start;
            //calculate line's angle
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(lineTex, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 3), null, Color.Red, angle,
                new Vector2(0,0), SpriteEffects.None, 0);
        }
    }
}
