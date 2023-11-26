using System;
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
        public bool Paused { get; private set; }

        [SerializeField] private Player playerPrefab;

        private TransitionManager _transitionManager;
        private InputReader _inputReader;
        private Biome _currentBiome;

        protected override void Awake()
        {
            base.Awake();

            _inputReader = new InputReader();
            ServiceLocator.Instance.TryToRegisterService(_inputReader);
        }

        private void Start()
        {
            _currentBiome = Biome.ForgottenCatacombs;
            _transitionManager = ServiceLocator.Instance.GetService<TransitionManager>();

            _inputReader.DisableMainInput();
        }

        public void GoToLobby()
        {
            DisableMainInput();
            _transitionManager.LoadLobbyScene();
        }

        public void GoToNextBiome() => _transitionManager.LoadBiomeScene(_currentBiome);

        public void SetPauseInput(bool state) => _inputReader.SetPauseEnable(state);

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

        public void PauseGame()
        {
            Paused = true;
            DisableMainInput();
            SetPauseInput(false);
            OnGamePauseChanged?.Invoke(true);
        }

        public void UnpauseGame()
        {
            Paused = false;
            EnableMainInput();
            SetPauseInput(true);
            OnGamePauseChanged?.Invoke(false);
        }

        public void RegisterPlayer(Player player) => Player = player;
    }
}