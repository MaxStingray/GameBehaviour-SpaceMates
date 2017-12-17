using Microsoft.Xna.Framework;
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
        RigidBody2D ObjRB;
        //public RigidBody2D ObjRB { get; set; }

        public Tile(RigidBody2D rb, bool isRendered, Texture2D texture, SpriteBatch spriteBatch, float friction) 
            : base(rb.Position, rb.Rotation, rb.Scale, rb.Tag, rb.IsStatic, rb.Friction, rb.Mass)
        {
            IsRendered = isRendered;
            Texture = texture;
            SpriteBatch = spriteBatch;
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
            //maybe set back to the top left for consistency
            aStarNode.worldPosition = new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2));
            //ObjRB = rb;
            rb.spr = spriteBatch;
            center = new Vector2(Position.X + (Texture.Width / 2), Position.X + (texture.Height / 2));
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
            
        }
    }
}
