using System.Collections;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyHitBox : MonoBehaviour
    {
        public bool IsUnstoppable { get; private set; }

        [SerializeField] private float cooldown = 1f;
        [SerializeField] private bool debug;

        private Collider2D _hitBox;
        private Player _player;
        private Enemy _enemy;
        private EnemyAnimation _animation;
        private Collider2D[] _results;

        private bool _isSphere;
        private bool _onCooldown;
        private int _offsetDirection;

        private void Awake()
        {
            _hitBox = GetComponent<Collider2D>();
            _enemy = GetComponentInParent<Enemy>();
            _results = new Collider2D[50];
            _isSphere = _hitBox is CircleCollider2D;
        }

        private void Start()
        {
            _offsetDirection = transform.localPosition.x >= 0 ? 1 : -1;
            _enemy.OnXFlipped += EnemyOnXFlipped;
        }

        private void EnemyOnXFlipped(bool facingLeft)
        {
            var localX = facingLeft ? -_offsetDirection : _offsetDirection;
            var localPosition = _hitBox.transform.localPosition;
            localPosition.x = Mathf.Abs(localPosition.x) * localX;
            _hitBox.transform.localPosition = localPosition;
        }

        private void OverlapHitBox()
        {
            var size = _isSphere ? OverlapCircle() : OverlapBox();

            for (int i = 0; i < size; i++)
            {
                if (!_results[i].TryGetComponent(out Player player)) continue;
                _player = player;
                return;
            }

            _player = null;
        }

        private int OverlapBox()
        {
            Bounds bounds = _hitBox.bounds;
            return Physics2D.OverlapBoxNonAlloc(bounds.center, bounds.size, 0f, _results,
                _enemy.BaseStats.AttackLayer);
        }

        private int OverlapCircle() => Physics2D.OverlapCircleNonAlloc(_hitBox.bounds.center,
            ((CircleCollider2D)_hitBox).radius, _results, _enemy.BaseStats.AttackLayer);

        public void Attack(bool isUnstoppable = false)
        {
            IsUnstoppable = isUnstoppable;
            OverlapHitBox();
            if (_player) _player.TakeDamage(_enemy, isUnstoppable: isUnstoppable);
            StartCoroutine(CooldownAsync());
        }

        public bool IsPlayerInRange()
        {
            if (_onCooldown) return false;
            OverlapHitBox();
            return _player;
        }

        private IEnumerator CooldownAsync()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(cooldown);
            _onCooldown = false;
        }

        private void OnDestroy() => _enemy.OnXFlipped -= EnemyOnXFlipped;

        private void OnDrawGizmos()
        {
            if (!debug) return;

            _hitBox = GetComponent<Collider2D>();
            _isSphere = _hitBox is CircleCollider2D;

            Gizmos.color = Color.red;
            if (_isSphere)
            {
                Gizmos.DrawWireSphere(_hitBox.bounds.center, ((CircleCollider2D)_hitBox).radius);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position, _hitBox.bounds.size);
            }
        }
    }
}