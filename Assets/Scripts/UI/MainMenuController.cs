using System.Collections;
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

        private void Awake()
        {
            container.SetActive(false);
        }

        private void Start()
        {
            startGameButton?.onClick.AddListener(() => StartCoroutine(StartButtonPressedAsync()));
            settingsButton?.onClick.AddListener(SettingsButtonPressed);
            creditsButton?.onClick.AddListener(CreditsButtonPressed);
            exitButton?.onClick.AddListener(ExitButtonPressed);

            StartCoroutine(StartMenuAsync());
        }

        private IEnumerator StartMenuAsync()
        {
            yield return null;
            yield return TransitionManager.Instance.SetMenuPanel(true);
            container.SetActive(true);
        }

        private IEnumerator StartButtonPressedAsync()
        {
            container.SetActive(false);
            yield return TransitionManager.Instance.SetMenuPanel(false);
            GameManager.Instance.EnablePlayerMovement();
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