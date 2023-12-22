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
        [SerializeField] private BiomeData[] biomeDataLevels;

        private TransitionManager _transitionManager;
        private InputReader _inputReader;
        private Biome _currentBiome;
        private int _currentStep;
        private int _biomeIndex;

        protected override void Awake()
        {
            base.Awake();

            _inputReader = new InputReader();
            ServiceLocator.Instance.TryToRegisterService(_inputReader);
        }

        private void Start()
        {
            LoadBiomeData();
            _transitionManager = ServiceLocator.Instance.GetService<TransitionManager>();
            _inputReader.DisableMainInput();
        }

        private void LoadBiomeData()
        {
            _currentBiome = biomeDataLevels[_biomeIndex].Biome;
            _currentStep = biomeDataLevels[_biomeIndex].Steps;
        }

        public void GoToLobby()
        {
            DisableMainInput();
            _transitionManager.LoadLobbyScene();
        }

        public void StartGame()
        {
            LoadBiomeData();
            LoadBiomeScene();
        }

        public void GoToNextBiome()
        {
            _biomeIndex++;

            if (_biomeIndex >= biomeDataLevels.Length)
                throw new Exception("Biome index out of range!");

            LoadBiomeData();
            LoadBiomeScene();
        }

        public void GoToNextLevel()
        {
            _currentStep--;

            if (_currentStep == 0)
                LoadBossBiomeScene();
            else
                LoadBiomeScene();
        }

        private void LoadBiomeScene() =>
            _transitionManager.LoadBiomeScene(_currentBiome);

        private void LoadBossBiomeScene() =>
            _transitionManager.LoadBossBiomeScene(_currentBiome);

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