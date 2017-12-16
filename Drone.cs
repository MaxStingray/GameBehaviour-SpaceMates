using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameBehaviour
{
    public class Drone : GameObject
    {
        public RigidBody2D ObjRB;
        SpriteBatch Spr;
        public AIManager Manager;
        Texture2D Texture;
        public Selector Selector;
        public Player Player;

        float delta;

        bool pathComplete;
        public Vector2 target;
        public Vector2[] path;
        int targetIndex;

        public bool hasKey = false;

        public float nextWaypointDistance = 3;

        public float speed = 300f;
        public float updateRate = 2f;

        public Drone(AIManager manager, Player player, Selector selector, Astar pathFinder, RigidBody2D rb, SpriteBatch spr, Texture2D tex) : 
            base(rb.Position, rb.Rotation, rb.Scale, rb.Tag)
        {
            ObjRB = rb;
            Spr = spr;
            Texture = tex;
            Selector = selector;
            ObjRB.boxColl = new BoxCollider(Position, new Vector2(Position.X + Texture.Width), Texture.Width, Texture.Height);
            ObjRB.LinearDrag = 0.05f;
            ObjRB.polygonColl = new PolygonCollider();
            SetPolygonPoints(ObjRB.polygonColl);
            target = player.ObjRB.Position;
            Manager = manager;
            Manager.RequestPath(Position, target, OnPathFound);
        }

        public void OnPathFound(Vector2[] newPath, bool pathSuccess)
        {
            if (pathSuccess)
            {
                path = newPath;
            }
        }

        void UpdatePath()
        {
            Manager.RequestPath(Position, target, OnPathFound);
            FollowPath();
        }

        void FollowPath()
        {
            if (target != null)
            {
                //TODO: Always look at target
            }
            if (path == null)
                return;
            if (targetIndex >= path.Length)
            {
                if (pathComplete)
                    return;
                targetIndex = 0;
                //path = new Vector2[0];
                pathComplete = true;
                return;
            }
            pathComplete = false;

            //Direction to next waypoint
            Vector2 dir = Vector2.Normalize(path[targetIndex] - Position);//may need to reverse y axis
            dir *= speed * delta;

            ObjRB.Velocity += dir;
            float dist = Vector2.Distance(Position, path[targetIndex]);
            //may not need this
            if (dist < nextWaypointDistance)
            {
                targetIndex++;
                return;
            }          
        }
        
        public override void Update(GameTime gameTime)
        {
            if (hasKey)
                Console.WriteLine("drone has key");

            delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (target != null)
            {
                UpdatePath();
            }
           
            Position = ObjRB.Position;
            SetPolygonPoints(ObjRB.polygonColl);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Spr.Draw(Texture, Position, Color.White);
        }

        public override void OnCollision(Manifold man)
        {
            /*RigidBody2D collisionObj = man.A == (RigidBody2D)this ? man.B : man.A;//check which object we are

            if (collisionObj.Tag == "key")
            {
                hasKey = true;
            }

            if (collisionObj.Tag == "player")
            {
                if (!hasKey)
                    return;
                else
                    if (!Player.hasKey)
                {
                    Player.hasKey = true;
                    hasKey = false;
                }
            }
            base.OnCollision(man);*/
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
