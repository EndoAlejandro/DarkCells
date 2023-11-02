using System;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.ImpulseComponents
{
    [Serializable]
    public class ImpulseAction
    {
        [SerializeField] private float time;
        [SerializeField] private float force;
        [SerializeField] private float deceleration;
        [SerializeField] private ImpulseActionExtensions.ImpulseDirection direction;
        public float Time => time;
        public float Force => force;
        public float Deceleration => deceleration;
        public int Direction => direction == ImpulseActionExtensions.ImpulseDirection.Forward ? 1 : -1;
    }

    [Serializable]
    public class AttackImpulseAction : ImpulseAction
    {
        [Range(1f, 2f)] [SerializeField] private float damageMultiplier;
        [SerializeField] private float cooldownTime;
        public float DamageMultiplier => damageMultiplier;
        public float CoolDownTime => cooldownTime;
    }
}