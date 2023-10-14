using PlayerComponents.States;
using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        private Animator _animator;
        private SpriteRenderer _renderer;

        private InputReader _inputReader;
        private PlayerStateMachine _playerStateMachine;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();

            _inputReader = GetComponentInParent<InputReader>();
            _playerStateMachine = GetComponentInParent<PlayerStateMachine>();
        }

        private void Start()
        {
            _playerStateMachine.OnEntityStateChanged += PlayerStateMachineOnEntityStateChanged;
        }

        private void Update()
        {
            switch (_playerStateMachine.CurrentStateType)
            {
                case GroundState idle:
                    FlipCheck();
                    HorizontalFloat();
                    break;
            }
        }

        private void FlipCheck()
        {
            if (_inputReader.Movement.x == 0) return;
            _renderer.flipX = _inputReader.Movement.x < 0;
        }

        private void HorizontalFloat() => _animator.SetFloat(Horizontal, Mathf.Abs(_inputReader.Movement.x));

        private void PlayerStateMachineOnEntityStateChanged(IState state)
        {
            switch (state)
            {
                case GroundState idle:
                    break;
            }
        }
    }
}