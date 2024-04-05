using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Senses
{
    public static class EntityVision
    {
        public static T CircularCheck<T>(Vector2 origin, float distance, ref Collider2D[] results)
            where T : Object, IEntity
        {
            results ??= new Collider2D[100];
            int size = Physics2D.OverlapCircleNonAlloc(origin, distance, results);

            for (int i = 0; i < size; i++)
            {
                if (!results[i].TryGetComponent(out T t)) continue;
                return t;
            }

            return null;
        }

        public static bool IsVisible<T>(Vector3 from, Vector3 to, bool facingLeft, LayerMask sourceLayerMask,
            bool radialVision)
            where T : IEntity
        {
            float direction = to.x - from.x;
            if (!radialVision && ((direction < 0f && !facingLeft) || (direction > 0f && facingLeft))) return false;

            var result = Physics2D.Linecast(from, to, ~sourceLayerMask);
            return result && result.transform.TryGetComponent(out T t);
        }

        public static WallResult CheckWallCollision(Collider2D collider, WallDetection wallDetection, bool facingLeft)
        {
            Physics2D.queriesStartInColliders = false;

            float horizontal = facingLeft ? collider.bounds.min.x : collider.bounds.max.x;
            horizontal += facingLeft ? -wallDetection.HorizontalOffset : wallDetection.HorizontalOffset;
            Vector2 direction = facingLeft ? Vector2.left : Vector2.right;

            // Top Check.
            Vector2 origin = new Vector2(horizontal, collider.bounds.max.y + wallDetection.TopOffset);
            bool topCheck = WallRayCast(origin, direction, wallDetection.DistanceCheck, wallDetection.WallLayer, out _);

            // Center Check.
            origin.y = collider.bounds.center.y + wallDetection.MidOffset;
            bool midCheck = WallRayCast(origin, direction, wallDetection.DistanceCheck, wallDetection.WallLayer, out _);

            // Bottom Check.
            origin.y = collider.bounds.min.y + wallDetection.BottomOffset;
            bool bottomCheck = WallRayCast(origin, direction, wallDetection.DistanceCheck, wallDetection.WallLayer,
                out _);

            return new WallResult(topCheck, midCheck, bottomCheck);
        }

        public static Vector2 CheckLedge(Collider2D collider, WallDetection wallDetection, bool facingLeft)
        {
            Physics2D.queriesStartInColliders = false;

            float horizontal = facingLeft ? collider.bounds.min.x : collider.bounds.max.x;
            float horizontalOffset =
                facingLeft ? -wallDetection.HorizontalOffset : wallDetection.HorizontalOffset;
            horizontal += horizontalOffset;

            Vector2 direction = facingLeft ? Vector2.left : Vector2.right;

            // Top Check.
            Vector2 origin = new Vector2(horizontal, collider.bounds.max.y + wallDetection.TopOffset);
            bool topCheck = WallRayCast(origin, direction, wallDetection.DistanceCheck, wallDetection.WallLayer, out _);

            // Center Check.
            origin.y = collider.bounds.center.y + wallDetection.MidOffset;
            bool midCheck = WallRayCast(origin, direction, wallDetection.DistanceCheck, wallDetection.WallLayer,
                out RaycastHit2D hit);

            if (!topCheck && midCheck)
            {
                float sphereOffset =
                    facingLeft ? -wallDetection.LedgeDetectorOffset.x : wallDetection.LedgeDetectorOffset.x;
                Vector2 spherePosition = new Vector2(sphereOffset + horizontal,
                    collider.bounds.max.y + wallDetection.LedgeDetectorOffset.y);
                Collider2D result = Physics2D.OverlapCircle(spherePosition, wallDetection.LedgeDetectorRadius,
                    hit.transform.gameObject.layer);

                if (result == null) return Vector2.zero;

                var results = new ContactPoint2D[50];
                var filer = new ContactFilter2D
                {
                    layerMask = hit.transform.gameObject.layer
                };

                int amount = result.GetContacts(filer, results);
                if (amount == 0) return Vector2.zero;
                Vector2 corner = results[0].point;

                for (int i = 0; i < amount; i++)
                {
                    Vector2 point = results[i].point;
                    if (facingLeft && point.x > corner.x) corner.x = point.x;
                    else if (!facingLeft && point.x < corner.x) corner.x = point.x;

                    if (corner.y < point.y) corner.y = point.y;
                }

                return corner;
            }

            return Vector2.zero;
        }

        public static bool WallRayCast(Vector2 origin, Vector2 direction, float distanceCheck, LayerMask wallLayer,
            out RaycastHit2D result)
        {
            result = Physics2D.Raycast(origin, direction, distanceCheck, wallLayer);
            return result;
        }
    }
}