using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class HazardsLayer : InteractiveTrigger<Player>, IDoDamage
    {
        [SerializeField] private float damage = 1;
        public float Damage => damage;

        protected override void TriggerInteraction(Player player) =>
            DoDamage(player);

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1) =>
            takeDamage.TakeDamage(this, damageMultiplier);
    }
}