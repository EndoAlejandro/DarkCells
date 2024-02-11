using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class BombEnemyHitBox : EnemyHitBox
    {
        [SerializeField] private BombRangedAttack bombRangedAttackPrefab;
        [SerializeField] private StaticRangedAttack bombExplosionPrefab;

        public override DamageResult TryToAttack(bool isUnstoppable = false)
        {
            SetUnstoppable(isUnstoppable);

            DamageResult result = DamageResult.Failed;
            if (enemy.Player)
            {
                result = DamageResult.Success;
                var bombAttack = Instantiate(bombRangedAttackPrefab, enemy.transform.position, Quaternion.identity);
                bombAttack.Setup(() =>
                {
                    var bombExplosion = Instantiate(bombExplosionPrefab, bombAttack.transform.position,
                        Quaternion.identity);
                    bombExplosion.Setup(enemy);
                });
            }
            else
            {
                Debug.Log("Failed");
            }

            StartCoroutine(CooldownAsync());
            return result;
        }
    }
}