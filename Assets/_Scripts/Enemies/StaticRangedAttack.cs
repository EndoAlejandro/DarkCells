using DarkHavoc.CustomUtils;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class StaticRangedAttack : MonoBehaviour
    {
        private Collider2D _collider;

        private bool _isSphere;
        private Collider2D[] _results;
        private Player _player;
        private IDoDamage _doDamage;

        public void Setup(IDoDamage doDamage)
        {
            _collider ??= GetComponent<Collider2D>();
            
            _doDamage = doDamage;
            _results = new Collider2D[50];
        }

        private void PerformAttack()
        {
            var player = OverlapHitBox();
            if (player != null) _doDamage.DoDamage(player);
        }

        private void EndAttack() => Destroy(gameObject);

        private Player OverlapHitBox()
        {
            var size = _isSphere ? OverlapCircle() : OverlapBox();

            for (int i = 0; i < size; i++)
            {
                if (!_results[i].TryGetComponent(out Player player)) continue;
                return player;
            }

            return null;
        }

        private int OverlapBox()
        {
            Bounds bounds = _collider.bounds;
            return Physics2D.OverlapBoxNonAlloc(bounds.center, bounds.size, 0f, _results,
                Constants.PlayerLayer);
        }

        private int OverlapCircle() => Physics2D.OverlapCircleNonAlloc(_collider.bounds.center,
            ((CircleCollider2D)_collider).radius, _results, Constants.PlayerLayer);
    }
}