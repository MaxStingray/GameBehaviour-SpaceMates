using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameBehaviour
{
    public struct PolygonCollisionResult
    {
        public bool WillIntersect;
        public bool Intersect;
        public Vector2 MinTranslation;
        public float penDepth;
    }
    //class for managing physics objects
    public class World
    {
        public List<RigidBody2D> PhysObjects { get; set; }

        public World()
        {
            PhysObjects = new List<RigidBody2D>();
        }

        public void Step(GameTime time)
        {
            List<Manifold> manifolds = new List<Manifold>();
            List<Manifold> potentialCollisions = new List<Manifold>();

            foreach (RigidBody2D rb in PhysObjects)//update each rigidbody accordingly
            {
                rb.Update(time);
            }
            //broad phase baby
            for (int i = 0; i < PhysObjects.Count; i++)
            {
                for (int j = i + 1; j < PhysObjects.Count; j++)
                {
                    if (PhysObjects[i].Tag != PhysObjects[j].Tag)
                    {
                        Manifold manifold = new Manifold();
                        if (AABBvsAABB(PhysObjects[i], PhysObjects[j], manifold))
                        {
                            potentialCollisions.Add(manifold);
                        }
                    }      
                }
            }

            for (int i = 0; i < potentialCollisions.Count; i++)//loop through potential collisions
            {               
                SATResolve(potentialCollisions[i], time); //resolve using SAT              
            }
            potentialCollisions.Clear();
        }
        //method for testing collision pairs
        private static bool AABBvsAABB(RigidBody2D a, RigidBody2D b, Manifold manifold)
        {
          
            if(a.Position.X < b.Position.X + b.boxColl.width
            && a.Position.X + a.boxColl.width > b.Position.X
            && a.Position.Y < b.Position.Y + b.boxColl.height
            && a.boxColl.height + a.Position.Y > b.Position.Y)
                {
                    manifold.A = a;
                    manifold.B = b;
                    return true;
                }
                return false;      
            
        }

        public void ProjectPolygon(Vector2 axis, PolygonCollider polygon, ref float min, ref float max)
        {
            float dotProduct = Vector2.Dot(axis, polygon.points[0]);
            min = dotProduct;
            max = dotProduct;
            for (int i = 0; i < polygon.points.Count; i++)
            {
                dotProduct = Vector2.Dot(polygon.points[i], axis);
                if (dotProduct < min)
                {
                    min = dotProduct;
                }
                else
                {
                    if (dotProduct > max)
                    {
                        max = dotProduct;
                    }
                }
            }
        }

        // Calculate the distance between [minA, maxA] and [minB, maxB]
        // The distance will be negative if the intervals overlap
        public float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        // Check if polygon A is going to collide with polygon B.
        // The last parameter is the *relative* velocity 
        // of the polygons (i.e. velocityA - velocityB)
        public PolygonCollisionResult PolygonCollision(PolygonCollider polygonA,
                                      PolygonCollider polygonB, Vector2 velocity, GameTime gameTime)
        {
            PolygonCollisionResult result = new PolygonCollisionResult();
            result.Intersect = true;
            result.WillIntersect = true;

            int edgeCountA = polygonA.edges.Count;
            int edgeCountB = polygonB.edges.Count;
            float minIntervalDistance = float.PositiveInfinity;
            Vector2 translationAxis = new Vector2();
            Vector2 edge;

            // Loop through all the edges of both polygons
            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = polygonA.edges[edgeIndex];
                }
                else
                {
                    edge = polygonB.edges[edgeIndex - edgeCountA];
                }

                // ===== 1. Find if the polygons are currently intersecting =====

                // Find the axis perpendicular to the current edge
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting
                if (IntervalDistance(minA, maxA, minB, maxB) > 0)
                result.Intersect = false;

                // ===== 2. Now find if the polygons *will* intersect =====

                // Project the velocity on the current axis
                float velocityProjection = Vector2.Dot(axis, velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

                // Get the projection of polygon A during the movement
                if (velocityProjection < 0)
                {
                    minA += velocityProjection;
                }
                else
                {
                    maxA += velocityProjection;
                }

                // Do the same test as above for the new projection
                float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance > 0) result.WillIntersect = false;

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.Intersect && !result.WillIntersect) break;

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation vector
                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {
                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;

                    Vector2 d = polygonA.centre() - polygonB.centre();
                    
                    if (Vector2.Dot(d, translationAxis) < 0)
                        translationAxis = -translationAxis;
                }
            }

            // The minimum translation vector
            // can be used to push the polygons appart.
            if (result.WillIntersect || result.Intersect)
            {
                result.MinTranslation = 
                       translationAxis * minIntervalDistance;
                //Console.WriteLine(result.MinTranslation);
                result.penDepth = minIntervalDistance;
            }


            return result;
        }


        void SATResolve(Manifold manifold, GameTime gameTime)
        {
            Vector2 relativeVelocity = manifold.A.Velocity - manifold.B.Velocity;
            Vector2 relativeVelocityHack = manifold.B.Velocity - manifold.A.Velocity;
            
            PolygonCollisionResult result = PolygonCollision(manifold.A.polygonColl, manifold.B.polygonColl, relativeVelocity, gameTime);
            Vector2 normal = Vector2.Normalize(result.MinTranslation);
            if (!float.IsNaN(normal.X) || !float.IsNaN(normal.Y))
            {
                if (manifold.A.parent != null && manifold.B.parent != null)
                {
                    manifold.A.parent.OnCollision(manifold);
                    manifold.B.parent.OnCollision(manifold);
                }
                
                float contactVelocity = Vector2.Dot(relativeVelocityHack, normal);

                if (contactVelocity < 0)
                {
                    return;
                }

                //apply friction
                float friction = ((manifold.A.Friction + manifold.B.Friction) / 2);

                //will eventually be restitution, leave as 1 for now
                float e = Math.Min(1, 1);

                float j = -(1.0f + e) * contactVelocity;
                j /= (1 / manifold.A.Mass) + (1 / manifold.B.Mass);

                Vector2 impulse = j * normal;

                //apply impulse
                if (!manifold.A.IsStatic)
                {
                    manifold.A.Velocity -= (1 / manifold.A.Mass) * (impulse);
                    if (manifold.A.Tag != "drone")
                    {
                        if (manifold.A.Velocity.X > 0)
                            manifold.A.Velocity.X -= (friction);
                        else if (manifold.A.Velocity.X < 0)
                            manifold.A.Velocity.X += friction;
                    }
                }
                if (!manifold.B.IsStatic)
                {
                    manifold.B.Velocity += (1 / manifold.B.Mass) * impulse;
                    if (manifold.B.Tag != "drone")
                    {
                        if (manifold.B.Velocity.X > 0)
                            manifold.B.Velocity.X -= (friction);
                        else if (manifold.B.Velocity.X < 0)
                            manifold.B.Velocity.X += friction;
                    }
                }
            }
        }

    }
}
