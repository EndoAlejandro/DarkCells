using System.Collections;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class LobbyController : MonoBehaviour
    {
        private TransitionManager _transitionManager;

        private void Start()
        {
            _transitionManager= ServiceLocator.Instance.GetService<TransitionManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            _transitionManager.LoadBiomeScene(Biome.ForgottenCatacombs);
        }
    }
}