using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class HealthUI : Service<HealthUI>
    {
        [SerializeField] private Image healthBar;

        private ITakeDamage _takeDamage;
        private float NormalizedHealth => _takeDamage.Health / _takeDamage.MaxHealth;

        private void OnEnable() => SetActive(false);

        public void Setup(ITakeDamage takeDamage)
        {
            _takeDamage = takeDamage;
            _takeDamage.OnDamageTaken += TakeDamageOnDamageTaken;
            UpdateHealthBar();
            SetActive(true);
        }

        private void SetActive(bool isActive) => healthBar.gameObject.SetActive(isActive);
        private void TakeDamageOnDamageTaken() => UpdateHealthBar();
        private void UpdateHealthBar() => healthBar.fillAmount = NormalizedHealth;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _takeDamage.OnDamageTaken -= TakeDamageOnDamageTaken;
        }
    }
}