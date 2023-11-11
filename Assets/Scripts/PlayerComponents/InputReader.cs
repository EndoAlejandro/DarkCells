using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    public class InputReader : MonoBehaviour
    {
        private SwordMaster _input;

        // Input Pressed.
        public Vector2 Movement => _input != null ? _input.Main.Movement.ReadValue<Vector2>() : Vector2.zero;
        public bool JumpHold => _input != null && _input.Main.Jump.IsPressed();
        public bool BlockHold => _input != null && _input.Main.Block.IsPressed();

        // On Input Down.
        public bool Jump => _input != null && _input.Main.Jump.WasPerformedThisFrame();
        public bool Roll => _input != null && _input.Main.Roll.WasPerformedThisFrame();
        public bool Attack => _input != null && _input.Main.Attack.WasPerformedThisFrame();
        public bool Block => _input != null && _input.Main.Block.WasPerformedThisFrame();

        private void Awake()
        {
            _input = new SwordMaster();
            // _input.Enable();
            GameManager.OnSetInputEnabled += GameManagerOnSetInputEnabled;
        }

        private void GameManagerOnSetInputEnabled(bool value)
        {
            if (value) _input.Enable();
            else _input.Disable();
        }
    }
}