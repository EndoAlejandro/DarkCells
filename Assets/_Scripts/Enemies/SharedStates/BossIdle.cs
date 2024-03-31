using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class BossIdle : IState
    {
        public override string ToString() => "Idle";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;
        
        private readonly Boss _boss;
        private readonly float _duration;

        private Player _player;
        private float _timer;
        private bool _firstTime;

        public BossIdle(Boss boss)
        {
            _boss = boss;
            _duration = _boss.Stats.IdleTime;
            _firstTime = true;
        }

        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _boss.Move(0);

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
            _boss.Setup();
        }

        public void OnExit() => _timer = _duration;
    }
}