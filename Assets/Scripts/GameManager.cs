using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using UnityEngine.SceneManagement;

namespace DarkHavoc
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action<bool> OnSetInputEnabled;
        private void ActivateInput() => OnSetInputEnabled?.Invoke(true);
        private void DeactivateInput() => OnSetInputEnabled?.Invoke(false);

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            DontDestroyOnLoad(gameObject);
            DeactivateInput();
        }

        public void StartGame() => ActivateInput();

        public void LoadLobbyScene() => StartCoroutine(LoadLobbySceneAsync());

        private IEnumerator LoadLobbySceneAsync()
        {
            yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
            DeactivateInput();
        }
    }
}