using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Interactable
{
    public class TrapHazard : Hazard
    {
        private Player _player;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive ||_player != null) return;
            if (!other.TryGetComponent(out Player player)) return;
            _player = player;
            SetAvailable(true);
            StartCoroutine(DeactivateTrapAsync());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            _player = null;
        }

        protected override IEnumerator DeactivateTrapAsync()
        {
            yield return base.DeactivateTrapAsync();
            _player = null;
        }

        #region Animator events

        private void TrapDamage()
        {
            if (_player == null) return;
            DoDamage(_player);
        }

        #endregion
    }
}