using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class HazardsLayer : RepetitiveInteractive<Player>, IDoDamage
    {
        [SerializeField] private float damage = 1;
        public float Damage => damage;

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1, bool unstoppable = false) =>
            takeDamage.TakeDamage(this, damageMultiplier, true);

        protected override void TriggerInteraction(Player player) =>
            DoDamage(player);
    }
}