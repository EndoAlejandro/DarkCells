using System;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Image healthBar;

        private ITakeDamage _takeDamage;
        private float NormalizedHealth => _takeDamage.Health / _takeDamage.MaxHealth;

        private void Awake()
        {
            Player.OnPlayerSpawned += PlayerOnPlayerSpawned;
            Player.OnPlayerDeSpawned += PlayerOnPlayerDeSpawned;
            SetActive(false);
        }

        private void PlayerOnPlayerSpawned(Player player) => Setup(player);
        private void PlayerOnPlayerDeSpawned(Player player) => SetActive(false);

        private void Setup(ITakeDamage takeDamage)
        {
            _takeDamage = takeDamage;
            _takeDamage.OnDamageTaken += TakeDamageOnDamageTaken;
            UpdateHealthBar();
            SetActive(true);
        }

        private void SetActive(bool isActive) => healthBar.gameObject.SetActive(isActive);
        private void TakeDamageOnDamageTaken() => UpdateHealthBar();
        private void UpdateHealthBar() => healthBar.fillAmount = NormalizedHealth;

        private void OnDestroy()
        {
            _takeDamage.OnDamageTaken -= TakeDamageOnDamageTaken;
            Player.OnPlayerSpawned -= PlayerOnPlayerSpawned;
            Player.OnPlayerDeSpawned -= PlayerOnPlayerDeSpawned;
        }
    }
}