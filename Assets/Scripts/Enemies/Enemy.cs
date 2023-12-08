using System;
using DarkHavoc.Enemies.CagedShocker;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] protected CagedShockerStats stats;

        public event Action<bool> OnXFlipped;
        public bool FacingLeft { get; private set; }
        public CagedShockerStats Stats => stats;

        public void SetFacingLeft(bool facingLeft)
        {
            FacingLeft = facingLeft;
            OnXFlipped?.Invoke(FacingLeft);
        }
    }
}