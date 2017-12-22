using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameBehaviour
{
    public class Drone : GameObject
    {
        public RigidBody2D ObjRB;
        SpriteBatch Spr;
        Texture2D Texture;
        public Selector Selector;
        public Player Player;
        public Astar pFinder;
        float delta;

        bool pathComplete;
        public Vector2 target;
        public Vector2[] path;
        int targetIndex;

        public bool hasKey = false;

        public float nextWaypointDistance = 3;

        public float speed = 300f;
        public float updateRate = 2f;

        public Drone(Player player, Selector selector, Astar pathFinder, RigidBody2D rb, SpriteBatch spr, Texture2D tex) : 
            base(rb.Position, rb.Tag)
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
            pFinder = pathFinder;
            path = pFinder.getPath(Position, target);
            UpdatePath();
        }

        void UpdatePath()
        {
            path = pFinder.getPath(Position, target);
            FollowPath();
        }

        void FollowPath()
        {
            if (path == null)
                return;
            if (targetIndex >= path.Length)
            {
                if (pathComplete)
                    return;
                targetIndex = 0;
                pathComplete = true;
                return;
            }
            pathComplete = false;

            //Direction to next waypoint
            Vector2 dir = Vector2.Normalize(path[targetIndex] - Position);
            dir *= speed * delta;

            ObjRB.Velocity += dir;
            float dist = Vector2.Distance(Position, path[targetIndex]);

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
