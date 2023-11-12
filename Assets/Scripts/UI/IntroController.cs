using UnityEngine;
using UnityEngine.UI;

namespace DarkHavoc.UI
{
    public class IntroController : MonoBehaviour
    {
        private Button _button;
        private void Awake() => _button = GetComponentInChildren<Button>();
        private void Start() => _button.onClick.AddListener(OnButtonPressed);
        private void OnButtonPressed() => GameManager.Instance.LoadLobbyScene();
    }
}