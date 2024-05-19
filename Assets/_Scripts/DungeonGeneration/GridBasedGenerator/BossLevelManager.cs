using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.Interactable;
using DarkHavoc.Managers;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class BossLevelManager : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Boss.Boss boss;
        [SerializeField] private ExitDoorAnimation exitDoor;
        [SerializeField] private CompositeCollider2D cameraBounds;

        private GameManager _gameManager;

        private void Start()
        {
            boss.OnDeath += BossOnDeath;
            StartCoroutine(StartBossAsync());
        }

        private IEnumerator StartBossAsync()
        {
            yield return null;
            ServiceLocator.GetService<CameraManager>()?.SetCameraBounds(cameraBounds);
            yield return null;
            yield return ServiceLocator.GetService<GameManager>()?.CreatePlayerAsync(playerSpawnPoint.position);
        }

        private void BossOnDeath(ITakeDamage takeDamage) => exitDoor.ActivateAnimation();

        private void OnDestroy() => boss.OnDeath -= BossOnDeath;
    }
}