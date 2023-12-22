using System;
using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class HazardsLayer : MonoBehaviour, IDoDamage
    {
        public float Damage => damage;

        [SerializeField] private float cooldown = 1f;
        [SerializeField] private float damage = 1;

        private bool _canDoDamage;

        private void OnEnable() => _canDoDamage = true;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            if (_canDoDamage) StartCoroutine(DoDamageAsync(player));
        }

        private IEnumerator DoDamageAsync(Player player)
        {
            _canDoDamage = false;
            player.TakeDamage(this);
            yield return new WaitForSeconds(cooldown);
            _canDoDamage = true;
        }

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1)
        {
            takeDamage.TakeDamage(this, damageMultiplier);
        }
    }
}