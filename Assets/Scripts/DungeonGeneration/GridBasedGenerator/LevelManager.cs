using System.Collections;
using System.Collections.Generic;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.Serialization;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class LevelManager : Service<LevelManager>
    {
        [SerializeField] private float spawnPointOffset;
        //[SerializeField] private ExitDoor exitDoorPrefab;

        [SerializeField] private BiomeBestiary bestiary;

        private GridBasedLevelGenerator _levelGenerator;
        private CameraManager _cameraManager;
        private GameManager _gameManager;
        
        private Vector3 _spawnPoint;

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
            _cameraManager ??= ServiceLocator.Instance.GetService<CameraManager>();
            _cameraManager.SetCameraBounds(bounds);
            SpawnEnemies();
            yield return null;
            CreatePlayer();
            CreateExit();
        }

        private void CreateExit()
        {
            GridRoomData exitRoomData = _levelGenerator.ExitRoom;
            Vector3 position = _levelGenerator.GetWorldPosition(exitRoomData);
            // Instantiate(exitDoorPrefab, position, Quaternion.identity);
        }

        private void SpawnEnemies()
        {
            List<Vector3> spawnPoints = _levelGenerator.WorldPositionSpawnPoints;

            foreach (var spawnPoint in spawnPoints)
            {
                int index = Random.Range(0, bestiary.Bestiary.Length);
                Instantiate(bestiary.Bestiary[index], spawnPoint, Quaternion.identity);
            }
        }

        private void CreatePlayer()
        {
            var playerSpawnPoint = _levelGenerator.GetWorldPosition(_levelGenerator.InitialRoom);
            playerSpawnPoint.y += spawnPointOffset;
            var playerPrefab = _gameManager.PlayerPrefab;
            var player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
            _gameManager.EnableMainInput();
            _gameManager.RegisterPlayer(player);
        }

        public void ExitLevel()
        {
            _gameManager.GoToNextLevel();
        }
    }
}