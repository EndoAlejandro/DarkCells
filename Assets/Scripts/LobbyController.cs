using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class LobbyController : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Start() => _gameManager = ServiceLocator.Instance.GetService<GameManager>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            _gameManager.GoToNextBiome();
        }
    }
}