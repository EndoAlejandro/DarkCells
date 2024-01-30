using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class SideToSidePatrolState : IState
    {
        public override string ToString() => "Patrol";
        public AnimationState AnimationState => AnimationState.Ground;

        private readonly Enemy _enemy;
        private readonly Collider2D _collider;

        private int _direction;

        private WallResult _wallResult;

        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        private bool IsLeftFootOnAir => !_enemy.LeftFoot && _enemy.RightFoot;
        private bool IsRightFootOnAir => _enemy.LeftFoot && !_enemy.RightFoot;

        public SideToSidePatrolState(Enemy enemy, Collider2D collider)
        {
            _enemy = enemy;
            _collider = collider;
        }

        public void Tick()
        {
            _enemy.SeekPlayer();

            _direction = _enemy.FacingLeft ? -1 : 1;

            if (IsLeftFootOnAir) Ended = _enemy.FacingLeft;
            else if (IsRightFootOnAir) Ended = !_enemy.FacingLeft;

            if (_wallResult.FacingWall) Ended = true;
        }

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision(_collider, _enemy.Stats.WallDetection,
                _enemy.FacingLeft);
            _enemy.Move(_direction);
        }

        public void OnEnter()
        {
            _wallResult = EntityVision.CheckWallCollision(_collider, _enemy.Stats.WallDetection,
                _enemy.FacingLeft);

            if (_wallResult.FacingWall) _enemy.SetFacingLeft(!_enemy.FacingLeft);
            else if (IsLeftFootOnAir) _enemy.SetFacingLeft(false);
            else if (IsRightFootOnAir) _enemy.SetFacingLeft(true);

            Ended = false;
        }

        public void OnExit() => Ended = false;
    }
}