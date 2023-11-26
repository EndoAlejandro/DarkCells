using System.Collections;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class LevelManager : Service<LevelManager>
    {
        [SerializeField] private float spawnPointOffset;

        private Vector3 _spawnPoint;
        private GridBasedLevelGenerator _levelGenerator;
        private GameManager _gameManager;

        private void Start()
        {
            _levelGenerator = ServiceLocator.Instance.GetService<GridBasedLevelGenerator>();
            _gameManager = ServiceLocator.Instance.GetService<GameManager>();
            StartCoroutine(StartLevelAsync());
        }

        private IEnumerator StartLevelAsync()
        {
            yield return null;
            _levelGenerator.GenerateLevel();
            yield return null;
            CompositeCollider2D bounds = _levelGenerator.GetLevelBounds();
            CameraManager.Instance.SetCameraBounds(bounds);
            yield return null;
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            var playerSpawnPoint = _levelGenerator.GetWorldPosition(_levelGenerator.InitialRoom);
            playerSpawnPoint.y += spawnPointOffset;
            var playerPrefab = ServiceLocator.Instance.GetService<Player>();
            var player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
            _gameManager.EnablePlayerMovement();
            _gameManager.RegisterPlayer(player);
        }
    }
}