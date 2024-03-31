using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.Colossal.States
{
    public class ColossalBoomerangAttackState : IState
    {
        public override string ToString() => "Boomerang Attack";
        public AnimationState AnimationState => AnimationState.BoomerangAttack;
        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        private readonly Colossal _colossal;
        private readonly ColossalAnimation _animation;

        private ColossalBoomerangArms _boomerangArms;

        public ColossalBoomerangAttackState(Colossal colossal, ColossalAnimation animation)
        {
            _colossal = colossal;
            _animation = animation;
        }

        public void Tick()
        {
        }

        public void FixedTick() => _colossal.Move(0);

        public void OnEnter()
        {
            Ended = false;
            _animation.OnAttackPerformed += AnimationOnBoomerangAttack;
        }

        private void AnimationOnBoomerangAttack()
        {
            _boomerangArms =
                Object.Instantiate(_colossal.BoomerangArms, _colossal.transform.position, Quaternion.identity);
            _boomerangArms.Setup(_colossal, 10f);

            _boomerangArms.OnDestroyed += BoomerangArmsOnDestroyed;
        }

        private void BoomerangArmsOnDestroyed(bool playerHit) => Ended = true;

        public void OnExit()
        {
            _animation.OnAttackPerformed -= AnimationOnBoomerangAttack;
            if (_boomerangArms != null) _boomerangArms.OnDestroyed -= BoomerangArmsOnDestroyed;
        }
    }
}