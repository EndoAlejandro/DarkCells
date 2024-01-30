using DarkHavoc.Enemies.CagedShocker.States;
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
            var chase = new ColossalChaseState(_colossal, 3.5f);
            var death = new BossDeathState(_colossal);

            // Attacks.
            var rangedAttack = new ColossalRangedAttackState(
                _colossal, _animation, _colossal.RangedHitBox, 1.5f);
            var meleeAttack = new ColossalMeleeAttackState(
                _colossal, _animation, _colossal.MeleeHitBox, 2f);
            var buffAttack = new ColossalBuffAttackState(
                _colossal, _animation, _colossal.BuffHitBox, 2f);
            var boomerangAttack = new ColossalBoomerangAttackState(
                _colossal, _animation);

            stateMachine.SetState(initialDelay);

            stateMachine.AddTransition(initialDelay, awake, () => initialDelay.Ended);
            stateMachine.AddTransition(awake, idle, () => awake.Ended);
            stateMachine.AddTransition(idle, chase, () => idle.Ended);

            stateMachine.AddTransition(chase, boomerangAttack, () => BoomerangTransition(chase));
            stateMachine.AddTransition(chase, buffAttack, () => _colossal.CanBuff /*chase.BuffAvailable*/);
            stateMachine.AddTransition(chase, meleeAttack, () => chase.MeleeAvailable);
            stateMachine.AddTransition(chase, rangedAttack, () => chase.Ended && chase.RangedAvailable);

            stateMachine.AddTransition(boomerangAttack, idle, () => boomerangAttack.Ended);
            stateMachine.AddTransition(buffAttack, idle, () => buffAttack.Ended);
            stateMachine.AddTransition(meleeAttack, idle, () => meleeAttack.Ended);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);
            
            stateMachine.AddAnyTransition(death, ()=> !_colossal.IsAlive);
        }

        private bool BoomerangTransition(ColossalChaseState chase)
        {
            bool result = chase.Ended && !chase.MeleeAvailable && !chase.RangedAvailable && !chase.BuffAvailable &&
                          chase.BoomerangAvailable;

            if (result) chase.BoomerangCooldown();
            return result;
        }
    }
}