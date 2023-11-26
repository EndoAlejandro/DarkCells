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
        public Player PlayerPrefab => playerPrefab;

        [SerializeField] private Player playerPrefab;

        private TransitionManager _transitionManager;
        private InputReader _inputReader;
        private bool _paused;
        private Biome _currentBiome;

        protected override void Awake()
        {
            base.Awake();

            _inputReader = new InputReader();
            ServiceLocator.Instance.AddService(_inputReader);
        }

        private void Start()
        {
            _currentBiome = Biome.ForgottenCatacombs;
            _transitionManager = ServiceLocator.Instance.GetService<TransitionManager>();
            
            _inputReader.DisableMainInput();
        }

        private void Update()
        {
            if (!_inputReader.Pause) return;
            _paused = !_paused;
            if (_paused) PauseGame();
            else UnpauseGame();
        }

        public void GoToLobby() => _transitionManager.LoadLobbyScene();
        public void GoToNextBiome() => _transitionManager.LoadBiomeScene(_currentBiome);

        public void EnableMainInput()
        {
            _inputReader.EnableMainInput();
            OnSetInputEnabled?.Invoke(true);
        }

        public void DisableMainInput()
        {
            _inputReader.DisableMainInput();
            OnSetInputEnabled?.Invoke(false);
        }

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

        public void RegisterPlayer(Player player) => Player = player;
    }
}