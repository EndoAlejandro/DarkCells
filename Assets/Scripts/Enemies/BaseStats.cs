using UnityEngine;

namespace DarkHavoc.Enemies
{
    public abstract class BaseStats : ScriptableObject
    {
        [Header("Base Stats")]
        [SerializeField] private int maxHealth = 10;

        [SerializeField] private float idleTime = 1f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float maxSpeed = 1.5f;

        public int MaxHealth => maxHealth;
        public float IdleTime => idleTime;
        public float Acceleration => acceleration;
        public float MaxSpeed => maxSpeed;
    }
}