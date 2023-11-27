using DarkHavoc.ImpulseComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.PlayerComponents
{
    public class WallSlideState : IState
    {
        public override string ToString() => "Wall Slide";
        public AnimationState Animation => AnimationState.WallSlide;
        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        private readonly Player _player;
        private readonly InputReader _input;

        private WallResult _wallResult;

        public WallSlideState(Player player, InputReader input)
        {
            _player = player;
            _input = input;
        }

        public void Tick()
        {
            if (Ended) return;

            _wallResult =
                EntityVision.CheckWallCollision(_player.Collider, _player.Stats.WallDetection, _player.FacingLeft);

            if (!_wallResult.FacingWall) Ended = true;

            if (_player.HasBufferedJump)
            {
                // _player.AddImpulse(_player.Stats.WallJumpImpulse);
                _player.WallJump();
                _player.ApplyVelocity();
                Ended = true;
            }
        }

        public void FixedTick() => _player.Move(_input.Movement.x);

        public void OnEnter()
        {
            Ended = false;
            _player.SetWallSliding(true);
        }

        public void OnExit() => _player.SetWallSliding(false);
    }
}