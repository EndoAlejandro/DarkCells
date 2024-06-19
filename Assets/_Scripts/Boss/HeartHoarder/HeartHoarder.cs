using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.Enemies;
using UnityEngine;

namespace DarkHavoc.Boss.HeartHoarder
{
    public class HeartHoarder : Boss
    {
        public EnemyHitBox WalkAttackHitBox => walkAttackHitBox;
        public CompositeHitBox AirAttackHitBox => airAttackHitBox;
        public CompositeHitBox MeleeAttackHitBox => meleeAttackHitBox;
        public float WallDetectionDistance => wallDetectionDistance;
        public LayerMask WallLayerMask => wallLayerMask;

        [SerializeField] private EnemyHitBox walkAttackHitBox;
        [SerializeField] private CompositeHitBox airAttackHitBox;
        [SerializeField] private CompositeHitBox meleeAttackHitBox;
        
        [SerializeField] private float wallDetectionDistance = 1f;
        [SerializeField] private LayerMask wallLayerMask;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            var source = MidPoint.transform.position;
            Gizmos.DrawLine(source, source + Vector3.right * wallDetectionDistance);
        }

        public void Teleport(Vector3 destination)
        {
            StartCoroutine(TeleportAsync(destination));
        }

        private IEnumerator TeleportAsync(Vector3 destination)
        {
            SetCollisions(false);
            yield return null;
            transform.position = destination.With(y: transform.position.y);
            yield return null;
            SetCollisions(true);
        }

        public void SetCollisions(bool state)
        {
            rigidbody.simulated = state;
            collider.enabled = state;
        }
    }
}