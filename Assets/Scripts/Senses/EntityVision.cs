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
    }
}