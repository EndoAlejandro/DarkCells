using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    public class ImputReader
    {
        private SwordMaster _input;

        public bool IsActive { get; private set; }

        // Input Pressed.
        public Vector2 Movement => _input != null ? _input.Main.Movement.ReadValue<Vector2>() : Vector2.zero;
        public bool JumpHold => _input != null && _input.Main.Jump.IsPressed();
        public bool BlockHold => _input != null && _input.Main.Block.IsPressed();

        // On Input Down.
        public bool Jump => _input != null && _input.Main.Jump.WasPerformedThisFrame();
        public bool Roll => _input != null && _input.Main.Roll.WasPerformedThisFrame();
        public bool Attack => _input != null && _input.Main.Attack.WasPerformedThisFrame();
        public bool Block => _input != null && _input.Main.Block.WasPerformedThisFrame();

        public bool Pause => _input != null && _input.Pause.Pause.WasPerformedThisFrame();

        public ImputReader()
        {
            _input = new SwordMaster();
            SetPauseEnable(true);
        }

        /*private void GameManagerOnSetInputEnabled(bool value)
        {
            if (value) EnableMainInput();
            else DisableMainInput();
        }*/

        public void SetPauseEnable(bool state)
        {
            if (state) _input.Pause.Enable();
            else _input.Pause.Disable();
        }

        public void EnableMainInput()
        {
            IsActive = true;
            _input.Main.Enable();
        }

        public void DisableMainInput()
        {
            IsActive = false;
            _input.Main.Disable();
        }
    }
}