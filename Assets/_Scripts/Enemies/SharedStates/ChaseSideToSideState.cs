using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.SharedStates
{
    public class ChaseSideToSideState : MultiHitboxState, IState
    {
        public override string ToString() => "Chase";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool IsPlayerVisible { get; private set; }

        private bool CanWalk => (_enemy.LeftFoot && _enemy.FacingLeft) ||
                                (_enemy.RightFoot && !_enemy.FacingLeft);

        private readonly Enemy _enemy;
        private readonly Collider2D _collider;

        private Player _player;
        private WallResult _wallResult;

        private int _targetDirection;

        public ChaseSideToSideState(Enemy enemy, Collider2D collider, EnemyHitBox firstHitBox = null,
            EnemyHitBox secondHitBox = null, EnemyHitBox thirdHitBox = null) :
            base(firstHitBox, secondHitBox, thirdHitBox)
        {
            _enemy = enemy;
            _collider = collider;
        }

        public void Tick()
        {
            if (_enemy.Player == null) return;
            _enemy.SeekPlayer();

            IsPlayerVisible = _enemy.IsPlayerVisible(_player);
            var horizontalDistance = PlayerHorizontalDistance();

            _targetDirection = Mathf.Abs(horizontalDistance) > _enemy.Stats.StoppingDistance
                ? (int)Mathf.Sign(horizontalDistance)
                : 0;

            if (_enemy.FacingLeft && horizontalDistance > 0f) _enemy.SetFacingLeft(false);
            if (!_enemy.FacingLeft && horizontalDistance < 0f) _enemy.SetFacingLeft(true);

            _enemy.KeepTrackPlayer();

            HitBoxCheck();
        }

        private float PlayerHorizontalDistance() =>
            _player.transform.position.x - _enemy.transform.position.x;

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision(_collider, _enemy.Stats.WallDetection,
                _enemy.FacingLeft);

            if (!_wallResult.FacingWall) _enemy.Move(CanWalk ? _targetDirection : 0);
            else _enemy.Move(0);
        }

        public void OnEnter()
        {
            IsPlayerVisible = false;
            _player = _enemy.Player;
        }

        public void OnExit() => _player = null;
    }
}