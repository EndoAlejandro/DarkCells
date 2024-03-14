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

        protected IEnemy entity;
        protected Collider2D[] results;
        
        private Collider2D _collider;
        private EnemyAnimation _animation;
        protected Player player;
        protected IDoDamage doDamage;

        protected bool isCircle;
        private bool _onCooldown;
        private int _offsetDirection;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
            doDamage = GetComponentInParent<IDoDamage>();
            entity = GetComponentInParent<IEnemy>();
            results = new Collider2D[50];
            isCircle = _collider is CircleCollider2D;
        }

        private void Start()
        {
            _offsetDirection = transform.localPosition.x >= 0 ? 1 : -1;
            entity.OnXFlipped += IEntityOnXFlipped;
        }

        private void IEntityOnXFlipped(bool facingLeft)
        {
            var localX = facingLeft ? -_offsetDirection : _offsetDirection;
            var localPosition = _collider.transform.localPosition;
            localPosition.x = Mathf.Abs(localPosition.x) * localX;
            _collider.transform.localPosition = localPosition;
        }

        protected virtual void OverlapHitBox()
        {
            var size = isCircle ? OverlapCircle() : OverlapBox();

            for (int i = 0; i < size; i++)
            {
                if (!results[i].TryGetComponent(out Player player)) continue;
                this.player = player;
                return;
            }

            player = null;
        }

        protected int OverlapBox()
        {
            Bounds bounds = _collider.bounds;
            return Physics2D.OverlapBoxNonAlloc(bounds.center, bounds.size, 0f, results,
                Constants.PlayerLayer);
        }

        protected int OverlapCircle() => Physics2D.OverlapCircleNonAlloc(_collider.bounds.center,
            ((CircleCollider2D)_collider).radius, results, Constants.PlayerLayer);

        public void SetUnstoppable(bool value) => IsUnstoppable = value;

        /// <summary>
        /// Executes attack and try to deal damage to player.
        /// </summary>
        /// <param name="isUnstoppable"></param>
        /// <returns>Returns true if successfully do damage.</returns>
        public virtual DamageResult TryToAttack(bool isUnstoppable = false)
        {
            SetUnstoppable(isUnstoppable);
            OverlapHitBox();

            DamageResult result = DamageResult.Failed;
            if (player) result = player.TakeDamage(doDamage, isUnstoppable: isUnstoppable);

            StartCoroutine(CooldownAsync());
            return result;
        }

        public virtual bool IsPlayerInRange()
        {
            if (_onCooldown) return false;
            OverlapHitBox();
            return player;
        }

        protected IEnumerator CooldownAsync()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(cooldown);
            _onCooldown = false;
        }

        private void OnDestroy() => entity.OnXFlipped -= IEntityOnXFlipped;

        private void OnDrawGizmos()
        {
            if (!debug) return;

            _collider = GetComponent<Collider2D>();
            isCircle = _collider is CircleCollider2D;

            Gizmos.color = Color.red;
            if (isCircle)
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