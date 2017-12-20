using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        Texture2D BounceTexture;
        SpriteBatch SpriteBatch;

        public Board(SpriteBatch spriteBatch, Texture2D blockTexture, Texture2D bounceTexture, int numCols, int numRows)
        {
            tiles = new Tile[numCols, numRows];
            SpriteBatch = spriteBatch;
            BlockTexture = blockTexture;
            BounceTexture = bounceTexture;
            cols = numCols;
            rows = numRows;

            for (int x = 0; x < numCols; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    Vector2 blockPosition = new Vector2(x * blockTexture.Width, y * blockTexture.Height);

                    tiles[x, y] = new Tile(new RigidBody2D(blockPosition, new Vector2(0, 0), 1, "obstacle" , true, 10, 50), false,
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
                    if (x >9 && x < 18 && y >= 6 && y < 10 || x > 9 && x <= 18 && y == 4
                        || x == 18 && y >= 6 && y < 10)
                    {
                        tiles[x, y].IsRendered = true;
                        tiles[x, y].aStarNode.isTraversible = false;
                    }

                    if (x >= 18 && x < 47 && y == 9 || x == 18 && y >= 6 && y <= 10 || x == 18 && y == 4)
                    {
                        tiles[x, y].Texture = BounceTexture;
                        tiles[x, y].IsRendered = true;
                        tiles[x, y].aStarNode.isTraversible = false;
                    }

                    if (x == 40 && y > 0 && y <= 5)
                    {
                        tiles[x, y].IsRendered = true;
                        tiles[x, y].aStarNode.isTraversible = false;
                    }

                    if (x > 41 && x < 47 && y == 8 || x > 49 && x < cols - 1 && y > 2 && y <= 9)
                    {
                        tiles[x, y].IsRendered = true;
                        tiles[x, y].aStarNode.isTraversible = false;
                    }

                    if (x >= 6 && x <= 8 && y == 7)
                    {
                        tiles[x, y].IsRendered = true;
                        tiles[x, y].aStarNode.isTraversible = false;
                    }
                }
            }
            

            generationFinished = true;
        }

        void CreatePlatform()
        {
            Tile startPoint;//first tile in platform (from the left)
            Tile endPoint;
            int y = 0;
            
            for (int x = 0; x < cols;)//loop through each tile in the row indicated by y
            {
                if (!tiles[x, y].IsRendered)//if we find an unrendered tile
                    x++;//skip it
                else//otherwise
                {
                    startPoint = tiles[x, y];//set this as the start point
                    for (int c = x; c < cols;)//start a new counter
                    {
                        if (!tiles[c, y].IsRendered)//when we hit the next unrendered tile
                        {
                            endPoint = tiles[c - 1, y];//set the previous tile as the end point
                            x = c;//set the previous counter to this one
                            //c = 0;//reset c
                            Platform platform = new Platform((c), new RigidBody2D(startPoint.Position, startPoint.Rotation,
                                                startPoint.Scale, startPoint.Tag, true, 4, 4), startPoint, endPoint, false);
                            if (platform.StartPoint == tiles[10, 9])
                            {
                                platform.SetBouncy();
                            }
                            platforms.Add(platform);//add the platform to the collection
                            break;//exit the loop (CONTINUE?)
                        }
                        else if (c + 1 == cols)
                        {
                            endPoint = tiles[c, y];
                            Platform platform = new Platform((c), new RigidBody2D(startPoint.Position, startPoint.Rotation,
                            startPoint.Scale, startPoint.Tag, true, 4, 4), startPoint, endPoint, false);
                            if (platform.StartPoint == tiles[10, 9])
                            {
                                platform.SetBouncy();
                            }

                            platforms.Add(platform);
                            x = c;
                            //c = 0;
                            break;//exit the loop
                        }
                        else
                            c++;//increment and check the next tile
                    }
                }
                if (y + 1 != rows && x + 1 == cols)//if we reach the end of a row
                {
                    x = 0;
                    y++;//reset x and go to the next line
                }
                else
                    x++;//otherwise, check the next in the row

            }

                foreach (Platform p in platforms)
                {
                    for (int i = 0; i < cols; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            if (i == 37 && j <= 9 || i == 18 && j >= 6 && j <= 9
                                || i >=0 && j == 0)
                            {                             
                                if (p.StartPoint == tiles[i, j] || p.EndPoint == tiles[i,j])
                                {
                                    tiles[i, j].IsRendered = true;
                                    tiles[i, j].Texture = BounceTexture;
                                    p.SetBouncy();
                                }
                            }
                        }
                    }
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
            float percentX = (worldPos.X + boardSize.X / 54) / boardSize.X;
            float percentY = (worldPos.Y - boardSize.Y / 16) / boardSize.Y;
            percentX = HandyMath.Clamp01(percentX);
            percentY = HandyMath.Clamp01(percentY);

            int x = (int)Math.Round((boardSizeX + 1) * percentX, MidpointRounding.AwayFromZero);
            int y = (int)Math.Round((boardSizeY) * percentY, MidpointRounding.AwayFromZero);

            return tiles[x - 2, y].aStarNode;
        }

        public Tile TileFromWorldPoint(Vector2 worldPos)
        {
            float percentX = (worldPos.X + boardSize.X / 54) / boardSize.X;
            float percentY = (worldPos.Y - boardSize.Y / 16) / boardSize.Y;
            percentX = HandyMath.Clamp01(percentX);
            percentY = HandyMath.Clamp01(percentY);

            int x = (int)Math.Round((boardSizeX + 1) * percentX, MidpointRounding.AwayFromZero);
            int y = (int)Math.Round((boardSizeY) * percentY, MidpointRounding.AwayFromZero);

            return tiles[x - 2, y];
        }
    }
}
