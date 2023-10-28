using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class ParryState : IState
    {
        public AnimationState Animation => AnimationState.Parry;
        public override string ToString() => "Parry";
        public bool CanTransitionToSelf => true;
        public bool Ended => _timer <= 0f;
        public bool ParryAvailable { get; private set; }

        private readonly Player _player;

        private Vector2 _targetVelocity;
        private float _timer;

        public ParryState(Player player)
        {
            _player = player;
            _player.OnAttackBlocked += PlayerOnAttackBlocked;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, 0f, Time.deltaTime * 5f);
        }

        public void FixedTick()
        {
            // _player.Move(ref _targetVelocity, 0f);
            _player.CheckCollisions(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
        }

        private void PlayerOnAttackBlocked() => ParryAvailable = true;

        public void OnEnter()
        {
            _player.Parry(ref _targetVelocity);

            ParryAvailable = false;
            _timer = _player.Stats.ParryTime;
            _player.TryToBlockDamage += PlayerOnTryToBlockDamage;
        }

        private bool PlayerOnTryToBlockDamage(Vector2 damageSource)
        {
            float difference = damageSource.x - _player.transform.position.x;
            ParryAvailable = (difference < 0 && _player.FacingLeft) || (difference > 0 && !_player.FacingLeft);
            return ParryAvailable;
        }

        public void OnExit()
        {
            ParryAvailable = false;
            _player.TryToBlockDamage -= PlayerOnTryToBlockDamage;
            _timer = 0f;
        }
    }
}