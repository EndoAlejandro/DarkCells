using UnityEngine;

namespace DarkHavoc.Enemies.OrbMage
{
    public class OrbMage : Enemy
    {
        public override float Damage => 1f;
        public BuffOnlyEnemyHitBox BuffHitBox => buffHitBox;
        public SummonAttackEnemyHitBox RangedHitBox => rangedHitBox;
        
        [SerializeField] private BuffOnlyEnemyHitBox buffHitBox;
        [SerializeField] private SummonAttackEnemyHitBox rangedHitBox;
    }
}