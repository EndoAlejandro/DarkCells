using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Colossal : Boss
    {
        public ColossalBoomerangArms BoomerangArms => boomerangArms;
        public EnemyHitBox RangedHitBox => rangedHitBox;
        public EnemyHitBox MeleeHitBox => meleeHitBox;
        public EnemyHitBox BuffHitBox => buffHitBox;

        [SerializeField] private ColossalBoomerangArms boomerangArms;
        [SerializeField] private EnemyHitBox rangedHitBox;
        [SerializeField] private EnemyHitBox meleeHitBox;
        [SerializeField] private EnemyHitBox buffHitBox;

        private float _initialHeight;

        private void Start() => _initialHeight = transform.position.y;

        private void Update()
        {
            if (transform.position.y != _initialHeight)
                transform.position = transform.position.With(y: _initialHeight);
        }
    }
}