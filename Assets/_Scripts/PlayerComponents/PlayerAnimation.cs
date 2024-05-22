using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.Fx;
using DarkHavoc.PlayerComponents.States;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        public static Sprite Sprite => _instance?._renderer?.sprite;
        public event Action OnAttackPerformed;
        public event Action OnComboAvailable;

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int HitValue = Shader.PropertyToID("_HitValue");

        private static PlayerAnimation _instance;

        private Animator _animator;
        private SpriteRenderer _renderer;

        private InputReader _inputReader;
        private PlayerStateMachine _playerStateMachine;
        private Player _player;

        private IEnumerator _hitAnimation;
        private IState _previousState;
        private MaterialPropertyBlock _materialPb;

        private FxManager _fxManager;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();

            _player = GetComponentInParent<Player>();
            _playerStateMachine = GetComponentInParent<PlayerStateMachine>();

            _materialPb = new MaterialPropertyBlock();
        }

        private void Start()
        {
            _inputReader = ServiceLocator.GetService<InputReader>();
        }

        private void OnEnable()
        {
            _player.OnDamageTaken += PlayerOnDamageTaken;
            _playerStateMachine.OnEntityStateChanged += PlayerStateMachineOnEntityStateChanged;
        }

        private void OnDisable()
        {
            _player.OnDamageTaken -= PlayerOnDamageTaken;
            _playerStateMachine.OnEntityStateChanged -= PlayerStateMachineOnEntityStateChanged;
        }

        private void PlayerOnDamageTaken()
        {
            if (_hitAnimation != null) StopCoroutine(_hitAnimation);
            _hitAnimation = HitAnimation();
            StartCoroutine(_hitAnimation);
        }

        private IEnumerator HitAnimation()
        {
            ServiceLocator.GetService<FxManager>()
                ?.PlayFx(PlayerFx.PlayerTakeDamage, transform.position + Vector3.up * .5f);
            _renderer.GetPropertyBlock(_materialPb);

            float timer = 0f;
            while (timer < Constants.HitAnimationDuration)
            {
                timer += Time.deltaTime;
                float hitThreshold = 1 - (timer / Constants.HitAnimationDuration);
                _materialPb.SetFloat(HitValue, hitThreshold);
                _renderer.SetPropertyBlock(_materialPb);
                yield return null;
            }

            yield return null;
            _materialPb.SetFloat(HitValue, 0f);
            _renderer.SetPropertyBlock(_materialPb);
        }


        private void Update()
        {
            switch (_playerStateMachine.CurrentStateType)
            {
                case GroundState:
                    FlipCheck();
                    HorizontalFloat();
                    VerticalFloat();
                    break;
                case AirState:
                    FlipCheck();
                    HorizontalFloat();
                    VerticalFloat();
                    break;
                case WallSlideState:
                    FlipCheck();
                    HorizontalFloat();
                    VerticalFloat();
                    break;
                case BlockState:
                    FlipCheck();
                    break;
                case RollState:
                    break;
                case AttackState:
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
            if (_previousState != null) _animator.ResetTrigger(_previousState.AnimationState.ToString());

            _animator.SetTrigger(state.AnimationState.ToString());
            _previousState = state;
        }

        #region AnimationEvents

        private void FootStep()
        {
            _fxManager ??= ServiceLocator.GetService<FxManager>();
            _fxManager.PlayFx(PlayerFx.FootStep, transform.position);
        }

        private void PerformAttack() => OnAttackPerformed?.Invoke();
        private void ComboAvailable() => OnComboAvailable?.Invoke();

        #endregion
    }
}