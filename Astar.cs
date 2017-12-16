using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace GameBehaviour
{
    public class Astar
    {

        public AIManager Manager;
        public Board board;
       
        public Astar()
        {
            
        }

        public void StartFindPath(Vector2 startPosition, Vector2 endPosition)
        {
            FindPath(startPosition, endPosition);
        }

        public void FindPath(Vector2 startPos, Vector2 targetPos)
        {
            Node startNode = board.NodeFromWorldPoint(startPos);
            Node targetNode = board.NodeFromWorldPoint(targetPos);
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            Vector2[] waypoints = new Vector2[0];
            bool pathSuccess = false;
            openSet.Add(startNode);

            if (startNode.isTraversible && targetNode.isTraversible)
            {
                while (openSet.Count > 0)
                {
                    Node currentNode = openSet[0];
                    for (int i = 1; i < openSet.Count; i++)
                    {
                        if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                        {
                            currentNode = openSet[i];
                        }
                    }

                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbor in board.GetNeighbors(currentNode))
                    {
                        if (!neighbor.isTraversible || closedSet.Contains(neighbor))
                            continue;

                        int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                        if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = newMovementCostToNeighbor;
                            neighbor.hCost = GetDistance(neighbor, targetNode);
                            neighbor.Parent = currentNode;

                            if (!openSet.Contains(neighbor))
                                openSet.Add(neighbor);
                        }
                    }
                }
            }
            if(pathSuccess)
                waypoints = RetracePath(startNode, targetNode);

            Manager.FinishedProcessingPath(waypoints, pathSuccess);
        }

        Vector2[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            List<Vector2> pathCoOrds = new List<Vector2>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            for (int i = 0; i < path.Count; i++)
            {
                pathCoOrds.Add(path[i].worldPosition);
                
            }
            Vector2[] waypoints = pathCoOrds.ToArray();     
            Array.Reverse(waypoints);
            return waypoints;
            
        }

        public int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Math.Abs(nodeA.gridX - nodeB.gridX);
            int distY = Math.Abs(nodeA.gridY - nodeB.gridY);// +- may need reversing

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distX - distY);

        }
    }
}
