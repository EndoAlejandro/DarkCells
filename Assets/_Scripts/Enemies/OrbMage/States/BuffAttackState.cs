using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.OrbMage.States
{
    public class BuffAttackState : IState
    {
        private readonly Enemy _enemy;
        private readonly EnemyAnimation _animation;
        private readonly float _buffDuration;
        public override string ToString() => "Buff";
        public AnimationState AnimationState => AnimationState.BuffAttack;
        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        public BuffAttackState(Enemy enemy, EnemyAnimation animation, float buffDuration)
        {
            _enemy = enemy;
            _animation = animation;
            _buffDuration = buffDuration;
        }

        public void Tick()
        {
        }

        public void FixedTick() => _enemy.Move(0);

        public void OnEnter()
        {
            Ended = false;
            _animation.OnAttackPerformed += AnimationOnAttackPerformed;
            _animation.OnAttackEnded += AnimationOnAttackEnded;
        }

        private void AnimationOnAttackEnded() => Ended = true;
        private void AnimationOnAttackPerformed() => _enemy.ActivateBuff(_buffDuration);

        public void OnExit()
        {
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
            _animation.OnAttackEnded -= AnimationOnAttackEnded;
        }
    }
}