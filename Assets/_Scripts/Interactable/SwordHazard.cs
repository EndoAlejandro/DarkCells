using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Interactable
{
    public class SwordHazard : Hazard
    {
        [SerializeField] private float damageCooldown = 1f;

        private IEnumerator _damageCooldownAsync;
        private bool _canDoDamage;

        protected override void Awake()
        {
            base.Awake();
            _canDoDamage = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_canDoDamage) return;
            if (!other.TryGetComponent(out Player player)) return;
            if (isActive) DoDamage(player);
            else ActivateHazard();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            StopCoroutine(_damageCooldownAsync);
            _canDoDamage = true;
        }

        public override void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1, bool unstoppable = false)
        {
            if (!_canDoDamage) return;
            base.DoDamage(takeDamage, damageMultiplier, true);
            _canDoDamage = false;
            _damageCooldownAsync = DamageCooldownAsync();
            StartCoroutine(_damageCooldownAsync);
        }

        private IEnumerator DamageCooldownAsync()
        {
            yield return new WaitForSeconds(damageCooldown);
            _canDoDamage = true;
        }

        #region Animator events

        private void EnableDamage() => _canDoDamage = true;
        private void DisableDamage() => _canDoDamage = false;

        #endregion
    }
}