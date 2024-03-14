using System;
using System.Collections;
using DarkHavoc.Enemies;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.Interactable;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class BossLevelManager : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Enemy boss;
        [SerializeField] private ExitDoorAnimation exitDoor;
        [SerializeField] private CompositeCollider2D cameraBounds;

        private GameManager _gameManager;

        private void Start()
        {
            boss.OnDeath += BosOnDeath;
            StartCoroutine(StartBossAsync());
        }

        private IEnumerator StartBossAsync()
        {
            yield return null;
            ServiceLocator.GetService<CameraManager>().SetCameraBounds(cameraBounds);
            yield return null;
            yield return ServiceLocator.GetService<GameManager>().CreatePlayerAsync(playerSpawnPoint.position);
        }

        private void BosOnDeath(ITakeDamage takeDamage) => exitDoor.ActivateAnimation();

        private void OnDestroy() => boss.OnDeath -= BosOnDeath;
    }
}