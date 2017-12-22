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
    /***************************************************************************************
    *   Some of the following code for building a polygon collider was adapted from code
    *   found here:
    *   
    *    Title: 2D Polygon Collision Detection
    *    Author: Laurent Cozic
    *    Date: 20 September 2006
    *    Availability: https://www.codeproject.com/Articles/15573/D-Polygon-Collision-Detection
    *
    ***************************************************************************************/
    public class PolygonCollider
    {
        public Vector2 pos = new Vector2();
        //list of points
        public List<Vector2> points = new List<Vector2>();
        //list of edges
        public List<Vector2> edges = new List<Vector2>();

        public void BuildEdges()
        {
            Vector2 p1;
            Vector2 p2;
            edges.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                p1 = points[i];
                if (i + 1 >= points.Count)
                {
                    p2 = points[0];//if we reach the end, connect back to the first point and we're done
                }
                else
                {
                    p2 = points[i + 1];//otherwise, connect to the next point in the collection
                }
                edges.Add(p2 - p1);
            }
        }

        public Vector2 centre() //find the logical centre of all points
        {
            float totalX = 0;
            float totalY = 0;
            for (int i = 0; i < points.Count; i++)
            {
                totalX += points[i].X;
                totalY += points[i].Y;
            }

            Vector2 logicalCentre = new Vector2(totalX / (float)points.Count, totalY / (float)points.Count);
            return pos + logicalCentre;
        }

        public void Offset(Vector2 v)//offset this collider by a given vector
        {
            Offset(v.X, v.Y);
        }

        public void Offset(float x, float y)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 p = points[i];
                points[i] = new Vector2(p.X + x, p.Y + y);
            }
        }
    }
}
