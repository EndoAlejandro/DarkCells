using UnityEngine;

namespace DarkHavoc.Enemies.DaggerMush
{
    public class DaggerMush : Enemy
    {
        public override float Damage => 1f;
        [SerializeField] private EnemyHitBox slashHitBox;
        public EnemyHitBox SlashHitBox => slashHitBox;
    }
}