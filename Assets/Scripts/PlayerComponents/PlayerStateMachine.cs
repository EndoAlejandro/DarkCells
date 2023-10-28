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
            var ground = new GroundState(_player, _rigidbody, _input);
            var air = new AirState(_player, _rigidbody, _input);
            var roll = new RollState(_player, _rigidbody, _input);
            var crouch = new CrouchState(_player, _rigidbody);
            var attack = new AttackState(_player, _rigidbody, _animation, AnimationState.LightAttack);
            var block = new BlockState(_player, _rigidbody, _input);
            var parry = new ParryState(_player);
            var parryAttack = new AttackState(_player, _rigidbody, _animation, AnimationState.ParryAttack);
            // TODO: Heavy attack maybe from combo.

            // Initial State.
            stateMachine.SetState(ground);

            // Locomotion.
            stateMachine.AddTransition(ground, air, () => !_player.Grounded);
            stateMachine.AddTransition(air, ground, () => _player.Grounded);

            // Roll.
            var toRollStates = new IState[] { ground, air, crouch, parry };
            stateMachine.AddManyTransitions(toRollStates, roll, () => _input.Roll);
            stateMachine.AddTransition(roll, ground,
                () => roll.Ended && _player.Grounded && !_player.CheckCeilingCollision());
            stateMachine.AddTransition(roll, air, () => roll.Ended && !_player.Grounded);
            stateMachine.AddTransition(roll, crouch,
                () => roll.Ended && _player.Grounded && _player.CheckCeilingCollision());

            // Crouch.
            stateMachine.AddTransition(crouch, ground, () => !_player.CheckCeilingCollision());

            // Attack.
            var toAttackStates = new IState[] { ground, air, roll };
            stateMachine.AddManyTransitions(toAttackStates, attack, () => _player.HasBufferedLightAttack);
            stateMachine.AddTransition(attack, attack,
                () => _player.HasBufferedLightAttack && attack.CanCombo);
            stateMachine.AddTransition(attack, ground, () => attack.Ended);
            stateMachine.AddTransition(attack, air, () => attack.Ended && !_player.Grounded);

            // Block.
            var toBlockStates = new IState[] { ground, air, roll };
            stateMachine.AddManyTransitions(toBlockStates, block, () => _input.BlockHold);

            stateMachine.AddTransition(block, ground, () => !_input.BlockHold && block.Ended);
            stateMachine.AddTransition(block, air, () => _player.HasBufferedJump && block.Ended);
            stateMachine.AddTransition(block, parry, () => block.ParryAvailable);

            // Parry.
            stateMachine.AddTransition(parry, parry, () => parry.ParryAvailable);

            stateMachine.AddTransition(parry, parryAttack, () => _player.HasBufferedLightAttack);
            stateMachine.AddTransition(parry, block, () => parry.Ended && _input.BlockHold);
            stateMachine.AddTransition(parry, ground, () => parry.Ended && !_input.BlockHold);

            stateMachine.AddTransition(parryAttack, ground, () => parryAttack.Ended);
        }
    }
}