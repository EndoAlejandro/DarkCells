using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.PlayerComponents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkHavoc
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action<bool> OnSetInputEnabled;
        public static event Action<bool> OnGamePauseChanged; 

        public Player PlayerPrefab => playerPrefab;
        public Player Player { get; private set; }

        [SerializeField] private Player playerPrefab;

        private const string TransitionScreen = "TransitionScreen";
        private IEnumerator _currentTransition;

        private void ActivateInput() => OnSetInputEnabled?.Invoke(true);
        private void DeactivateInput() => OnSetInputEnabled?.Invoke(false);
        private void PauseGame() => OnGamePauseChanged?.Invoke(true);
        private void UnpauseGame() => OnGamePauseChanged?.Invoke(false);

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            DontDestroyOnLoad(gameObject);
        }

        public void EnablePlayerMovement() => ActivateInput();

        public void LoadLobbyScene() => StartCoroutine(LoadLobbySceneAsync());

        private IEnumerator LoadLobbySceneAsync()
        {
            yield return TransitionManager.Instance.SetTransitionPanel(true);
            yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
            DeactivateInput();
            yield return TransitionManager.Instance.SetTransitionPanel(false);
        }

        public void LoadBiomeScene(Biome biome) => StartCoroutine(LoadBiomeSceneAsync(biome));

        private IEnumerator LoadBiomeSceneAsync(Biome biome)
        {
            DeactivateInput();
            yield return TransitionManager.Instance.SetTransitionPanel(true);
            yield return SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync(biome.ToString(), LoadSceneMode.Additive);
            yield return TransitionManager.Instance.SetTransitionPanel(false);
            ActivateInput();
        }

        public void RegisterPlayer(Player player) => Player = player;
    }
}