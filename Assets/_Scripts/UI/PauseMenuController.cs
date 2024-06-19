using System;
using System.Collections;
using DarkHavoc.Managers;
using DarkHavoc.PlayerComponents;
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
        private GameManager _gameManager;
        private InputReader _inputReader;

        private void Start()
        {
            container.SetActive(false);

            _inputReader = ServiceLocator.GetService<InputReader>();
            _transitionManager = ServiceLocator.GetService<TransitionManager>();
            _gameManager = ServiceLocator.GetService<GameManager>();

            _gameManager.SetPauseInput(true);
            GameManager.OnGamePauseChanged += GameManagerOnGamePauseChanged;

            resumeButton.onClick.AddListener(ResumeButtonPressed);
            exitButton.onClick.AddListener(ExitButtonPressed);
        }

        private void Update()
        {
            if (_gameManager.Paused || !_inputReader.Pause) return;
            _gameManager.PauseGame();
        }

        private void GameManagerOnGamePauseChanged(bool isPaused) =>
            StartCoroutine(GameManagerOnGamePauseChangedAsync(isPaused));

        private IEnumerator GameManagerOnGamePauseChangedAsync(bool isPaused)
        {
            if (!isPaused)
            {
                container.SetActive(false);
                SetButtonsState(false);
            }

            yield return null;
            yield return _transitionManager.SetMenuPanel(isPaused);

            if (isPaused)
            {
                container.SetActive(true);
                SetButtonsState(true);
            }
        }

        private void ResumeButtonPressed()
        {
            _gameManager.UnpauseGame();
        }

        private void ExitButtonPressed()
        {
            _gameManager.UnpauseGame();
            _gameManager.GoToLobby();
        }

        private void SetButtonsState(bool state)
        {
            resumeButton.enabled = state;
            settingsButton.enabled = state;
            exitButton.enabled = state;
        }

        private void OnDestroy() => GameManager.OnGamePauseChanged -= GameManagerOnGamePauseChanged;
    }
}