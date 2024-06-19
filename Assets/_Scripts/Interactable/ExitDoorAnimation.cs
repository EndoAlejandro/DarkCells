using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Interactable
{
    [RequireComponent(typeof(Animator))]
    public class ExitDoorAnimation : MonoBehaviour
    {
        private static readonly int Activate = Animator.StringToHash("Activate");
        private Animator _animator;
        private void Awake() => _animator = GetComponent<Animator>();
        public void ActivateAnimation()
        {
            _animator.SetTrigger(Activate);
            ServiceLocator.GetService<FxManager>()?.PlayFx(BossFx.ExitDoor, transform.position);
        }
    }
}
