using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D hitBox;

        private Player _player;
        private Enemy _enemy;
        private EnemyAnimation _animation;
        private Collider2D[] _results;

        public BoxCollider2D HitBox => hitBox;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _results = new Collider2D[50];
        }

        private void Start() => _enemy.OnXFlipped += EnemyOnXFlipped;

        private void EnemyOnXFlipped(bool facingLeft)
        {
            var localX = facingLeft ? -1 : 1;
            var localPosition = HitBox.transform.localPosition;
            localPosition.x = Mathf.Abs(localPosition.x) * localX;
            HitBox.transform.localPosition = localPosition;
        }

        private void OverlapHitBox()
        {
            var size = Physics2D.OverlapBoxNonAlloc(HitBox.bounds.center, HitBox.bounds.size, 0f,
                _results, _enemy.Stats.AttackLayer);

            for (int i = 0; i < size; i++)
            {
                if (!_results[i].TryGetComponent(out Player player)) continue;
                _player = player;
                return;
            }

            _player = null;
        }

        public void Attack()
        {
            OverlapHitBox();
            if (_player) _player.TakeDamage(_enemy as IDoDamage);
        }

        public bool IsPlayerInRange()
        {
            OverlapHitBox();
            return _player;
        }

        private void OnDestroy() => _enemy.OnXFlipped -= EnemyOnXFlipped;
    }
}