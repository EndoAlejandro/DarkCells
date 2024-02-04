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

        #region Debug

        private void OnDrawGizmos()
        {
            if (!debug) return;
            stats ??= ScriptableObject.CreateInstance<EnemyStats>();
            if (collider == null) collider = GetComponent<CapsuleCollider2D>();

            DetectionRange(MidPoint.position);
            Gizmos.color = Color.magenta;
            GroundRays(transform.position);
            WallRays();
        }

        private void DetectionRange(Vector3 position)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(position, Stats.DetectionDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, Stats.ScapeDistance);
        }

        private void GroundRays(Vector3 position)
        {
            var offset = Vector3.up * Stats.FootPositionOffset.y;
            var distance = Vector3.down * (Stats.GroundOffset + Stats.GroundCheckDistance);
            // Left Foot.
            var leftFootPosition =
                new Vector3(collider.bounds.min.x - Stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(leftFootPosition + offset, leftFootPosition + offset + distance);
            // Right Foot.
            var rightFootPosition =
                new Vector3(collider.bounds.max.x + Stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(rightFootPosition + offset, rightFootPosition + offset + distance);
        }

        private void WallRays()
        {
            float horizontal = FacingLeft ? collider.bounds.min.x : collider.bounds.max.x;
            horizontal += Stats.WallDetection.HorizontalOffset;
            var direction = FacingLeft ? Vector2.left : Vector2.right;
            // Top Ray.
            Vector2 topOrigin = new Vector2(horizontal, collider.bounds.max.y + Stats.WallDetection.TopOffset);
            Gizmos.DrawLine(topOrigin, topOrigin + (direction * Stats.WallDetection.DistanceCheck));
            // Middle Ray.
            var centerOrigin = new Vector2(horizontal, collider.bounds.center.y + Stats.WallDetection.MidOffset);
            Gizmos.DrawLine(centerOrigin, centerOrigin + (direction * Stats.WallDetection.DistanceCheck));
            // Bottom Ray.
            Vector2 bottomOrigin = new Vector2(horizontal, collider.bounds.min.y + Stats.WallDetection.BottomOffset);
            Gizmos.DrawLine(bottomOrigin, bottomOrigin + (direction * Stats.WallDetection.DistanceCheck));
        }

        #endregion
    }
}