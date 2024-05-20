using DarkHavoc.Boss.Colossal.States;
using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.Fx;
using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Boss.Colossal
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
            var idle = new BossIdle(_colossal);
            var chase = new ColossalChaseState(_colossal, 3.5f);
            var death = new BossDeathState(_colossal);

            var rangedTelegraph = new BossTelegraphState(_colossal, FxType.ColossalTelegraph, .5f);
            var meleeTelegraph = new BossTelegraphState(_colossal, FxType.ColossalTelegraph, .5f);
            var buffTelegraph = new BossTelegraphState(_colossal, FxType.ColossalTelegraph, .5f);
            var boomerangTelegraph = new BossTelegraphState(_colossal, FxType.ColossalTelegraph, .5f);
            
            // Attacks.
            var rangedAttack = new ColossalRangedAttackState(_colossal, _animation, _colossal.RangedHitBox,
                AnimationState.RangedAttack, 3.75f);
            var meleeAttack = new BossMeleeAttackState(_colossal, _animation, _colossal.MeleeHitBox, 3.75f);
            var buffAttack = new ColossalBuffState(_colossal, _animation, _colossal.BuffHitBox, 3.75f);
            var boomerangAttack = new ColossalBoomerangAttackState(_colossal, _animation);

            stateMachine.SetState(initialDelay);

            // Locomotion
            stateMachine.AddTransition(initialDelay, awake, () => initialDelay.Ended);
            stateMachine.AddTransition(awake, idle, () => awake.Ended);
            stateMachine.AddTransition(idle, chase, () => idle.Ended);

            // Boomerang Attack
            stateMachine.AddTransition(chase, boomerangTelegraph, () => BoomerangTransition(chase));
            stateMachine.AddTransition(boomerangTelegraph, boomerangAttack, () => boomerangTelegraph.Ended);
            
            // Buff Attack
            stateMachine.AddTransition(chase, buffTelegraph, () => _colossal.CanBuff);
            stateMachine.AddTransition(buffTelegraph, buffAttack, () => buffTelegraph.Ended);
            
            // Melee Attack
            stateMachine.AddTransition(chase, meleeTelegraph, () => chase.MeleeAvailable);
            stateMachine.AddTransition(meleeTelegraph, meleeAttack, () => meleeTelegraph.Ended);
            
            // Ranged Attack
            stateMachine.AddTransition(chase, rangedTelegraph, () => chase.Ended && chase.RangedAvailable);
            stateMachine.AddTransition(rangedTelegraph, rangedAttack, () => rangedTelegraph.Ended);

            // Attacks to Idle
            stateMachine.AddTransition(boomerangAttack, idle, () => boomerangAttack.Ended);
            stateMachine.AddTransition(buffAttack, idle, () => buffAttack.Ended);
            stateMachine.AddTransition(meleeAttack, idle, () => meleeAttack.Ended);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);

            // Death
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