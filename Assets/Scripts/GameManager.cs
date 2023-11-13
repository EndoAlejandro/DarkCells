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
        public static event Action OnTransitionStarted;
        public static event Action OnTransitionEnded;

        public Player PlayerPrefab => playerPrefab;

        [SerializeField] private Player playerPrefab;

        private const string TransitionScreen = "TransitionScreen";
        private IEnumerator _currentTransition;

        private void ActivateInput() => OnSetInputEnabled?.Invoke(true);
        private void DeactivateInput() => OnSetInputEnabled?.Invoke(false);

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            DontDestroyOnLoad(gameObject);
            // DeactivateInput();
        }

        public void EnablePlayerMovement() => ActivateInput();

        public void LoadLobbyScene()
        {
            StartCoroutine(LoadLobbySceneAsync());
        }

        private IEnumerator LoadLobbySceneAsync()
        {
            yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
            DeactivateInput();
        }

        public void LoadBiomeScene(Biome biome) => StartCoroutine(LoadBiomeSceneAsync(biome));

        private IEnumerator LoadBiomeSceneAsync(Biome biome)
        {
            DeactivateInput();
            yield return SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Single);
            OnTransitionStarted?.Invoke();
            yield return new WaitForSeconds(1f);
            yield return SceneManager.LoadSceneAsync(biome.ToString(), LoadSceneMode.Additive);
            yield return new WaitForSeconds(1f);
            OnTransitionEnded?.Invoke();
            ActivateInput();
        }
    }
}