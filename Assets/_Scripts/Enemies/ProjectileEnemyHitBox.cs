using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class ProjectileEnemyHitBox : EnemyHitBox
    {
        [SerializeField] private ProjectileAttack projectileAttackPrefab;
        [SerializeField] private StaticRangedAttack projectileExplosionPrefab;

        public override DamageResult TryToAttack(bool isUnstoppable = false)
        {
            SetUnstoppable(isUnstoppable);

            DamageResult result = DamageResult.Failed;
            if (entity.Player)
            {
                result = DamageResult.Success;
                var projectileAttack = Instantiate(projectileAttackPrefab, entity.MidPoint.position, Quaternion.identity);
                projectileAttack.Setup(entity,() =>
                {
                    var projectileExplosion = Instantiate(projectileExplosionPrefab, projectileAttack.transform.position,
                        Quaternion.identity);
                    projectileExplosion.Setup(doDamage);
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