using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private static readonly int HidePanel = Animator.StringToHash("HidePanel");

        [SerializeField] private GameObject container;

        [SerializeField] private Button startGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            startGameButton?.onClick.AddListener(StartButtonPressed);
            settingsButton?.onClick.AddListener(SettingsButtonPressed);
            creditsButton?.onClick.AddListener(CreditsButtonPressed);
            exitButton?.onClick.AddListener(ExitButtonPressed);
        }

        private void StartButtonPressed()
        {
            StartCoroutine(StartButtonPressedAsync());
            /*_animator.SetTrigger(HidePanel);
            GameManager.Instance.StartGame();*/
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

        private IEnumerator StartButtonPressedAsync()
        {
            _animator.SetTrigger(HidePanel);
            yield return new WaitForSeconds(2);
            GameManager.Instance.StartGame();
        }
    }
}