using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.PlayerComponents
{
    public class LedgeGrabState : IState
    {
        public AnimationState Animation => AnimationState.LedgeGrab;
        public bool CanTransitionToSelf => false;

        private readonly Player _player;
        private readonly InputReader _input;

        public bool Ended { get; private set; }

        public LedgeGrabState(Player player, InputReader input)
        {
            _player = player;
            _input = input;
        }

        public void Tick()
        {
            if (Ended) return;

            _player.Move(_player.Direction);

            if (_input.Movement.x == -_player.Direction || _input.Movement.y < 0)
                Ended = true;

            if (_player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
                Ended = true;
            }
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            Ended = false;
            _player.LedgeGrab(true);
        }

        public void OnExit()
        {
            _player.LedgeGrab(false);
        }
    }
}