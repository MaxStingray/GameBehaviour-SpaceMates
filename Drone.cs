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
    public class Drone : RigidBody2D
    {
        public RigidBody2D ObjRB;
        //public Node target;
        SpriteBatch Spr;
        //Astar PathFinder;
        public AIManager manager;
        Texture2D Texture;
        public Selector Selector;

        public Vector2 target;
        public Vector2[] path;
        int targetIndex;

        float maxVelocityX = 10;
        float maxVelocityY = 100;

        public Drone(Selector selector, Astar pathFinder, RigidBody2D rb, SpriteBatch spr, Texture2D tex, Vector2 position, Vector2 rotation, float scale, string tag, bool isStatic) : 
            base(position, rotation, scale, tag, isStatic)
        {
            ObjRB = rb;
            Spr = spr;
            Texture = tex;
            Selector = selector;
            ObjRB.boxColl = new BoxCollider(Position, new Vector2(Position.X + Texture.Width), Texture.Width, Texture.Height);
            ObjRB.Mass = 3;
            ObjRB.polygonColl = new PolygonCollider();
            SetPolygonPoints(ObjRB.polygonColl);
            
        }

        public void OnPathFound(Vector2[] newPath, bool pathSuccess)
        {
            if (pathSuccess)
            {
                path = newPath;
                Console.WriteLine(path.Count());
                FollowPath();
            }
        }

        void FollowPath()
        {
            if (path.Count() > 0)
            {
                Vector2 currentWaypoint = path[0];

                if (Position == currentWaypoint)
                {
                    targetIndex++;
                    //if (targetIndex >= path.Length)
                    //break;
                    if (targetIndex <= path.Length)
                    {
                        currentWaypoint = path[targetIndex];
                    }
                }

                if (path[targetIndex].X > Position.X)
                {
                    if (Velocity.X < maxVelocityX)
                        ObjRB.Velocity.X += 1f;
                }

                if (path[targetIndex].X < Position.X)
                {
                    if (Velocity.X > -maxVelocityX)
                        ObjRB.Velocity.X -= 1f;
                }
                if(path[targetIndex].X == Position.X)
                {
                    if (Velocity.X > 0)
                        ObjRB.Velocity.X -= 1f;
                    else if (Velocity.X < 0)
                        ObjRB.Velocity.X += 1f;
                    else
                        ObjRB.Velocity.X = 0;
                }

                if (path[targetIndex].Y + 5 < Position.Y)
                {
                    if (Velocity.Y >= -maxVelocityY)
                        ObjRB.Velocity.Y += -5f;
                }

            }
        }
        bool onlyOnce = false;
        public override void Update(GameTime gameTime)
        {
            if (target != null && !onlyOnce)
            {
                manager.RequestPath(Position, target, OnPathFound);
                //onlyOnce = true;
            }
           
            Position = ObjRB.Position;
            SetPolygonPoints(ObjRB.polygonColl);
            //if(target != null && Selector != null)
                //go there

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Spr.Draw(Texture, Position, Color.White);
        }

        void SetPolygonPoints(PolygonCollider p)
        {
            p.points.Clear();
            ///top of drone
            p.points.Add(new Vector2(Position.X + (Texture.Width / 2), Position.Y));
            //right side of drone
            p.points.Add(new Vector2(Position.X + Texture.Width, Position.Y + (Texture.Height / 2)));
            //bottom of drone
            p.points.Add(new Vector2(Position.X + (Texture.Width / 2), Position.Y + Texture.Height));
            //left side of drone
            p.points.Add(new Vector2(Position.X, Position.Y + (Texture.Height / 2)));

            //create it
            p.BuildEdges();
        }

        
    }
}
