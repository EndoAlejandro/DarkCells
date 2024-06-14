using System.Collections;
using System.Collections.Generic;
using DarkHavoc.Managers;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class BiomeLevelManager : MonoBehaviour
    {
        [SerializeField] private float spawnPointOffset;
        [SerializeField] private BiomeBestiary bestiary;

        private LevelGenerator _levelGenerator;
        private GameManager _gameManager;

        private void Start()
        {
            _levelGenerator = ServiceLocator.GetService<LevelGenerator>();
            _gameManager = ServiceLocator.GetService<GameManager>();
            StartCoroutine(StartLevelAsync());
        }

        private IEnumerator StartLevelAsync()
        {
            yield return null;
            _levelGenerator.GenerateLevel();
            yield return null;
            CompositeCollider2D bounds = _levelGenerator.GetLevelBounds();
            ServiceLocator.GetService<CameraManager>()?.SetCameraBounds(bounds);
            SpawnEnemies();
            SpawnInstantiables();
            yield return null;
            yield return CreatePlayerAsync();
        }

        private void SpawnEnemies()
        {
            List<Vector3> spawnPoints = _levelGenerator.WorldPositionSpawnPoints;

            foreach (var spawnPoint in spawnPoints)
            {
                int index = Random.Range(0, bestiary.Bestiary.Length);
                Instantiate(bestiary.Bestiary[index], spawnPoint, Quaternion.identity, transform);
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
            yield return _gameManager.CreatePlayerAsync(playerSpawnPoint);
        }
    }
}