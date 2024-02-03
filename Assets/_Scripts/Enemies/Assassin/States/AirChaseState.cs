using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Assassin.States
{
    public class AirChaseState : IState
    {
        public override string ToString() => "Air";
        public AnimationState AnimationState => AnimationState.Air;
        public bool CanTransitionToSelf => false;

        private readonly Assassin _assassin;
        private readonly Player _player;
        private readonly Collider2D _collider;
        private readonly EntityPathfinding _pathfinding;

        private int _horizontalDirection;

        public AirChaseState(Assassin assassin, Player player, Collider2D collider, EntityPathfinding pathfinding)
        {
            _assassin = assassin;
            _player = player;
            _collider = collider;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
            _horizontalDirection = (int)Mathf.Sign(_pathfinding.Direction.x);
            _assassin.Move(Mathf.Abs(_pathfinding.Direction.x) > _assassin.Stats.StoppingDistance * .5f
                ? _horizontalDirection
                : 0);
        }

        public void OnEnter() => _pathfinding.StartFindPath(_player.transform);

        public void OnExit()
        {
        }
    }
}