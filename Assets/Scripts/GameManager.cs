using System;
using System.Collections;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class GameManager : Service<GameManager>
    {
        protected override bool DonDestroyOnLoad => true;
        public static event Action<bool> OnSetInputEnabled;
        public static event Action<bool> OnGamePauseChanged;

        public Player Player { get; set; }

        private IEnumerator _currentTransition;
        private ImputReader _inputReader;
        private bool _paused;

        protected override void Awake()
        {
            base.Awake();

            _inputReader = new ImputReader();
            ServiceLocator.Instance.AddService(_inputReader);
        }

        private void Update()
        {
            if (!_inputReader.Pause) return;
            _paused = !_paused;
            if (_paused) PauseGame();
            else UnpauseGame();
        }

        private void ActivateInput() => OnSetInputEnabled?.Invoke(true);
        private void DeactivateInput() => OnSetInputEnabled?.Invoke(false);

        private void PauseGame()
        {
            OnGamePauseChanged?.Invoke(true);
            Time.timeScale = 0;
        }

        private void UnpauseGame()
        {
            OnGamePauseChanged?.Invoke(false);
            Time.timeScale = 1;
        }

        public void EnablePlayerMovement() => ActivateInput();
        public void RegisterPlayer(Player player) => Player = player;
    }
}