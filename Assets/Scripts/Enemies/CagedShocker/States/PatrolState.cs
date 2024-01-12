using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class PatrolState : IState
    {
        public override string ToString() => "Patrol";
        public AnimationState AnimationState => AnimationState.Ground;

        private readonly CagedShocker _cagedShocker;
        private readonly Collider2D _collider;

        private int _direction;

        private WallResult _wallResult;

        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        private bool IsLeftFootOnAir => !_cagedShocker.LeftFoot && _cagedShocker.RightFoot;
        private bool IsRightFootOnAir => _cagedShocker.LeftFoot && !_cagedShocker.RightFoot;

        public PatrolState(CagedShocker cagedShocker, Collider2D collider)
        {
            _cagedShocker = cagedShocker;
            _collider = collider;
        }

        public void Tick()
        {
            _cagedShocker.SeekPlayer();

            _direction = _cagedShocker.FacingLeft ? -1 : 1;

            if (IsLeftFootOnAir) Ended = _cagedShocker.FacingLeft;
            else if (IsRightFootOnAir) Ended = !_cagedShocker.FacingLeft;

            if (_wallResult.FacingWall) Ended = true;
        }

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision(_collider, _cagedShocker.Stats.WallDetection,
                _cagedShocker.FacingLeft);
            _cagedShocker.Move(_direction);
        }

        public void OnEnter()
        {
            _wallResult = EntityVision.CheckWallCollision(_collider, _cagedShocker.Stats.WallDetection,
                _cagedShocker.FacingLeft);

            if (_wallResult.FacingWall) _cagedShocker.SetFacingLeft(!_cagedShocker.FacingLeft);
            else if (IsLeftFootOnAir) _cagedShocker.SetFacingLeft(false);
            else if (IsRightFootOnAir) _cagedShocker.SetFacingLeft(true);

            Ended = false;
        }

        public void OnExit() => Ended = false;
    }
}