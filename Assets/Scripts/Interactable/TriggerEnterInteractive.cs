using System.Collections;
using UnityEngine;

namespace DarkHavoc.Interactable
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TriggerEnterInteractive<T> : MonoBehaviour
    {
        [SerializeField] protected float cooldown;

        private bool _available;

        protected virtual void OnEnable() => _available = true;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_available || !other.TryGetComponent(out T t)) return;
            StartCoroutine(TriggerInteractionAsync(t));
        }

        private IEnumerator TriggerInteractionAsync(T t)
        {
            SetAvailable(false);
            TriggerInteraction(t);
            yield return new WaitForSeconds(cooldown);
            SetAvailable(true);
        }

        protected virtual void SetAvailable(bool value) => _available = value;

        protected abstract void TriggerInteraction(T t);
    }
}