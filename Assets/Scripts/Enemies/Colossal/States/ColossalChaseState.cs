using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

using DarkHavoc.CustomUtils;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.Colossal.States
{
    public class ColossalChaseState : IState
    {
        public override string ToString() => "Chase";

        private readonly Colossal _colossal;
        private readonly EnemyHitBox _rangedHitBox;
        private readonly EnemyHitBox _meleeHitBox;
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
        private bool Stop => _distance <= _stoppingDistance;
        public bool RangedAvailable { get; private set; }
        public bool MeleeAvailable { get; private set; }

        public ColossalChaseState(Colossal colossal, EnemyHitBox rangedHitBox, EnemyHitBox meleeHitBox,
            float stoppingDistance)
        {
            _colossal = colossal;
            _rangedHitBox = rangedHitBox;
            _meleeHitBox = meleeHitBox;
            _stoppingDistance = stoppingDistance;

            _chaseTime = .2f;
            _turnAwait = Constants.TurnAwait;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
            SetDirection();

            _colossal.Move(Stop || _changingDirection ? 0 : _direction);

            RangedAvailable = _rangedHitBox.IsPlayerInRange();
            MeleeAvailable = _meleeHitBox.IsPlayerInRange();

            if (_changingDirection) return;
            if (_direction < 0 && !_colossal.FacingLeft) ModifyFacingDirection(true);
            else if (_direction > 0 && _colossal.FacingLeft) ModifyFacingDirection(false);
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
            catch (Exception e)
            {
                // Ignore.
            }

            _changingDirection = false;
        }
    }
}