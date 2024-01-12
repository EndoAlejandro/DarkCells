using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalChase : IState
    {
        public AnimationState AnimationState { get; }
        public bool CanTransitionToSelf { get; }
        public void Tick()
        {
            throw new System.NotImplementedException();
        }

        public void FixedTick()
        {
            throw new System.NotImplementedException();
        }

        public void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}