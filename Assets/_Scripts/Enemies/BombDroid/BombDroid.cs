using UnityEngine;

namespace DarkHavoc.Enemies.BombDroid
{
    public class BombDroid : Enemy
    {
        public override float Damage => 1f;

        [SerializeField] private LayerMask groundOnlyLayerMask;
        public LayerMask GroundOnlyLayerMask => groundOnlyLayerMask;

        public void VerticalMove(float direction)
        {
            if (direction == 0)
            {
                var deceleration = Stats.Acceleration * 3;
                targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                var acceleration = Stats.Acceleration;
                var speed = Player == null ? Stats.PatrolSpeed : Stats.MaxSpeed;
                targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, direction * speed,
                    acceleration * Time.fixedDeltaTime);
            }
        }

        protected override void CustomGravity()
        {
        }

        [SerializeField] private bool playerVisible;
        public void SetVisibility(bool value) => playerVisible = value;
    }
}