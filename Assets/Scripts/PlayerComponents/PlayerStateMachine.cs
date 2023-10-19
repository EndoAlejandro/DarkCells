using PlayerComponents.States;
using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerStateMachine : FiniteStateBehaviour
    {
        private Player _player;
        private Rigidbody2D _rigidbody;
        private InputReader _input;

        protected override void References()
        {
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<InputReader>();
        }

        protected override void StateMachine()
        {
            var ground = new GroundState(_player, _rigidbody, _input);
            var air = new AirState(_player, _rigidbody, _input);
            var roll = new RollState(_player, _rigidbody, _input);
            var attack = new AttackState(_player, _rigidbody, _input);
            var block = new BlockState(_player, _rigidbody, _input);
            // TODO: Heavy attack maybe from combo.

            stateMachine.SetState(ground);

            // Locomotion
            stateMachine.AddTransition(ground, air, () => !_player.Grounded);
            stateMachine.AddTransition(air, ground, () => _player.Grounded);

            // Roll.
            stateMachine.AddTransition(ground, roll, () => _input.Roll);
            stateMachine.AddTransition(air, roll, () => _input.Roll);
            stateMachine.AddTransition(roll, ground, () => roll.Ended && _player.Grounded);
            stateMachine.AddTransition(roll, air, () => roll.Ended && !_player.Grounded);

            // To Attack
            var toAttackStates = new IState[] { ground, air, roll };
            stateMachine.AddManyTransitions(toAttackStates, attack, () => _player.HasBufferedLightAttack);
            stateMachine.AddTransition(attack, attack,
                () => _player.HasBufferedLightAttack && attack.CanCombo);
            stateMachine.AddTransition(attack, ground, () => attack.Ended);

            // Block.
            var toBlockStates = new IState[] { ground, air, roll };
            stateMachine.AddManyTransitions(toBlockStates, block, () => _input.BlockHold);
            stateMachine.AddTransition(block, ground, () => !_input.BlockHold && block.Ended);
            stateMachine.AddTransition(block, air, () => _player.HasBufferedJump && block.Ended);
        }
    }
}