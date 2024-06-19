using System;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class HorizontalProjectileAttack : ProjectileAttack
    {
        protected override Vector2 Direction => _direction;

        private Vector2 _direction;

        public override void Setup(IEntity entity, Action onCollisionCallback)
        {
            base.Setup(entity, onCollisionCallback);
            _direction = entity.FacingLeft ? Vector2.left : Vector2.right;
        }
    }
}