using System.Collections;
using System.Collections.Generic;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class LevelManager : Service<LevelManager>
    {
        [SerializeField] private float spawnPointOffset;

        [SerializeField] private BiomeBestiary bestiary;

        private GridBasedLevelGenerator _levelGenerator;
        private CameraManager _cameraManager;
        private GameManager _gameManager;

        private Vector3 _spawnPoint;

        private void Start()
        {
            _levelGenerator = ServiceLocator.GetService<GridBasedLevelGenerator>();
            _gameManager = ServiceLocator.GetService<GameManager>();
            StartCoroutine(StartLevelAsync());
        }

        private IEnumerator StartLevelAsync()
        {
            yield return null;
            _levelGenerator.GenerateLevel();
            yield return null;
            CompositeCollider2D bounds = _levelGenerator.GetLevelBounds();
            _cameraManager ??= ServiceLocator.GetService<CameraManager>();
            _cameraManager.SetCameraBounds(bounds);
            SpawnEnemies();
            SpawnInstantiables();
            yield return null;
            yield return CreatePlayerAsync();
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

        private void SpawnInstantiables()
        {
            List<Instantiable> instantiables = _levelGenerator.Instantiables;

            foreach (var instantiable in instantiables)
                Instantiate(instantiable.Prefab, instantiable.WorldPosition, Quaternion.identity);
        }

        private IEnumerator CreatePlayerAsync()
        {
            var playerSpawnPoint = _levelGenerator.GetWorldPosition(_levelGenerator.InitialRoom);
            playerSpawnPoint.y += spawnPointOffset;
            var playerPrefab = _gameManager.PlayerPrefab;
            Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
            yield return null;
            _gameManager.EnableMainInput();
        }

        public void ExitLevel()
        {
            _gameManager.GoToNextLevel();
        }
    }
}