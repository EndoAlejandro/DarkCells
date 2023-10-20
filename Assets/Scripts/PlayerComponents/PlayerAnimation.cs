using System;
using PlayerComponents.States;
using Unity.VisualScripting;
using UnityEngine;
using IState = StateMachineComponents.IState;

namespace PlayerComponents
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        private Animator _animator;
        private SpriteRenderer _renderer;

        private InputReader _inputReader;
        private PlayerStateMachine _playerStateMachine;
        private Player _player;

        private IState _previousState;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();

            _player = GetComponentInParent<Player>();
            _inputReader = GetComponentInParent<InputReader>();
            _playerStateMachine = GetComponentInParent<PlayerStateMachine>();
        }

        private void OnEnable() => _playerStateMachine.OnEntityStateChanged += PlayerStateMachineOnEntityStateChanged;
        private void OnDisable() => _playerStateMachine.OnEntityStateChanged -= PlayerStateMachineOnEntityStateChanged;

        private void Update()
        {
            switch (_playerStateMachine.CurrentStateType)
            {
                case GroundState idleState:
                    FlipCheck();
                    HorizontalFloat();
                    break;
                case AirState airState:
                    FlipCheck();
                    HorizontalFloat();
                    VerticalFloat();
                    break;
                case CrouchState crouchState:
                case BlockState blockState:
                    FlipCheck();
                    break;
                case RollState rollState:
                    break;
                case AttackState lightAttackState:
                    break;
            }

            _renderer.flipX = _player.FacingLeft;
        }

        private void FlipCheck()
        {
            if (_inputReader.Movement.x == 0) return;
            _player.SetFacingLeft(_inputReader.Movement.x < 0);
        }

        private void HorizontalFloat() =>
            _animator.SetFloat(Horizontal, Mathf.Abs(_player.GetNormalizedHorizontal()));

        private void VerticalFloat() =>
            _animator.SetFloat(Vertical, Mathf.Clamp(_player.GetNormalizedVertical(), -1, 1));

        private void PlayerStateMachineOnEntityStateChanged(IState state)
        {
            if (_previousState != null) _animator.ResetTrigger(_previousState.ToString());

            _animator.SetTrigger(state.ToString());
            _previousState = state;
        }
    }
}