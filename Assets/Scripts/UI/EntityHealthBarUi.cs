using System;
using DarkHavoc.AttackComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class EntityHealthBarUi : MonoBehaviour
    {
        [SerializeField] private Image healthBar;

        private ITakeDamage _entity;
        private float _maxHealth;
        private float _health;
        private float NormalizedHealth => _health / _maxHealth;

        private void Awake() => _entity = GetComponentInParent<ITakeDamage>();

        private void Start()
        {
            _maxHealth = _entity.Health;
            healthBar.fillAmount = 1f;
            _entity.OnDamageTaken += EntityOnDamageTaken;
        }

        private void EntityOnDamageTaken()
        {
            _health = _entity.Health;
            healthBar.fillAmount = NormalizedHealth;
        }

        private void OnDestroy()
        {
            if (_entity == null) return;
            _entity.OnDamageTaken -= EntityOnDamageTaken;
        }
    }
}