using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Interactable
{
    public class HazardsLayer : MonoBehaviour, IDoDamage
    {
        public float Damage => damage;

        [SerializeField] private float damage = 1;
        [SerializeField] protected float cooldown;

        private bool _available;

        protected virtual void OnEnable() => _available = true;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_available || !other.TryGetComponent(out Player player)) return;
            StartCoroutine(TriggerInteractionAsync(player));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            StopAllCoroutines();
            SetAvailable(true);
        }

        private IEnumerator TriggerInteractionAsync(Player player)
        {
            SetAvailable(false);
            TriggerInteraction(player);
            yield return new WaitForSeconds(cooldown);
            SetAvailable(true);
        }

        protected virtual void SetAvailable(bool value) => _available = value;
        private void TriggerInteraction(Player player) => DoDamage(player);

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1, bool unstoppable = false) =>
            takeDamage.TakeDamage(this, damageMultiplier, true);
    }
}