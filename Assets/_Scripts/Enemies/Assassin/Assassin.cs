using UnityEngine;

namespace DarkHavoc.Enemies.Assassin
{
    public class Assassin : Enemy
    {
        public override float Damage => 1f;
        public EnemyHitBox SlashHitBox => slashHitBox;
        [SerializeField] private EnemyHitBox slashHitBox;
    }
}