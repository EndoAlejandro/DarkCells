using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Senses
{
    public static class EntityVision
    {
        public static T CircularCheck<T>(Vector2 origin, float distance, ref Collider2D[] results)
            where T : Object, IEntity
        {
            int size = Physics2D.OverlapCircleNonAlloc(origin, distance, results);

            for (int i = 0; i < size; i++)
            {
                if (!results[i].TryGetComponent(out T t)) continue;
                return t;
            }

            return null;
        }

        public static bool IsVisible<T>(Vector3 from, Vector3 to, bool facingLeft, LayerMask sourceLayerMask)
            where T : IEntity
        {
            float direction = to.x - from.x;
            if ((direction < 0f && !facingLeft) || (direction > 0f && facingLeft)) return false;

            var result = Physics2D.Linecast(from, to, ~sourceLayerMask);
            return result && result.transform.TryGetComponent(out T t);
        }

        public static WallResult CheckWallCollision(Collider2D collider, WallDetection wallDetection, bool facingLeft)
        {
            Physics2D.queriesStartInColliders = false;

            // facingWall = false;
            float wallCheckTopOffset = wallDetection.TopOffset;
            float wallCheckBottomOffset = wallDetection.BottomOffset;

            float horizontal = facingLeft ? collider.bounds.min.x : collider.bounds.max.x;
            Vector2 direction = facingLeft ? Vector2.left : Vector2.right;

            // Top Check.
            Vector2 origin = new Vector2(horizontal, collider.bounds.max.y - wallCheckTopOffset);
            bool midCheck = WallRayCast(origin, direction, wallDetection);

            // Center Check.
            origin.y = collider.bounds.center.y;
            bool topCheck = WallRayCast(origin, direction, wallDetection);

            // Bottom Check.
            origin.y = collider.bounds.min.y + wallCheckBottomOffset;
            bool bottomCheck = WallRayCast(origin, direction, wallDetection);

            return new WallResult(topCheck, midCheck, bottomCheck);
        }

        private static bool WallRayCast(Vector2 origin, Vector2 direction, WallDetection wallDetection) =>
            Physics2D.Raycast(origin, direction, wallDetection.DistanceCheck, wallDetection.WallLayerMask);
    }
}