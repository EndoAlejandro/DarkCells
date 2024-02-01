using Calcatz.MeshPathfinding;

namespace DarkHavoc.Enemies
{
    public class EntityPathfinding : PathfindingUserBase
    {
        private void Start()
        {
            //pathfinding.SetTarget(target);
            pathfinding.StartFindPath(1, true);
        }

        private void FixedUpdate()
        {
            Node[] path = pathfinding.GetPathResult();
            if (path != null)
            {
            }
        }
    }
}