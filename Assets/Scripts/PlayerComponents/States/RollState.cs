using DarkHavoc.ImpulseComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class RollState : IState
    {
        public override string ToString() => "Roll";
        public AnimationState AnimationState => AnimationState.Roll;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly ImpulseAction _rollAction;

        private float _timer;

        public bool Ended { get; private set; }
        public bool CanTransitionToSelf => false;

        public RollState(Player player, Rigidbody2D rigidbody, InputReader input, ImpulseAction rollAction)
        {
            _player = player;
            _rigidbody = rigidbody;
            _rollAction = rollAction;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            if (_player.HasBufferedJump && !_player.CheckCeilingCollision(_player.Stats.CrouchCeilingDistance))
            {
                _player.Jump();
                _player.ApplyVelocity();
                Ended = true;
            }

            if (_timer <= 0f) Ended = true;
        }

        public void FixedTick()
        {
            _player.AddImpulse(_rollAction);
        }

        public void OnEnter()
        {
            Ended = false;
            _timer = _rollAction.Time;
            _player.Roll();
            //_player.AddImpulse(_rollAction);
            _player.SetPlayerCollider(false);
        }

        public void OnExit()
        {
            Ended = false;
            _player.AddImpulse(new ImpulseAction());
            _player.SetPlayerCollider(true);
        }
    }
}