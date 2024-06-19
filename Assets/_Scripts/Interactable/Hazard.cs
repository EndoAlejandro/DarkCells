using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Interactable
{
    public abstract class Hazard : MonoBehaviour, IDoDamage
    {
        private static readonly int Active = Animator.StringToHash("Active");
        public float Damage => damage;

        [SerializeField] protected float timeActive = 4f;
        [SerializeField] protected float damage = 1f;

        private Animator _animator;
        protected bool isActive;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public virtual void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1, bool unstoppable = false) =>
            takeDamage.TakeDamage(this, damageMultiplier, true);

        protected void SetAvailable(bool value)
        {
            _animator.SetBool(Active, value);
            isActive = value;
        }
        
        protected void ActivateHazard()
        {
            SetAvailable(true);
            StartCoroutine(DeactivateTrapAsync());
        }
        
        protected virtual IEnumerator DeactivateTrapAsync()
        {
            yield return new WaitForSeconds(timeActive);
            SetAvailable(false);
        }
    }
}