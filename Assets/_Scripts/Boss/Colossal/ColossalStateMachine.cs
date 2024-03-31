using DarkHavoc.Boss.Colossal.States;
using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Boss.Colossal
{
    public class ColossalStateMachine : FiniteStateBehaviour
    {
        private DarkHavoc.Boss.Colossal.Colossal _colossal;
        private ColossalAnimation _animation;
        private Player _player;

        protected override void References()
        {
            _colossal = GetComponent<DarkHavoc.Boss.Colossal.Colossal>();
            _animation = GetComponentInChildren<ColossalAnimation>();
        }

        protected override void StateMachine()
        {
            var initialDelay = new AnimationOnlyState(5f, AnimationState.None);
            var awake = new AnimationOnlyState(1.1f, AnimationState.Awake);
            var idle = new BossIdle(_colossal);
            var chase = new ColossalChaseState(_colossal, 3.5f);
            var death = new BossDeathState(_colossal);

            // Attacks.
            var rangedAttack = new BossAttackState(_colossal, _animation, _colossal.RangedHitBox,
                AnimationState.RangedAttack, 3.75f);
            var meleeAttack = new BossMeleeAttackState(_colossal, _animation, _colossal.MeleeHitBox, 3.75f);
            var buffAttack = new ColossalBuffState(_colossal, _animation, _colossal.BuffHitBox, 3.75f);
            var boomerangAttack = new ColossalBoomerangAttackState(_colossal, _animation);

            stateMachine.SetState(initialDelay);

            stateMachine.AddTransition(initialDelay, awake, () => initialDelay.Ended);
            stateMachine.AddTransition(awake, idle, () => awake.Ended);
            stateMachine.AddTransition(idle, chase, () => idle.Ended);

            stateMachine.AddTransition(chase, boomerangAttack, () => BoomerangTransition(chase));
            stateMachine.AddTransition(chase, buffAttack, () => _colossal.CanBuff);
            stateMachine.AddTransition(chase, meleeAttack, () => chase.MeleeAvailable);
            stateMachine.AddTransition(chase, rangedAttack, () => chase.Ended && chase.RangedAvailable);

            stateMachine.AddTransition(boomerangAttack, idle, () => boomerangAttack.Ended);
            stateMachine.AddTransition(buffAttack, idle, () => buffAttack.Ended);
            stateMachine.AddTransition(meleeAttack, idle, () => meleeAttack.Ended);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_colossal.IsAlive);
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