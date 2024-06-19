using UnityEngine;

namespace DarkHavoc.Enemies.ShockSweeper
{
    public class ShockSweeper : Enemy
    {
        public override float Damage => 1f;
        public EnemyHitBox HeavyHitBox => heavyHitBox;
        public SummonAttackEnemyHitBox StaticRangedHitBox => summonRangedHitBox;

        [SerializeField] private EnemyHitBox heavyHitBox;
        [SerializeField] private SummonAttackEnemyHitBox summonRangedHitBox;
    }
}