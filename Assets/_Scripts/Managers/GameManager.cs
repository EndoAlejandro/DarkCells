using System;
using System.Collections;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Managers
{
    public class GameManager : Service<GameManager>
    {
        protected override bool DonDestroyOnLoad => true;
        public static event Action<bool> OnSetInputEnabled;
        public static event Action<bool> OnGamePauseChanged;

        public Player Player { get; private set; }
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
            ServiceLocator.TryToRegisterService(_inputReader);

            Player.OnPlayerSpawned += PlayerOnPlayerSpawned;
            Player.OnPlayerDeSpawned += PlayerOnPlayerDeSpawned;
        }

        private void PlayerOnPlayerDeSpawned(Player player) => Player = null;
        private void PlayerOnPlayerSpawned(Player player) => Player = player;

        private void Start()
        {
            LoadBiomeData();
            _transitionManager = ServiceLocator.GetService<TransitionManager>();
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

        private void GoToNextBiome()
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

            switch (_currentStep)
            {
                case >0:
                LoadBiomeScene();
                    break;
                case 0:
                LoadBossBiomeScene();
                    break;
                case <0 :
                    GoToNextBiome();
                    break;
                    
            }
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Player.OnPlayerSpawned -= PlayerOnPlayerSpawned;
            Player.OnPlayerDeSpawned -= PlayerOnPlayerDeSpawned;
        }

        public IEnumerator CreatePlayerAsync(Vector3 playerSpawnPoint)
        {
            Instantiate(PlayerPrefab, playerSpawnPoint, Quaternion.identity);
            yield return null;
            EnableMainInput();
        }
    }
}