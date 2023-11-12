using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class LobbyController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.TryGetComponent(out Player player)) return;
            GameManager.Instance.LoadBiomeScene(Biome.ForgottenCatacombs);
        }
    }
}