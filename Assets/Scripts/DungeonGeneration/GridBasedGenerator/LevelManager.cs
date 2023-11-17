using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private float spawnPointOffset;

        private Vector3 _spawnPoint;
        private GridBasedLevelGenerator _levelGenerator;

        private void Awake() => _levelGenerator = GetComponentInChildren<GridBasedLevelGenerator>();

        private void Start()
        {
            _levelGenerator.GenerateLevel();
            var bounds = _levelGenerator.GetLevelBounds();
            CameraManager.Instance.SetCameraBounds(bounds);
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            var playerSpawnPoint = _levelGenerator.GetWorldPosition(_levelGenerator.InitialRoom);
            playerSpawnPoint.y += spawnPointOffset;
            var player = Instantiate(GameManager.Instance.PlayerPrefab, playerSpawnPoint, Quaternion.identity);
            GameManager.Instance.EnablePlayerMovement();
            GameManager.Instance.RegisterPlayer(player);
        }
    }
}