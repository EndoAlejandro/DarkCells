using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class DeadState : IState
    {
        public override string ToString() => "Dead";
        public AnimationState Animation => AnimationState.Death;
        public bool CanTransitionToSelf => false;

        private readonly Enemies.CagedShocker.CagedShocker _cagedShocker;

        private Vector2 _targetVelocity;

        public DeadState(Enemies.CagedShocker.CagedShocker cagedShocker) => _cagedShocker = cagedShocker;

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        public void OnEnter() => _cagedShocker.Death();

        public void OnExit()
        {
        }
    }
}