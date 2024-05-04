using DarkHavoc.Managers;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc
{
    [RequireComponent(typeof(Collider2D))]
    public class LobbyController : MonoBehaviour
    {
        private Collider2D _collider;

        protected virtual void Awake() => _collider = GetComponent<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            ServiceLocator.GetService<GameManager>()?.StartGame();
            _collider.enabled = false;
        }
    }
}