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
            var groundState = new GroundState(_player, _rigidbody, _input);
            var airState = new AirState(_player, _rigidbody, _input);

            stateMachine.SetState(groundState);

            stateMachine.AddTransition(groundState, airState, () => !_player.IsGrounded);
            stateMachine.AddTransition(airState, groundState, () => _player.IsGrounded);
        }
    }
}