using UnityEngine;

namespace DarkHavoc.Enemies.TarnishedWidow
{
    public class TarnishedWidow : Boss
    {
        [SerializeField] private EnemyHitBox meleeHitBox;
        [SerializeField] private EnemyHitBox rangedHitBox;
        [SerializeField] private EnemyHitBox buffHitBox;
        [SerializeField] private EnemyHitBox jumpHitBox;
        public EnemyHitBox MeleeHitBox => meleeHitBox;
        public EnemyHitBox RangedHitBox => rangedHitBox;
        public EnemyHitBox BuffHitBox => buffHitBox;
        public EnemyHitBox JumpHitBox => jumpHitBox;

        public void JumpUp()
        {
            collider.enabled = false;
            rigidbody.simulated = false;
        }

        public void Teleport(Vector3 newPosition) => transform.position = newPosition;

        public void JumpDown()
        {
            collider.enabled = true;
            rigidbody.simulated = true;
        }
    }
}