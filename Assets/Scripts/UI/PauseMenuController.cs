using System.Collections;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject container;

        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        private TransitionManager _transitionManager;

        private void Start()
        {
            _transitionManager = ServiceLocator.Instance.GetService<TransitionManager>();
            GameManager.OnGamePauseChanged += GameManagerOnGamePauseChanged;
        }

        private void GameManagerOnGamePauseChanged(bool isPaused) =>
            StartCoroutine(GameManagerOnGamePauseChangedAsync(isPaused));

        private IEnumerator GameManagerOnGamePauseChangedAsync(bool isPaused)
        {
            yield return null;
            yield return _transitionManager.SetMenuPanel(isPaused);
            container.SetActive(isPaused);
        }

        private void OnDestroy() => GameManager.OnGamePauseChanged -= GameManagerOnGamePauseChanged;
    }
}