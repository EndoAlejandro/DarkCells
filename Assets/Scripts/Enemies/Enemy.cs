using System;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected EnemyStats stats;
        public event Action<bool> OnXFlipped;
        public bool FacingLeft { get; private set; }
        public EnemyStats BaseStats => stats;

        public abstract float GetNormalizedHorizontal();

        public void SetFacingLeft(bool facingLeft)
        {
            FacingLeft = facingLeft;
            OnXFlipped?.Invoke(FacingLeft);
        }
    }
}