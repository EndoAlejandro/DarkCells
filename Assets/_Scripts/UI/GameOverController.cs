using DarkHavoc.Managers;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class GameOverController : MonoBehaviour
    {
        private Button _button;
        private GameManager _gameManager;
        
        private void Awake() => _button = GetComponentInChildren<Button>();

        private void Start()
        {
            _gameManager = ServiceLocator.GetService<GameManager>();
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            _button.enabled = false;
            _gameManager?.GoToLobby();
        }
    }
}
