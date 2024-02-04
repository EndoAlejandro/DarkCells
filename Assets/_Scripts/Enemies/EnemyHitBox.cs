using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyHitBox : MonoBehaviour
    {
        public float TelegraphTime => telegraphTime;
        public bool IsUnstoppable { get; private set; }

        [SerializeField] private float cooldown = 1f;
        [SerializeField] private float telegraphTime = 1f;
        [SerializeField] private bool debug;

        private Collider2D _collider;
        private Player _player;
        private IEntity _entity;
        private IDoDamage _doDamage;
        private EnemyAnimation _animation;
        private Collider2D[] _results;

        private bool _isSphere;
        private bool _onCooldown;
        private int _offsetDirection;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _entity = GetComponentInParent<IEntity>();
            _doDamage = GetComponentInParent<IDoDamage>();
            _results = new Collider2D[50];
            _isSphere = _collider is CircleCollider2D;
        }

        private void Start()
        {
            _offsetDirection = transform.localPosition.x >= 0 ? 1 : -1;
            _entity.OnXFlipped += IEntityOnXFlipped;
        }

        private void IEntityOnXFlipped(bool facingLeft)
        {
            var localX = facingLeft ? -_offsetDirection : _offsetDirection;
            var localPosition = _collider.transform.localPosition;
            localPosition.x = Mathf.Abs(localPosition.x) * localX;
            _collider.transform.localPosition = localPosition;
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
            Bounds bounds = _collider.bounds;
            return Physics2D.OverlapBoxNonAlloc(bounds.center, bounds.size, 0f, _results,
                Constants.PlayerLayer);
        }

        private int OverlapCircle() => Physics2D.OverlapCircleNonAlloc(_collider.bounds.center,
            ((CircleCollider2D)_collider).radius, _results, Constants.PlayerLayer);

        public void SetUnstoppable(bool value) => IsUnstoppable = value;

        /// <summary>
        /// Executes attack and try to deal damage to player.
        /// </summary>
        /// <param name="isUnstoppable"></param>
        /// <returns>Returns true if successfully do damage.</returns>
        public DamageResult TryToAttack(bool isUnstoppable = false)
        {
            SetUnstoppable(isUnstoppable);
            OverlapHitBox();
            
            DamageResult result = DamageResult.Failed;
            if (_player) result = _player.TakeDamage(_doDamage, isUnstoppable: isUnstoppable);
            
            StartCoroutine(CooldownAsync());
            return result;
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

        private void OnDestroy() => _entity.OnXFlipped -= IEntityOnXFlipped;

        private void OnDrawGizmos()
        {
            if (!debug) return;

            _collider = GetComponent<Collider2D>();
            _isSphere = _collider is CircleCollider2D;

            Gizmos.color = Color.red;
            if (_isSphere)
            {
                Gizmos.DrawWireSphere(_collider.bounds.center, ((CircleCollider2D)_collider).radius);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
            }
        }
    }
}