using System;
using DarkHavoc.PlayerComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Image healthBar;

        // TODO: Inject player.
        [SerializeField] private Player player;

        private float _maxHealth;
        private float _currentHealth;
        private float NormalizedHealth => _currentHealth / _maxHealth;

        private void Awake() => player.OnDamageTaken += PlayerOnDamageTaken;
        private void Start() => _maxHealth = player.Stats.MaxHealth;

        private void PlayerOnDamageTaken()
        {
            _currentHealth = player.Health;
            healthBar.fillAmount = 1 - NormalizedHealth;
        }

        private void OnDestroy()
        {
            if (player == null) return;
            player.OnDamageTaken -= PlayerOnDamageTaken;
        }
    }
}