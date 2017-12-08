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
    public class Node
    {
        public bool isTraversible;
        public int gCost; //the distance from the starting node
        public int hCost; //the distance from the end node (heuristic)

        public int gridX;
        public int gridY;

        public Node Parent;

        public Texture2D SelectorTexture;

        public Vector2 worldPosition;

        public int fCost
        {
            get {
                return gCost + hCost;
            }
        }
    }

    
}
