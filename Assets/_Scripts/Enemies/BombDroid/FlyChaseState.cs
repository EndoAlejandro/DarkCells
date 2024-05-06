using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.BombDroid
{
    public class FlyChaseState : MultiHitboxState, IState
    {
        public override string ToString() => "Chase";
        public AnimationState AnimationState => AnimationState.Air;
        public bool CanTransitionToSelf => false;

        private readonly BombDroid _bombDroid;
        private readonly Collider2D _collider;
        private readonly EntityPathfinding _pathfinding;
        private readonly EnemyHitBox _hitBox;

        private WallResult _wallResult;
        private float _targetHeight;
        private int _horizontalDirection;

        public FlyChaseState(BombDroid bombDroid, Collider2D collider, EntityPathfinding pathfinding,
            EnemyHitBox hitBox) : base(firstHitBox: hitBox)
        {
            _bombDroid = bombDroid;
            _collider = collider;
            _pathfinding = pathfinding;
            _hitBox = hitBox;
        }

        public void Tick()
        {
            if (_horizontalDirection < 0 && !_bombDroid.FacingLeft) _bombDroid.SetFacingLeft(true);
            else if (_horizontalDirection > 0 && _bombDroid.FacingLeft) _bombDroid.SetFacingLeft(false);

            HitBoxCheck();
        }

        private bool _playerVisible;
        private Vector3 _playerDirection;

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision
                (_collider, _bombDroid.Stats.WallDetection, _bombDroid.FacingLeft);

            var topCheck = Physics2D.Raycast(_bombDroid.transform.position,
                Vector2.up, 1f, _bombDroid.A);
            var downCheck = Physics2D.Raycast(_bombDroid.transform.position,
                Vector2.down, 3f);

            if(_bombDroid.Player == null) return;
            _playerDirection = _bombDroid.Player.transform.position - _bombDroid.transform.position;
            _playerVisible = _bombDroid.IsPlayerVisible(_bombDroid.Player);
            _bombDroid.SetVisibility(_playerVisible);

            var target = Mathf.Sign(_playerVisible ? _playerDirection.y : _pathfinding.Direction.y);

            // if (topCheck) target = -1f;
            if (downCheck) target = 1f;

            _bombDroid.VerticalMove(target);

            PathfindingMovement();
        }

        private void PathfindingMovement()
        {
            var a = _playerVisible ? _playerDirection.x : _pathfinding.Direction.x;
            _horizontalDirection = _playerVisible ? (int)_playerDirection.x : (int)Mathf.Sign(_pathfinding.Direction.x);

            if (Mathf.Abs(_playerDirection.x) > _bombDroid.Stats.StoppingDistance)
                _bombDroid.Move(_horizontalDirection);
            else
                _bombDroid.Move(0);
        }

        public void OnEnter()
        {
            if (_bombDroid.Player != null) _pathfinding.StartFindPath(_bombDroid.Player.transform, 2f);
        }

        public void OnExit()
        {
            _bombDroid.ResetVelocity();
        }
    }
}