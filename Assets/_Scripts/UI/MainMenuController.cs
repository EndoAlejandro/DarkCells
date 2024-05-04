using System.Collections;
using DarkHavoc.Managers;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject container;

        [SerializeField] private Button startGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;
        
        private GameManager _gameManager;
        private TransitionManager _transitionManager;

        private void Awake() => container.SetActive(false);

        private void Start()
        {
            _gameManager = ServiceLocator.GetService<GameManager>();
            _transitionManager = ServiceLocator.GetService<TransitionManager>();
            
            startGameButton?.onClick.AddListener(() => StartCoroutine(StartButtonPressedAsync()));
            settingsButton?.onClick.AddListener(SettingsButtonPressed);
            creditsButton?.onClick.AddListener(CreditsButtonPressed);
            exitButton?.onClick.AddListener(ExitButtonPressed);

            StartCoroutine(StartMenuAsync());
        }

        private IEnumerator StartMenuAsync()
        {
            yield return null;
            yield return _transitionManager.SetMenuPanel(true);
            container.SetActive(true);
        }

        private IEnumerator StartButtonPressedAsync()
        {
            container.SetActive(false);
            yield return _transitionManager.SetMenuPanel(false);
            _gameManager.UnpauseGame();
        }

        private void SettingsButtonPressed()
        {
        }

        private void CreditsButtonPressed()
        {
        }

        private void ExitButtonPressed()
        {
        }
    }
}