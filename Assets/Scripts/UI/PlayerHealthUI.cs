using DarkHavoc.PlayerComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        private static readonly int Out = Animator.StringToHash("Out");
        private static readonly int In = Animator.StringToHash("In");

        [SerializeField] private Image healthBar;

        private Player _player;
        private Animator _animator;

        private float _maxHealth;
        private float _currentHealth;
        private float NormalizedHealth => 1f - (_currentHealth / _maxHealth);

        private void OnEnable() => _animator = GetComponent<Animator>();

        private void Start()
        {
            Player.OnPlayerSpawned += PlayerOnPlayerSpawned;
            Player.OnPlayerDeSpawned += PlayerOnPlayerDeSpawned;
        }

        private void PlayerOnPlayerSpawned(Player player)
        {
            _animator = GetComponent<Animator>();
            _player = player;

            _maxHealth = _player.MaxHealth;
            _currentHealth = _player.Health;
            healthBar.fillAmount = NormalizedHealth;

            _animator.SetTrigger(In);
            player.OnDamageTaken += PlayerOnDamageTaken;
        }

        private void PlayerOnPlayerDeSpawned(Player player)
        {
            player.OnDamageTaken -= PlayerOnDamageTaken;
            _player = null;
        }

        private void PlayerOnDamageTaken()
        {
            _currentHealth = _player.Health;
            healthBar.fillAmount = NormalizedHealth;
        }

        private void OnDestroy()
        {
            Player.OnPlayerSpawned -= PlayerOnPlayerSpawned;
            Player.OnPlayerDeSpawned -= PlayerOnPlayerDeSpawned;
        }
    }
}