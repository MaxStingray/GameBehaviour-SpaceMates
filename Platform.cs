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
    public class Platform : RigidBody2D
    {
        public int NumTiles { get; set; }
        public Tile[] TileArray { get; set; }
        public Tile StartPoint { get; set; }
        public Tile EndPoint { get; set; }
        public RigidBody2D ObjRB { get; set; }

        public float tileWidth;
        public float tileHeight;

        public Platform(int numTiles, RigidBody2D rb, Tile startPoint, Tile endPoint, Vector2 position, Vector2 rotation,
            float scale, string tag, bool isStatic) : base(position, rotation, scale, tag, isStatic)
        {
            NumTiles = numTiles;
            StartPoint = startPoint;
            EndPoint = endPoint;
            tileWidth = startPoint.Texture.Width;
            tileHeight = startPoint.Texture.Height;
            ObjRB = rb;
            ObjRB.boxColl = new BoxCollider(new Vector2(StartPoint.Position.X, StartPoint.Position.Y), new Vector2(EndPoint.Position.X + tileWidth, EndPoint.Position.Y + tileHeight)
                ,tileWidth * numTiles, tileHeight);
            ObjRB.polygonColl = new PolygonCollider();
            Console.WriteLine("start point: " + startPoint.Position);
            Console.WriteLine("end point: " + endPoint.Position);
            Console.WriteLine("number of tiles: " + numTiles);
            SetPolygonPoints(ObjRB.polygonColl);
        }

        void SetPolygonPoints(PolygonCollider p)
        {
            p.points.Clear();
            //top left
            p.points.Add(new Vector2(StartPoint.Position.X, StartPoint.Position.Y));
            //top right
            p.points.Add(new Vector2(EndPoint.Position.X + tileWidth, EndPoint.Position.Y));
            //bottom right
            p.points.Add(new Vector2(EndPoint.Position.X + tileWidth, EndPoint.Position.Y + tileHeight));
            //bottom left
            p.points.Add(new Vector2(StartPoint.Position.X, StartPoint.Position.Y + tileHeight));

            p.BuildEdges();
        }

        public override void Update(GameTime gameTime)
        {
            ObjRB.boxColl.topLeft = new Vector2(StartPoint.Position.X, StartPoint.Position.Y);
            ObjRB.boxColl.bottomRight = new Vector2(EndPoint.Position.X + tileWidth, EndPoint.Position.Y + tileHeight);
            SetPolygonPoints(ObjRB.polygonColl);
        }

    }
}
