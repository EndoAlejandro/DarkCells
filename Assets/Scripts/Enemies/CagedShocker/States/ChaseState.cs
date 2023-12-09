using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class ChaseState : IState
    {
        public override string ToString() => "Chase";
        public AnimationState Animation => AnimationState.Ground;

        private readonly CagedShocker _cagedShocker;
        private readonly EnemyAttack _attack;
        private readonly Collider2D _collider;

        private Player _player;

        private int _targetDirection;

        private WallResult _wallResult;

        private bool CanWalk => (_cagedShocker.LeftFoot && _cagedShocker.FacingLeft) ||
                                (_cagedShocker.RightFoot && !_cagedShocker.FacingLeft);

        public bool CanTransitionToSelf => false;
        public bool AttackAvailable { get; private set; }

        public ChaseState(CagedShocker cagedShocker, EnemyAttack attack, Collider2D collider)
        {
            _cagedShocker = cagedShocker;
            _attack = attack;
            _collider = collider;
        }

        public void Tick()
        {
            if (_cagedShocker.Player == null) return;
            _cagedShocker.SeekPlayer();

            var isPlayerVisible = _cagedShocker.IsPlayerVisible(_player);
            var horizontalDistance = PlayerHorizontalDistance();

            AttackAvailable = isPlayerVisible && _attack.IsPlayerInRange();

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

            if (!_wallResult.FacingWall) _cagedShocker.Move(CanWalk ? _targetDirection : 0);
        }

        public void OnEnter()
        {
            AttackAvailable = false;
            _player = _cagedShocker.Player;
        }

        public void OnExit() => _player = null;
    }
}