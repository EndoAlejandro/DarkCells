using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedSpider
{
    public class CagedSpiderStateMachine : FiniteStateBehaviour
    {
        private CagedSpider _cagedSpider;
        private EnemyAnimation _animation;
        private Collider2D _collider;

        protected override void References()
        {
            _cagedSpider = GetComponent<CagedSpider>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_cagedSpider);
            var chase = new ChaseState(_cagedSpider, _cagedSpider.HitBox, _collider);
            var telegraph = new TelegraphState(_cagedSpider, .5f);
            var attack = new EnemyAttackState(_cagedSpider, _cagedSpider.HitBox, _animation);
            var death = new EnemyDeathState(_cagedSpider);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, chase, () => idle.Ended && _cagedSpider.Player != null);
            stateMachine.AddTransition(chase, telegraph, () => chase.AttackAvailable);
            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddAnyTransition(death, () => !_cagedSpider.IsAlive);
        }
    }
}