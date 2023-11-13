using UnityEngine;

namespace DarkHavoc
{
    public class TransitionAnimationController : MonoBehaviour
    {
        private static readonly int Out = Animator.StringToHash("Out");
        private static readonly int In = Animator.StringToHash("In");
        private Animator _animator;
        private void Awake() => _animator = GetComponent<Animator>();
        private void Start()
        {
            GameManager.OnTransitionStarted += GameManagerOnTransitionStarted;
            GameManager.OnTransitionEnded += GameManagerOnTransitionEnded;
        }

        private void GameManagerOnTransitionEnded()
        {
            ResetTriggers();
            _animator.SetTrigger(Out);
        }

        private void GameManagerOnTransitionStarted()
        {
            ResetTriggers();
            _animator.SetTrigger(In);
        }

        private void ResetTriggers()
        {
            _animator.ResetTrigger(Out);
            _animator.ResetTrigger(In);
        }

        private void OnDestroy()
        {
            GameManager.OnTransitionStarted -= GameManagerOnTransitionStarted;
            GameManager.OnTransitionEnded -= GameManagerOnTransitionEnded;
        }
    }
}