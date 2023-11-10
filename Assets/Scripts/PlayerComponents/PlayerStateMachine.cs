using DarkHavoc.PlayerComponents.States;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    public class PlayerStateMachine : FiniteStateBehaviour
    {
        private Player _player;
        private PlayerAnimation _animation;
        private Rigidbody2D _rigidbody;
        private InputReader _input;

        protected override void References()
        {
            _animation = GetComponentInChildren<PlayerAnimation>();
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<InputReader>();
        }

        protected override void StateMachine()
        {
            // Grounded States
            var ground = new GroundState(_player, _rigidbody, _input);
            var roll = new RollState(_player, _rigidbody, _input, _player.Stats.RollAction);
            var crouch = new CrouchState(_player, _rigidbody);

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
            // TODO: Heavy attack maybe from combo.

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
            var toRollStates = new IState[] { ground, air, crouch, parry };
            stateMachine.AddManyTransitions(toRollStates, roll, () => _player.HasBufferedRoll);
            stateMachine.AddTransition(roll, ground,
                () => roll.Ended && _player.Grounded && !_player.CheckCeilingCollision());
            stateMachine.AddTransition(roll, air, () => roll.Ended && !_player.Grounded);
            stateMachine.AddTransition(roll, crouch,
                () => roll.Ended && _player.Grounded && _player.CheckCeilingCollision());
            stateMachine.AddTransition(roll, lightAttack,
                () => _player.HasBufferedAttack && !_player.CheckCeilingCollision());

            // Crouch.
            stateMachine.AddTransition(crouch, ground, () => !_player.CheckCeilingCollision());

            // Air Attack.
            var toAirAttack = new IState[] { air, lightAttack };
            stateMachine.AddManyTransitions(toAirAttack, airAttack,
                () => _player.HasBufferedAttack && !_player.Grounded);
            stateMachine.AddTransition(airAttack, air, () => airAttack.Ended);

            // Heavy Attack
            stateMachine.AddTransition(ground, heavyAttack,
                () => _player.HasBufferedAttack && _player.CanPerformHeavyAttack);
            stateMachine.AddTransition(lightAttack, heavyAttack,
                () => _player.HasBufferedAttack && lightAttack.CanCombo && _player.CanPerformHeavyAttack);
            stateMachine.AddTransition(heavyAttack, ground, () => heavyAttack.Ended);

            // Light Attack.
            stateMachine.AddTransition(ground, lightAttack, () => _player.HasBufferedAttack);
            stateMachine.AddTransition(lightAttack, lightAttack,
                () => _player.HasBufferedAttack && lightAttack.CanCombo);

            stateMachine.AddTransition(lightAttack, ground, () => lightAttack.Ended);

            // Block.
            var toBlockStates = new IState[] { ground, air, roll };
            stateMachine.AddManyTransitions(toBlockStates, block, () => _player.HasBufferedBlock);

            stateMachine.AddTransition(block, ground, () => block.Ended);
            // stateMachine.AddTransition(block, air, () => block.Ended);
            stateMachine.AddTransition(block, parry, () => block.ParryAvailable);

            // Parry.
            stateMachine.AddTransition(parry, parry, () => parry.ParryAvailable);

            // stateMachine.AddTransition(parry, airAttack, () => _player.HasBufferedAttack);
            stateMachine.AddTransition(parry, block, () => parry.Ended && _input.BlockHold);
            stateMachine.AddTransition(parry, ground, () => parry.Ended && !_input.BlockHold);
        }
    }
}