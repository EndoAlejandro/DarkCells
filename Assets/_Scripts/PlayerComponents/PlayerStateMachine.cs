using DarkHavoc.PlayerComponents.States;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.PlayerComponents
{
    public class PlayerStateMachine : FiniteStateBehaviour
    {
        private Player _player;
        private PlayerAnimation _animation;
        private Rigidbody2D _rigidbody;
        private InputReader _input;

        private bool CanEndRoll =>
            _player != null && _player.CheckCeilingCollision(_player.Stats.CrouchCeilingDistance);

        protected override void References()
        {
            _animation = GetComponentInChildren<PlayerAnimation>();
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = ServiceLocator.GetService<InputReader>();
        }

        protected override void StateMachine()
        {
            // Grounded States
            var ground = new GroundState(_player, _input);
            var roll = new RollState(_player, _rigidbody, _input, _player.Stats.RollAction);
            var death = new DeathState(_player, _rigidbody);

            // Air States
            var air = new AirState(_player, _rigidbody, _input);
            var ledgeGrab = new LedgeGrabState(_player, _input);
            var wallSlide = new WallSlideState(_player, _input);

            // Defend States
            var block = new BlockState(_player, _rigidbody, _input);
            var parry = new ParryState(_player, _player.Stats.ParryAction);

            // Attack States
            var lightAttack = new AttackState(_player, _rigidbody, _input, _animation, AnimationState.LightAttack,
                _player.Stats.LightAttackAction);
            var airAttack = new AttackState(_player, _rigidbody, _input, _animation, AnimationState.ParryAttack,
                _player.Stats.LightAttackAction);
            var heavyAttack = new AttackState(_player, _rigidbody, _input, _animation, AnimationState.HeavyAttack,
                _player.Stats.HeavyAttackAction);

            var sitDown = new SitState(_player, true);
            var sitUp = new SitState(_player, false);

            // Initial State.
            stateMachine.SetState(ground);

            // Locomotion.
            stateMachine.AddTransition(ground, air, () => !_player.Grounded);
            stateMachine.AddTransition(air, ground, () => _player.Grounded);

            // Wall Slide.
            stateMachine.AddTransition(air, ledgeGrab, () => air.FacingLedge);
            stateMachine.AddTransition(wallSlide, ground, () => _player.Grounded);
            stateMachine.AddTransition(wallSlide, air, () => wallSlide.Ended);

            // Ledge Grab.
            stateMachine.AddTransition(air, wallSlide, () => air.FacingWall);
            stateMachine.AddTransition(ledgeGrab, air, () => ledgeGrab.Ended);

            // Roll.
            var toRollStates = new IState[] { ground, air, parry };
            stateMachine.AddManyTransitions(toRollStates, roll, () => _player.HasBufferedRoll);
            stateMachine.AddTransition(roll, ground, () => roll.Ended && _player.Grounded && !CanEndRoll);
            stateMachine.AddTransition(roll, air, () => roll.Ended && !_player.Grounded && !CanEndRoll);
            stateMachine.AddTransition(roll, lightAttack, () => _player.HasBufferedAttack && !CanEndRoll);

            // Air Attack.
            /*var toAirAttack = new IState[] { air, lightAttack };
            stateMachine.AddManyTransitions(toAirAttack, airAttack,
                () => _player.HasBufferedAttack && !_player.Grounded);
            stateMachine.AddTransition(airAttack, air, () => airAttack.Ended);*/

            // Heavy Attack
            stateMachine.AddTransition(ground, heavyAttack,
                () => _player.HasBufferedAttack && _player.CanPerformHeavyAttack);
            stateMachine.AddTransition(lightAttack, heavyAttack,
                () => _player.HasBufferedAttack && lightAttack.CanCombo &&
                      _player is { CanPerformHeavyAttack: true, Grounded: true });
            stateMachine.AddTransition(heavyAttack, ground, () => heavyAttack.Ended);

            // Light Attack.
            var toLightAttack = new IState[] { ground, air };
            stateMachine.AddManyTransitions(toLightAttack, lightAttack, () => _player.HasBufferedAttack);
            stateMachine.AddTransition(lightAttack, lightAttack,
                () => _player.HasBufferedAttack && lightAttack.CanCombo);

            stateMachine.AddTransition(lightAttack, ground, () => lightAttack.Ended && _player.Grounded);
            stateMachine.AddTransition(lightAttack, air, () => lightAttack.Ended && !_player.Grounded);

            // Block.
            var toBlockStates = new IState[] { ground, air, roll };
            stateMachine.AddManyTransitions(toBlockStates, block, () => _player.HasBufferedBlock);

            stateMachine.AddTransition(block, ground, () => block.Ended);
            stateMachine.AddTransition(block, parry, () => block.ParryAvailable);

            // Parry.
            stateMachine.AddTransition(parry, ground, () => parry.Ended);

            // Sit.
            stateMachine.AddAnyTransition(sitDown, () => !_input.IsActive);
            stateMachine.AddTransition(sitDown, sitUp, () => _input.IsActive);
            stateMachine.AddTransition(sitUp, ground, () => sitUp.Ended);

            stateMachine.AddAnyTransition(death, () => !_player.IsAlive);
        }
    }
}