using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using DarkHavoc.CustomUtils;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.Colossal.States
{
    public class ColossalChaseState : IState
    {
        public override string ToString() => "Chase";

        private readonly Colossal _colossal;
        private readonly float _stoppingDistance;
        private readonly float _chaseTime;

        private Player _player;
        private int _direction;
        private float _timer;
        private float _distance;

        // Turning.
        private readonly float _turnAwait;
        private CancellationTokenSource _cts;
        private bool _changingDirection;

        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => true;
        public bool Ended => _timer <= 0f;
        public bool RangedAvailable { get; private set; }
        public bool MeleeAvailable { get; private set; }
        public bool BuffAvailable { get; private set; }
        public bool BoomerangAvailable => _boomerangCdTimer <= 0f && PlayerInFront;

        private bool Stop => _distance <= _stoppingDistance;

        private bool PlayerInFront => (_colossal.FacingLeft && _direction < 0) ||
                                      (!_colossal.FacingLeft && _direction > 0);

        private float _boomerangCooldown;
        private float _boomerangCdTimer;

        public ColossalChaseState(Colossal colossal, float stoppingDistance)
        {
            _colossal = colossal;
            _stoppingDistance = stoppingDistance;

            _chaseTime = .2f;
            _turnAwait = Constants.TurnAwait;
            _boomerangCooldown = _colossal.BoomerangArms.Cooldown;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _boomerangCdTimer -= Time.deltaTime;
        }

        public async void FixedTick()
        {
            SetDirection();

            _colossal.Move(Stop || _changingDirection ? 0 : _direction);

            RangedAvailable = _colossal.RangedHitBox.IsPlayerInRange();
            MeleeAvailable = _colossal.MeleeHitBox.IsPlayerInRange();
            BuffAvailable = _colossal.BuffHitBox.IsPlayerInRange();

            if (_changingDirection) return;
            if (_direction < 0 && !_colossal.FacingLeft) await ModifyFacingDirection(true);
            else if (_direction > 0 && _colossal.FacingLeft) await ModifyFacingDirection(false);
        }

        private void SetDirection()
        {
            Vector2 direction = (_player.transform.position - _colossal.transform.position);
            _distance = Mathf.Abs(direction.x);
            _direction = (int)Mathf.Sign(direction.x);
        }

        public void OnEnter()
        {
            _changingDirection = false;
            _cts = new CancellationTokenSource();

            _player ??= ServiceLocator.GetService<GameManager>().Player;
            _timer = _chaseTime;

            SetDirection();
        }

        public void OnExit()
        {
            _colossal.Move(0);

            BuffAvailable = false;
            RangedAvailable = false;
            MeleeAvailable = false;
            _cts.Cancel();
        }

        private async Task ModifyFacingDirection(bool facingLeft)
        {
            _changingDirection = true;
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(_turnAwait), _cts.Token);
                _colossal.SetFacingLeft(facingLeft);
            }
            catch (Exception _)
            {
                // Ignore.
            }

            _changingDirection = false;
        }

        public void BoomerangCooldown()
        {
            _boomerangCdTimer = _boomerangCooldown;
        }
    }
}