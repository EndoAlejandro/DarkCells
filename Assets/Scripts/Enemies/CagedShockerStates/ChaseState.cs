using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShockerStates
{
    public class ChaseState : IState
    {
        public override string ToString() => "Chase";
        public AnimationState Animation => AnimationState.Ground;

        private readonly CagedShocker _cagedShocker;
        private readonly Rigidbody2D _rigidbody;
        private readonly Collider2D _collider;

        private Player _player;

        private Vector2 _targetVelocity;

        private int _targetDirection;

        // private bool _facingWall;
        private bool _leftFoot;
        private bool _rightFoot;
        private WallResult _wallResult;

        private bool CanWalk => (_leftFoot && _cagedShocker.FacingLeft) || (_rightFoot && !_cagedShocker.FacingLeft);

        public bool CanTransitionToSelf => false;
        public bool AttackAvailable { get; private set; }

        public ChaseState(CagedShocker cagedShocker, Rigidbody2D rigidbody, Collider2D collider)
        {
            _cagedShocker = cagedShocker;
            _rigidbody = rigidbody;
            _collider = collider;
        }

        public void Tick()
        {
            if (_cagedShocker.Player == null) return;
            _cagedShocker.SeekPlayer();

            var isPlayerVisible = _cagedShocker.IsPlayerVisible(_player);
            var horizontalDistance = PlayerHorizontalDistance();

            if (isPlayerVisible)
            {
                var result = Physics2D.OverlapBox(_cagedShocker.HitBox.bounds.center, _cagedShocker.HitBox.bounds.size, 0f,
                    _cagedShocker.Stats.AttackLayer);

                if (result != null && result.TryGetComponent(out Player player))
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
            _wallResult = EntityVision.CheckWallCollision(_collider, _cagedShocker.Stats.WallDetection,
                _cagedShocker.FacingLeft);
            // _cagedShocker.CheckWallCollisions(out _facingWall);
            _cagedShocker.CheckGrounded(out _leftFoot, out _rightFoot);

            if (!_wallResult.FacingWall)
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