using UnityEngine;

namespace DarkHavoc.Enemies.Assassin
{
    public class Assassin : Enemy
    {
        public float GetNormalizedVertical() => rigidbody.velocity.y;
        public override float Damage => 1f;
        public EnemyHitBox SlashHitBox => slashHitBox;

        [SerializeField] private EnemyHitBox slashHitBox;

        public void ResetVelocity()
        {
            rigidbody.velocity = Vector2.zero;
            targetVelocity = Vector2.zero;
        }

        public void Jump(bool lightJump = false)
        {
            if (!Grounded) return;
            float scale = lightJump ? .5f : 1f;
            targetVelocity.y = Stats.JumpForce * scale;
        }
    }
}