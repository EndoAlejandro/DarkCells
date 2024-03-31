using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyAttackFxState : EnemyAttackState
    {
        private readonly FxType[] _fxs;
        private FxManager _fxManager;
        private int _fxIndex;

        public EnemyAttackFxState(Enemy enemy, EnemyHitBox hitbox, EnemyAnimation animation,
            FxType[] fxs, bool isUnstoppable = false,
            AnimationState animationState = AnimationState.LightAttack) : base(enemy, hitbox, animation, isUnstoppable,
            animationState) => _fxs = fxs;

        public override void OnEnter()
        {
            base.OnEnter();
            _fxIndex = 0;
            animation.OnAttackFx += AnimationOnAttackFx;
        }

        private void AnimationOnAttackFx()
        {
            _fxManager ??= ServiceLocator.GetService<FxManager>();
            _fxManager.PlayFx(_fxs[_fxIndex], enemy.transform.position, flipX: enemy.FacingLeft);
            _fxIndex = (_fxIndex + 1) % _fxs.Length;
        }

        public override void OnExit()
        {
            base.OnExit();
            animation.OnAttackFx -= AnimationOnAttackFx;
        }
    }
}