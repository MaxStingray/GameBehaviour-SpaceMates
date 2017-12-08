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
        public Node target;
        SpriteBatch Spr;
        Astar PathFinder;
        Texture2D Texture;
        public Drone(Astar pathFinder, RigidBody2D rb, SpriteBatch spr, Texture2D tex, Vector2 position, Vector2 rotation, float scale, string tag, bool isStatic) : 
            base(position, rotation, scale, tag, isStatic)
        {
            ObjRB = rb;
            Spr = spr;
            Texture = tex;
            PathFinder = pathFinder;
            ObjRB.boxColl = new BoxCollider(Position, new Vector2(Position.X + Texture.Width), Texture.Width, Texture.Height);
            ObjRB.Mass = 5;
            ObjRB.polygonColl = new PolygonCollider();
            SetPolygonPoints(ObjRB.polygonColl);
        }

        public override void Update(GameTime gameTime)
        {
            Position = ObjRB.Position;
            SetPolygonPoints(ObjRB.polygonColl);
            if(target != null)
                MoveTo(target);
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

        public void MoveTo(Node targetNode)
        {
            PathFinder.FindPath(Position, targetNode.worldPosition);//maybe adjust selector to return co-ordinate instead of node
            //TODO: everything else.
            //if()
        }
    }
}
