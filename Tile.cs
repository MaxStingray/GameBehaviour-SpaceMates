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
    public class Tile : RigidBody2D//tile is a game object, no rigidbody as it does not need phys (yet)
    {
        public bool IsRendered { get; set; } //whether or not this tile is active. Also tells us whether this tile is traversible

        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Vector2 center;
        //Node for A*
        public Node aStarNode;
        //public RigidBody2D ObjRB { get; set; }

        public Tile(RigidBody2D rb, bool isRendered, Vector2 position, Vector2 rotation,
            float scale, string tag, bool isStatic, Texture2D texture, SpriteBatch spriteBatch) 
            : base(position, rotation, scale, tag, isStatic)
        {
            IsRendered = isRendered;
            Texture = texture;
            SpriteBatch = spriteBatch;
            rb.Mass = 50;
            aStarNode = new Node();//give each tile a node
            if (isRendered)
            {
                aStarNode.isTraversible = false;
            }
            else
            {
                aStarNode.isTraversible = true;
            }
            //tell it whether we can traverse it (for now if it is rendered we can traverse it)
            //set the node's worldPosition to be the centre of the tile as opposed to the top left
            aStarNode.worldPosition = new Vector2(position.X + (Texture.Width / 2), position.Y + (Texture.Height / 2));
            //ObjRB = rb;
            rb.spr = spriteBatch;
            center = new Vector2(position.X + (Texture.Width / 2), position.X + (texture.Height / 2));
            //ObjRB.boxColl = new BoxCollider(new Vector2(Position.X, Position.Y),
               //new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height), texture.Width, texture.Height);
            //ObjRB.polygonColl = new PolygonCollider();
            //SetPolygonPoints(ObjRB.polygonColl);


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
        }

        public override void Draw(SpriteBatch spr)
        {
            if(IsRendered)
                SpriteBatch.Draw(Texture, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            //ObjRB.boxColl.topLeft = new Vector2(Position.X, Position.Y);
            //ObjRB.boxColl.bottomRight = new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height);
            //SetPolygonPoints(ObjRB.polygonColl);
        }
    }
}
