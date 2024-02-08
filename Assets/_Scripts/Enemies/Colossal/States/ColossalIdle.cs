using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.Colossal.States
{
    public class ColossalIdle : IState
    {
        private readonly Colossal _colossal;
        private readonly float _duration;

        private Player _player;
        private float _timer;
        private bool _firstTime;

        public override string ToString() => "Idle";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        public ColossalIdle(Colossal colossal, float duration)
        {
            _colossal = colossal;
            _duration = duration;
            _firstTime = true;
        }

        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _colossal.Move(0);

        public void OnEnter()
        {
            _player ??= ServiceLocator.GetService<GameManager>().Player;
            _timer = _duration;

            FirstTimeCheck();
        }

        private void FirstTimeCheck()
        {
            if (!_firstTime) return;
            _firstTime = false;
            _colossal.Setup();
        }

        public void OnExit() => _timer = _duration;
    }
}