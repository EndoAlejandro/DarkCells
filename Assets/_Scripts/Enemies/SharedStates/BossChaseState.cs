using System;
using System.Threading;
using System.Threading.Tasks;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public abstract class BossChaseState : IState
    {
        public override string ToString() => "Chase";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => true;
        public bool Ended => _timer <= 0f;

        protected readonly Boss boss;
        protected readonly float _stoppingDistance;
        protected readonly float _chaseTime;

        protected Player _player;
        protected int _direction;
        protected float _timer;
        protected float _distance;

        // Turning.
        protected readonly float _turnAwait;
        protected CancellationTokenSource _cts;
        protected bool _changingDirection;
        
        protected bool Stop => _distance <= _stoppingDistance;

        protected BossChaseState(Boss boss, float stoppingDistance)
        {
            this.boss = boss;
            _stoppingDistance = stoppingDistance;

            _chaseTime = .2f;
        }

        public virtual void Tick() => _timer -= Time.deltaTime;

        public virtual async void FixedTick()
        {
            SetDirection();

            boss.Move(Stop || _changingDirection ? 0 : _direction);
            
            if (_changingDirection) return;
            if (_direction < 0 && !boss.FacingLeft) await ModifyFacingDirection(true);
            else if (_direction > 0 && boss.FacingLeft) await ModifyFacingDirection(false);
        }

        protected void SetDirection()
        {
            Vector2 direction = (_player.transform.position - boss.transform.position);
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

        public virtual void OnExit()
        {
            boss.Move(0);
            _cts.Cancel();
        }

        protected async Task ModifyFacingDirection(bool facingLeft)
        {
            _changingDirection = true;
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(_turnAwait), _cts.Token);
                boss.SetFacingLeft(facingLeft);
            }
            catch (Exception _)
            {
                // Ignore.
            }

            _changingDirection = false;
        }
    }
}