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
    public class Board
    {
        public Tile[,] tiles { get; set; }
        public Tile[,] nodes;
        public List<Platform> platforms = new List<Platform>();
        int cols { get; set; }
        int rows { get; set; }

        public bool generationFinished;
        int boardSizeX;
        int boardSizeY;
        Vector2 boardSize;
        Texture2D BlockTexture;
        SpriteBatch SpriteBatch;

        public Board(SpriteBatch spriteBatch, Texture2D blockTexture, int numCols, int numRows)
        {
            tiles = new Tile[numCols, numRows];
            SpriteBatch = spriteBatch;
            BlockTexture = blockTexture;
            cols = numCols;
            rows = numRows;

            for (int x = 0; x < numCols; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    Vector2 blockPosition = new Vector2(x * blockTexture.Width, y * blockTexture.Height);

                    tiles[x, y] = new Tile(new RigidBody2D(blockPosition, new Vector2(0, 0), 1, "obstacle" , true, 10), false, blockPosition, new Vector2(0, 0), 1, "obstacle", true,
                        blockTexture, spriteBatch, 10);
                    tiles[x, y].aStarNode.gridX = x;
                    tiles[x, y].aStarNode.gridY = y;
                    //tiles[x, y].ObjRB.Mass = 4;
                    
                }
            }
            CreateBorders();
            CreatePlatform();
            boardSize = new Vector2(tiles[0, 0].Texture.Width * numCols, tiles[0, 0].Texture.Height * numRows);//get the board's size
            Console.WriteLine(boardSize);
            //boardSizeX = (int)Math.Round((boardSize.X / nodeDiameter));
            boardSizeX = numCols;
            Console.WriteLine(boardSizeX);
            //boardSizeY = (int)Math.Round((boardSize.Y / nodeDiameter));
            boardSizeY = numRows;
            Console.WriteLine(boardSizeY);
            //our tiles array will also serve as our node graph for A*
            //it is fully set up by this stage
        }

        public void Draw(SpriteBatch spr)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spr);
            }
        }

        void CreateBorders()
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (x == 0 || x == (cols - 1) || y == 0 || y == (rows - 1))//cols and rows -1!!!
                    {
                        tiles[x, y].IsRendered = true;
                        tiles[x, y].aStarNode.isTraversible = false;
                    }
                    
                }
            }

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (x >6 && x < 10 && y >= 6 && y < 10 || x > 8 && x <= 18 && y == 4
                        || x == 18 && y >= 0 && y < 10)//cols and rows -1!!!
                    {
                        tiles[x, y].IsRendered = true;
                        tiles[x, y].aStarNode.isTraversible = false;
                    }

                }
            }

            generationFinished = true;
        }

        void RandomiseBoard()
        {
            Random rnd = new Random();
            //int randX = rnd.Next(0, cols);
            //int randY = rnd.Next(0, rows);
            for (int x = 0; x < cols; x++)
            {
                int randX = rnd.Next(0, cols);
                for (int y = 0; y < rows; y++)
                {
                    int randY = rnd.Next(0, rows);

                    if (x == randX && x == randY)
                        tiles[x, y].IsRendered = true;
                }
            }
        }

        void CreatePlatform()
        {
            Tile startPoint;//first tile in platform (from the left)
            Tile endPoint;
            int y = 0;

            for (int x = 0; x < cols;)
            {  
                    if (!tiles[x, y].IsRendered)//if we find an unrendered tile
                    {
                        x++;//skip it
                    }
                    else
                    {
                        startPoint = tiles[x, y];//stop incrementing x and set start point
                        for (int c = x; c < cols;)//start new counter 
                        {
                            if (!tiles[c, y].IsRendered)//if we hit an unrendered tile
                            {
                                endPoint = tiles[c - 1, y];//set the previous as the end point
                                x = c - 1;//set the original counter to this one
                                Platform platform = new Platform((c), new RigidBody2D(startPoint.Position, startPoint.Rotation,
                                startPoint.Scale, startPoint.Tag, true, 4), startPoint, endPoint, startPoint.Position, startPoint.Rotation, startPoint.Scale, startPoint.Tag, true, 4);
                                platform.ObjRB.Mass = 4;
                                platforms.Add(platform);
                                break;//exit the loop
                                }
                                else
                                {
                                    if (c + 1 == cols)
                                    {
                                        endPoint = tiles[c, y];
                                        Platform platform = new Platform((c), new RigidBody2D(startPoint.Position, startPoint.Rotation,
                                        startPoint.Scale, startPoint.Tag, true, 4), startPoint, endPoint, startPoint.Position, startPoint.Rotation, startPoint.Scale, startPoint.Tag, true, 4);
                                        platform.ObjRB.Mass = 4;
                                        platforms.Add(platform);
                                        x = c;
                                        break;//exit the loop
                                    }
                                    else
                                    {
                                        c++;//check the next tile
                                    }
                                    
                                }
                        }
                    }
                if (y + 1 != rows && x + 1 == cols)
                {
                    y++;//go to the next row
                    x = 0;
                }
                else
                    x++;                           
            }
            
        }

        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && x == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < boardSizeX && checkY >= 0 && checkY < boardSizeY)
                    {
                        neighbors.Add(tiles[checkX, checkY].aStarNode);
                    }
                }
            }

            return neighbors;
        }
        //returns a node from a point on the grid
        public Node NodeFromWorldPoint(Vector2 worldPos)
        {
            float percentX = (worldPos.X + boardSize.X / 16) / boardSize.X;
            float percentY = (worldPos.Y - boardSize.Y / 16) / boardSize.Y;
            percentX = HandyMath.Clamp01(percentX);
            percentY = HandyMath.Clamp01(percentY);

            int x = (int)Math.Round((boardSizeX - 1) * percentX, MidpointRounding.AwayFromZero);
            int y = (int)Math.Round((boardSizeY - 1) * percentY, MidpointRounding.AwayFromZero);

            return tiles[x - 1, y + 1].aStarNode;
        }

        public Tile TileFromWorldPoint(Vector2 worldPos)
        {
            float percentX = (worldPos.X + boardSize.X / 16) / boardSize.X;
            float percentY = (worldPos.Y - boardSize.Y / 16) / boardSize.Y;
            percentX = HandyMath.Clamp01(percentX);
            percentY = HandyMath.Clamp01(percentY);

            int x = (int)Math.Round((boardSizeX + 1) * percentX, MidpointRounding.AwayFromZero);
            int y = (int)Math.Round((boardSizeY - 1) * percentY, MidpointRounding.AwayFromZero);

            return tiles[x - 1, y + 1];
        }
    }
}
