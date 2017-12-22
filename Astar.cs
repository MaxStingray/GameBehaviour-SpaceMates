using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace GameBehaviour
{
    public class Astar
    {
        Vector2[] waypoints = new Vector2[0];
        public Board board;
       
        public Astar()
        {
           
        }

        public Vector2[] getPath(Vector2 start, Vector2 end)
        {
            if (start != null && end != null && board != null)
            {
                FindPath(start, end);
                return waypoints;
            }
            return null;
        }

        public void FindPath(Vector2 startPos, Vector2 targetPos)
        {
            Node startNode = board.NodeFromWorldPoint(startPos);
            Node targetNode = board.NodeFromWorldPoint(targetPos);
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            //Vector2[] waypoints = new Vector2[0];
            bool pathSuccess = false;
            openSet.Add(startNode);
            Node currentNode;
            if (startNode.isTraversible && targetNode.isTraversible)
            {
                while (openSet.Count > 0)
                {
                    currentNode = openSet[0];
                    //find the node with the lowest f cost
                    float lowestFCost = openSet.Min(node => node.fCost);
                    foreach (Node n in openSet)
                    {
                        if (n.fCost <= lowestFCost)
                            currentNode = n;
                    }
                    //remove current node from the open list
                    openSet.Remove(currentNode);
                    //add it to the closed list
                    closedSet.Add(currentNode);

                    //if the current node is the target node, we have found a path
                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    List<Node> neighbors = board.GetNeighbors(currentNode);
                    foreach (Node neighbor in neighbors)
                    {
                        if (!neighbor.isTraversible || closedSet.Contains(neighbor))
                            continue;
                        //calculate the new G cost to neighbor
                        int movementCost = CalculateGcost(currentNode, neighbor);

                        if (movementCost < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            //find heuristics and set parents
                            neighbor.gCost = movementCost;
                            neighbor.hCost = CalculateHcost(neighbor, targetNode);
                            neighbor.Parent = currentNode;

                            if (!openSet.Contains(neighbor))
                                openSet.Add(neighbor);
                        }
                    }
                }
            }
            if (pathSuccess)
            {
                //retrace the path
                List<Node> path = new List<Node>();
                List<Vector2> pathCoOrds = new List<Vector2>();

                while (targetNode != startNode && targetNode != null)//while the path has not been retraced
                {
                    path.Add(targetNode);//add target node to the new path
                    targetNode = targetNode.Parent;//set its parent as the new target
                }

                for (int i = 0; i < path.Count; i++)
                {
                    pathCoOrds.Add(path[i].worldPosition);

                }
                waypoints = pathCoOrds.ToArray();
                waypoints.Reverse();
            }
        }

        public int CalculateHcost(Node A, Node B)
        {
            Vector2 dist = new Vector2(Math.Abs(A.gridX - B.gridX), Math.Abs(A.gridY - B.gridY));

            if (dist.X > dist.Y)
                return (int)Math.Round(14 * dist.Y + 10 * (dist.X - dist.Y));
            return (int)Math.Round(14 * dist.X + 10 * (dist.X - dist.Y));
        }

        public int CalculateGcost(Node A, Node B)
        {
            int Hcost = CalculateHcost(A, B);
            return Hcost + A.gCost;            
        }
    }
}
