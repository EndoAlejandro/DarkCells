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

            stateMachine.SetState(ground);

            // Locomotion
            stateMachine.AddTransition(ground, air, () => !_player.IsGrounded);
            stateMachine.AddTransition(air, ground, () => _player.IsGrounded);

            // Roll.
            stateMachine.AddTransition(ground, roll, () => _input.Roll);
            stateMachine.AddTransition(air, roll, () => _input.Roll);
            stateMachine.AddTransition(roll, ground, () => roll.Ended);
        }
    }
}