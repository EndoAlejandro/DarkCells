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
            var idleState = new IdleState(_player, _rigidbody, _input);
            var jumpState = new JumpState(_player, _rigidbody, _input);

            stateMachine.SetState(idleState);

            stateMachine.AddTransition(idleState, jumpState, () => _input.Jump);
            stateMachine.AddTransition(jumpState, idleState, () => _input.Movement.y > 0.5f);
        }
    }
}