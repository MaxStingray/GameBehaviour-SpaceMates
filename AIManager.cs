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
    public class AIManager
    {
        Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        static AIManager instance;
        Astar Pathfinding;

        bool isProcessingPath;


        public AIManager(Astar pathFinding)
        {
            instance = this;
            Pathfinding = pathFinding;
        }

        public void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callBack)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
            instance.PathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        }
        //see if we're currently processing a path, if we're not, process the next one
        void TryProcessNext()
        {
            if (!isProcessingPath && PathRequestQueue.Count > 0)
            {
                currentPathRequest = PathRequestQueue.Dequeue();
                isProcessingPath = true;
                Pathfinding.FindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector2[] path, bool success)
        {
            currentPathRequest.callBack(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector2 pathStart;
            public Vector2 pathEnd;
            public Action<Vector2[], bool> callBack;

            public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callBack)
            {
                pathStart = _start;
                pathEnd = _end;
                callBack = _callBack;
            }
        }
    }
}
