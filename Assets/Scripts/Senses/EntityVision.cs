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
            bool topCheck = WallRayCast(origin, direction, wallDetection, out _);

            // Center Check.
            origin.y = collider.bounds.center.y;
            bool midCheck = WallRayCast(origin, direction, wallDetection, out _);

            // Bottom Check.
            origin.y = collider.bounds.min.y + wallCheckBottomOffset;
            bool bottomCheck = WallRayCast(origin, direction, wallDetection, out _);

            return new WallResult(topCheck, midCheck, bottomCheck);
        }

        private static bool WallRayCast(Vector2 origin, Vector2 direction, WallDetection wallDetection,
            out RaycastHit2D result)
        {
            result = Physics2D.Raycast(origin, direction, wallDetection.DistanceCheck, wallDetection.WallLayer);
            return result;
        }

        public static Vector2 CheckLedge(CapsuleCollider2D collider, WallDetection wallDetection, bool facingLeft)
        {
            Physics2D.queriesStartInColliders = false;

            float wallCheckTopOffset = wallDetection.TopOffset;
            float horizontal = facingLeft ? collider.bounds.min.x : collider.bounds.max.x;
            Vector2 direction = facingLeft ? Vector2.left : Vector2.right;

            // Top Check.
            Vector2 origin = new Vector2(horizontal, collider.bounds.max.y - wallCheckTopOffset);
            bool topCheck = WallRayCast(origin, direction, wallDetection, out _);

            // Center Check.
            origin.y = collider.bounds.center.y;
            bool midCheck = WallRayCast(origin, direction, wallDetection, out RaycastHit2D hit);

            if (!topCheck && midCheck)
            {
                Vector2 spherePosition =
                    new Vector2(collider.bounds.center.x, collider.bounds.max.y - wallDetection.TopOffset) +
                    wallDetection.LedgeDetectorOffset;
                var result = Physics2D.OverlapCircle(spherePosition, wallDetection.LedgeDetectorRadius,
                    hit.transform.gameObject.layer);

                var results = new ContactPoint2D[50];
                var filer = new ContactFilter2D();
                filer.layerMask = hit.transform.gameObject.layer;
                
                var amount = result.GetContacts(filer, results);
                var corner = results[0].point;

                /*for (int i = 0; i < amount; i++)
                {
                    var point = results[i].point;
                    if (facingLeft)
                    {
                        if (point.x > corner.x && point.y > corner.y)
                            corner = point;
                    }
                    else
                    {
                        if (point.x < corner.x && point.y > corner.y)
                            corner = point;
                    }
                }*/

                for (int i = 0; i < amount; i++)
                {
                    var point = results[i].point;
                    if (facingLeft && point.x > corner.x) corner.x = point.x;
                    else if (!facingLeft && point.x < corner.x) corner.x = point.x;

                    if (corner.y < point.y) corner.y = point.y;
                }

                return corner;
            }

            return Vector2.zero;
        }
    }
}