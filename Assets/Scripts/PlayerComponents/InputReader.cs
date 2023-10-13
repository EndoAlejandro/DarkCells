using UnityEngine;

namespace PlayerComponents
{
    public class InputReader : MonoBehaviour
    {
        private SwordMaster _input;

        public Vector2 Movement => _input != null ? _input.Main.Movement.ReadValue<Vector2>() : Vector2.zero;
        public bool Jump => _input != null && _input.Main.Jump.WasPerformedThisFrame();

        private void Awake()
        {
            _input = new SwordMaster();
            _input.Enable();
        }
    }
}