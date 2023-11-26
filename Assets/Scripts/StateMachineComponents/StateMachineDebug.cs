using TMPro;
using UnityEngine;

namespace DarkHavoc.StateMachineComponents
{
    public class StateMachineDebug : MonoBehaviour
    {
        private FiniteStateBehaviour _finiteStateBehaviour;
        private TMP_Text _debugText;

        private void Awake()
        {
            _finiteStateBehaviour = GetComponentInParent<FiniteStateBehaviour>();
            _debugText = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            FiniteStateBehaviourOnEntityStateChanged(_finiteStateBehaviour.CurrentStateType);
            _finiteStateBehaviour.OnEntityStateChanged += FiniteStateBehaviourOnEntityStateChanged;
        }

        private void FiniteStateBehaviourOnEntityStateChanged(IState state) =>
            _debugText.SetText(state.ToString());

        private void OnDestroy() =>
            _finiteStateBehaviour.OnEntityStateChanged -= FiniteStateBehaviourOnEntityStateChanged;
    }
}