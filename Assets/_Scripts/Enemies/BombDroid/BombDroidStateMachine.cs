using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.BombDroid
{
    [RequireComponent(typeof(BombDroid))]
    public class BombDroidStateMachine : FiniteStateBehaviour
    {
        private BombDroid _bombDroid;
        private Collider2D _collider;
        private EntityPathfinding _pathfinding;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _bombDroid = GetComponent<BombDroid>();
            _collider = GetComponent<Collider2D>();
            _pathfinding = GetComponent<EntityPathfinding>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_bombDroid);
            var chase = new FlyChaseState(_bombDroid, _collider, _pathfinding, _bombDroid.HitBox);
            var telegraph = new TelegraphState(_bombDroid, _bombDroid.HitBox, .5f);
            var attack = new EnemyBombAttackState(_bombDroid, _bombDroid.HitBox as ProjectileEnemyHitBox, _animation, true);
            var death = new EnemyDeathState(_bombDroid);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, chase, () => _bombDroid.Player != null);
            stateMachine.AddTransition(chase, idle, () => _bombDroid.Player == null);

            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddAnyTransition(death, () => !_bombDroid.IsAlive);
        }
    }
}