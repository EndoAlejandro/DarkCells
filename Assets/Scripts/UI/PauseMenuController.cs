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
    }
}