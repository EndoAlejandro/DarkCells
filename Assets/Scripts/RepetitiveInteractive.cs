using System.Collections;
using UnityEngine;

namespace DarkHavoc
{
    public abstract class RepetitiveInteractive<T> : MonoBehaviour
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
            _available = false;
            TriggerInteraction(t);
            yield return new WaitForSeconds(cooldown);
            _available = true;
        }

        protected abstract void TriggerInteraction(T t);
    }
}