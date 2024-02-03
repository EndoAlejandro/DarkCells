using Calcatz.MeshPathfinding;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class EntityPathfinding : Pathfinding
    {
        public Vector3 Direction => NextNode != null
            ? NextNode.transform.position - transform.position
            : Target.position - transform.position;

        public Node NextNode => HasPath ? pathResult[0] : null;
        private bool HasPath => pathResult is { Length: > 0 };

        public void StartFindPath(Transform target)
        {
            if (HasPath) return;
            StartFindPath(target, 1f, true);
        }
    }
}