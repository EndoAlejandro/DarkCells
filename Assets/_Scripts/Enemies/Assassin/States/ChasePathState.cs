using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Assassin.States
{
    public class ChasePathState : IState
    {
        public override string ToString() => "Chase Path";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool LightAttackAvailable { get; private set; }
        public bool SlashAttackAvailable { get; private set; }

        private readonly Assassin _assassin;
        private readonly EnemyHitBox _lightHitBox;
        private readonly EnemyHitBox _slashHitBox;
        private readonly Collider2D _collider;
        private readonly EntityPathfinding _pathfinding;

        private WallResult _wallResult;

        private int _horizontalDirection;

        public ChasePathState(Assassin assassin, EnemyHitBox lightHitBox, EnemyHitBox slashHitBox,
            Collider2D collider, EntityPathfinding pathfinding)
        {
            _assassin = assassin;
            _lightHitBox = lightHitBox;
            _slashHitBox = slashHitBox;
            _collider = collider;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
            if (_horizontalDirection < 0 && !_assassin.FacingLeft) _assassin.SetFacingLeft(true);
            else if (_horizontalDirection > 0 && _assassin.FacingLeft) _assassin.SetFacingLeft(false);

            LightAttackAvailable = _lightHitBox.IsPlayerInRange();
            SlashAttackAvailable = _slashHitBox.IsPlayerInRange();
        }

        private bool HeightJump() => _pathfinding.Direction.y > .5f;
        private bool JumpFirstCase() => _wallResult.FacingWall && HeightJump();
        private bool JumpSecondCase() => _assassin.LedgeInFront;

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision
                (_collider, _assassin.Stats.WallDetection, _assassin.FacingLeft);

            if ((JumpFirstCase() || JumpSecondCase())
                && !_wallResult.TopCheck && _assassin.Grounded)
            {
                _assassin.Jump(_pathfinding.Direction.y <= -.5f);
            }

            PathfindingMovement();
        }

        private void PathfindingMovement()
        {
            _horizontalDirection = (int)Mathf.Sign(_pathfinding.Direction.x);

            if (Mathf.Abs(_pathfinding.Direction.x) > _assassin.Stats.StoppingDistance &&
                !_wallResult.MidCheck && !_assassin.LedgeInFront)
                _assassin.Move(_horizontalDirection);
            else
                _assassin.Move(0);
        }

        public void OnEnter()
        {
            if (_assassin.Player != null) _pathfinding.StartFindPath(_assassin.Player.transform);
        }

        public void OnExit()
        {
        }
    }
}