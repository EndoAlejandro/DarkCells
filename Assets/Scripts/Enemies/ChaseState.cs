using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies
{
    public class ChaseState : IState
    {
        public override string ToString() => "Chase";
        public AnimationState Animation => AnimationState.Ground;

        private readonly CagedShocker _cagedShocker;
        private readonly Rigidbody2D _rigidbody;

        private Player _player;

        private Vector2 _targetVelocity;
        private int _targetDirection;
        private bool _facingWall;
        private bool _leftFoot;
        private bool _rightFoot;

        private bool CanWalk => (_leftFoot && _cagedShocker.FacingLeft) || (_rightFoot && !_cagedShocker.FacingLeft);

        public bool CanTransitionToSelf => false;
        public bool AttackAvailable { get; private set; }

        public ChaseState(CagedShocker cagedShocker, Rigidbody2D rigidbody)
        {
            _cagedShocker = cagedShocker;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
            if (_cagedShocker.Player == null) return;
            _cagedShocker.SeekPlayer();

            var isPlayerVisible = _cagedShocker.IsPlayerVisible(_player);
            var horizontalDistance = PlayerHorizontalDistance();
            if (isPlayerVisible)
            {
                if (Mathf.Abs(horizontalDistance) < _cagedShocker.AttackOffset.localPosition.x)
                    AttackAvailable = true;
            }

            _targetDirection =
                Mathf.Abs(horizontalDistance) > _cagedShocker.Stats.ChaseStoppingDistance
                    ? (int)Mathf.Sign(horizontalDistance)
                    : 0;

            if (_cagedShocker.FacingLeft && horizontalDistance > 0f) _cagedShocker.SetFacingLeft(false);
            if (!_cagedShocker.FacingLeft && horizontalDistance < 0f) _cagedShocker.SetFacingLeft(true);

            _cagedShocker.KeepTrackPlayer();
        }

        private float PlayerHorizontalDistance() =>
            _player.transform.position.x - _cagedShocker.transform.position.x;

        public void FixedTick()
        {
            _cagedShocker.CheckWallCollisions(out _facingWall);
            _cagedShocker.CheckGrounded(out _leftFoot, out _rightFoot);

            if (!_facingWall)
                _cagedShocker.Move(ref _targetVelocity, CanWalk ? _targetDirection : 0);

            _cagedShocker.CustomGravity(ref _targetVelocity);
            _cagedShocker.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            AttackAvailable = false;
            _player = _cagedShocker.Player;
        }

        public void OnExit() => _player = null;
    }
}