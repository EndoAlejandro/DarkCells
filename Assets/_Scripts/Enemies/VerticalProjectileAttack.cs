using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class VerticalProjectileAttack : ProjectileAttack
    {
        protected override Vector2 Direction => Vector2.down;
    }
}