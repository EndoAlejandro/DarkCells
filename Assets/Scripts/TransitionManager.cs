using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc
{
    public class TransitionManager : Singleton<TransitionManager>
    {
        private static readonly int Out = Animator.StringToHash("Out");
        private static readonly int In = Animator.StringToHash("In");
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");

        private bool _transitionInProgress;
        private Animator _animator;

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            DontDestroyOnLoad(gameObject);
            _animator = GetComponent<Animator>();
        }

        public IEnumerator SetTransitionPanel(bool state)
        {
            yield return new WaitUntil(() => _transitionInProgress != state);
            ResetTriggers();
            _animator.SetTrigger(state ? In : Out);
            yield return new WaitForSeconds(1f);
            _transitionInProgress = state;
        }

        public IEnumerator SetMenuPanel(bool state, Action callback = null)
        {
            yield return new WaitUntil(() => _transitionInProgress != state);
            ResetTriggers();
            _animator.SetTrigger(state ? Show : Hide);
            yield return new WaitForSeconds(.25f);
            callback?.Invoke();
            _transitionInProgress = state;
        }

        private void ResetTriggers()
        {
            _animator.ResetTrigger(Out);
            _animator.ResetTrigger(In);
            _animator.ResetTrigger(Show);
            _animator.ResetTrigger(Hide);
        }
    }
}