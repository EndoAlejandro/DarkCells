using UnityEngine;

namespace DarkHavoc
{
    public class BiomeController : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;

        private void Start()
        {
            Instantiate(GameManager.Instance.PlayerPrefab, spawnPoint.position, spawnPoint.rotation);
            GameManager.Instance.EnablePlayerMovement();
        }
    }
}
