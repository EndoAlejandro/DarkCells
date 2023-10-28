using System;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [Serializable]
    public struct ImpulseAction
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
}