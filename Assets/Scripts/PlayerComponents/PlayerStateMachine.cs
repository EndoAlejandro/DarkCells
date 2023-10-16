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
            var lightAttack = new LightAttackState(_player, _rigidbody);

            stateMachine.SetState(ground);

            // Locomotion
            stateMachine.AddTransition(ground, air, () => !_player.Grounded);
            stateMachine.AddTransition(air, ground, () => _player.Grounded);

            // Roll.
            stateMachine.AddTransition(ground, roll, () => _input.Roll);
            stateMachine.AddTransition(air, roll, () => _input.Roll);
            stateMachine.AddTransition(roll, ground, () => roll.Ended && _player.Grounded);
            stateMachine.AddTransition(roll, air, () => roll.Ended && !_player.Grounded);

            // Attack
            stateMachine.AddTransition(ground, lightAttack, () => _player.HasBufferedLightAttack);
            stateMachine.AddTransition(lightAttack, lightAttack,
                () => _player.HasBufferedLightAttack && lightAttack.CanCombo);

            stateMachine.AddTransition(lightAttack, ground, () => lightAttack.Ended);
        }
    }
}