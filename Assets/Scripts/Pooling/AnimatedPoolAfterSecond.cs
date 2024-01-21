using UnityEngine;

namespace DarkHavoc.Pooling
{
    public class AnimatedPoolAfterSecond : PooledMonoBehaviour
    {
        private static readonly int Animate = Animator.StringToHash("Animate");
        private Animator _animator;

        protected void OnEnable()
        {
            _animator ??= GetComponent<Animator>();
            if (_animator == null) return;
            _animator.SetTrigger(Animate);
        }

        private void AnimationEnded() => ReturnToPool();
    }
}