using DarkHavoc.Enemies.Colossal.States;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using DarkHavoc.UI;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalStateMachine : FiniteStateBehaviour
    {
        private Colossal _colossal;
        private ColossalAnimation _animation;
        private Player _player;

        protected override void References()
        {
            _colossal = GetComponent<Colossal>();
            _animation = GetComponentInChildren<ColossalAnimation>();
        }

        protected override void StateMachine()
        {
            var initialDelay = new AnimationOnlyState(5f, AnimationState.None);
            var awake = new AnimationOnlyState(1.1f, AnimationState.Awake);
            var idle = new ColossalIdle(_colossal, 1f);
            var chase = new ColossalChaseState(
                _colossal, _colossal.RangedHitBox, _colossal.MeleeHitBox, 3.5f);
            var rangedAttack = new ColossalRangedAttackState(
                _colossal, _animation, _colossal.RangedHitBox, 2f);
            var meleeAttack = new ColossalMeleeAttackState(_colossal, _animation, _colossal.MeleeHitBox, 2f);

            stateMachine.SetState(initialDelay);

            stateMachine.AddTransition(initialDelay, awake, () => initialDelay.Ended);
            stateMachine.AddTransition(awake, idle, () => awake.Ended);
            stateMachine.AddTransition(idle, chase, () => idle.Ended);
            /*stateMachine.AddTransition(chase, idle, () =>
                chase.Stop && !chase.RangedAvailable && !chase.MeleeAvailable);*/

            stateMachine.AddTransition(chase, meleeAttack, () => chase.MeleeAvailable);
            stateMachine.AddTransition(chase, rangedAttack, () => chase.Ended && chase.RangedAvailable);

            stateMachine.AddTransition(meleeAttack, idle, () => meleeAttack.Ended);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);
        }
    }
}