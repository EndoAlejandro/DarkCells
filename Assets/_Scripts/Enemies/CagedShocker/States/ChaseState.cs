using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class ChaseState : IState
    {
        public override string ToString() => "Chase";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool AttackAvailable { get; private set; }

        private bool CanWalk => (_enemy.LeftFoot && _enemy.FacingLeft) ||
                                (_enemy.RightFoot && !_enemy.FacingLeft);

        private readonly Enemy _enemy;
        private readonly EnemyHitBox _hitBox;
        private readonly Collider2D _collider;

        private Player _player;
        private WallResult _wallResult;

        private int _targetDirection;

        public ChaseState(Enemy enemy, EnemyHitBox hitBox, Collider2D collider)
        {
            _enemy = enemy;
            _hitBox = hitBox;
            _collider = collider;
        }

        public void Tick()
        {
            if (_enemy.Player == null) return;
            _enemy.SeekPlayer();

            var isPlayerVisible = _enemy.IsPlayerVisible(_player);
            var horizontalDistance = PlayerHorizontalDistance();

            AttackAvailable = isPlayerVisible && _hitBox.IsPlayerInRange();

            _targetDirection = Mathf.Abs(horizontalDistance) > _enemy.Stats.StoppingDistance
                ? (int)Mathf.Sign(horizontalDistance)
                : 0;

            if (_enemy.FacingLeft && horizontalDistance > 0f) _enemy.SetFacingLeft(false);
            if (!_enemy.FacingLeft && horizontalDistance < 0f) _enemy.SetFacingLeft(true);

            _enemy.KeepTrackPlayer();
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
            AttackAvailable = false;
            _player = _enemy.Player;
        }

        public void OnExit() => _player = null;
    }
}