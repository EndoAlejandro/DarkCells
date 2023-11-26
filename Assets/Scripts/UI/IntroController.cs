using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class IntroController : MonoBehaviour
    {
        private Button _button;
        private TransitionManager _transitionManager;

        private void Awake() => _button = GetComponentInChildren<Button>();

        private void Start()
        {
            _transitionManager = ServiceLocator.Instance.GetService<TransitionManager>();
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnButtonPressed() => _transitionManager.LoadLobbyScene();
    }
}