using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class BuffOnlyEnemyHitBox : EnemyHitBox
    {
        [SerializeField] private float buffDuration = 5f;

        public override DamageResult TryToAttack(bool isUnstoppable = false)
        {
            enemy.ActivateBuff(buffDuration);
            StartCoroutine(CooldownAsync());
            return DamageResult.Success;
        }
    }
}