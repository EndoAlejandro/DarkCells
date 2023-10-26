using DarkHavoc.PlayerComponents;

namespace DarkHavoc.StateMachineComponents
{
    public interface IState
    {
        AnimationState Animation { get; }
        bool CanTransitionToSelf { get; }
        /// <summary>
        /// Called each Update.
        /// </summary>
        void Tick();

        /// <summary>
        /// Called each FixedUpdate.
        /// </summary>
        void FixedTick();

        /// <summary>
        /// Called when the stateMachine enter this state.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Called when the stateMachine exit this state.
        /// </summary>
        void OnExit();
    }
}