using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.PlayerComponents.States
{
    public class GroundState : IState
    {
        public override string ToString() => "Grounded";
        public AnimationState AnimationState => AnimationState.Ground;

        private readonly Player _player;
        private readonly InputReader _input;

        public bool CanTransitionToSelf => false;

        public GroundState(Player player, InputReader input)
        {
            _player = player;
            _input = input;
        }

        public void Tick()
        {
            if (_player.CheckCeilingCollision(_player.Stats.CeilingDistance)) return;

            if (_player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick() => _player.Move(_input.Movement.x);

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}