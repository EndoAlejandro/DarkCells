using Cinemachine;
using DarkHavoc.CustomUtils;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private float offsetVelocity = 6f;
        [SerializeField] private float dampingVelocity = 2f;

        private Player _player;

        private float _distance;
        private Vector3 _leftPosition;
        private Vector3 _rightPosition;
        private Vector3 _targetPosition;

        private float _maxDamping;
        private float _targetDamping;

        private CameraManager _cameraManager;

        private void Awake() => _player = GetComponentInParent<Player>();

        private void Start()
        {
            _cameraManager = ServiceLocator.Instance.GetService<CameraManager>();
            
            FallingDampingSetup();
            CameraFollowSetup();
            _cameraManager.SetTarget(transform);
        }

        private void FallingDampingSetup()
        {
            _maxDamping = _cameraManager.Transposer.m_YDamping;
        }

        private void CameraFollowSetup()
        {
            _rightPosition = transform.localPosition;
            _leftPosition = transform.localPosition.With(x: -transform.localPosition.x);
        }

        private void Update()
        {
            FallingDampingController();
            CameraFollowController();
        }

        private void FallingDampingController()
        {
            _targetDamping = _player.Grounded ? _maxDamping : 0f;
            float distance = Mathf.Abs(_cameraManager.Transposer.m_YDamping - _targetDamping);
            if (distance < 0.02f) return;
            _cameraManager.Transposer.m_YDamping = Mathf.MoveTowards(_cameraManager.Transposer.m_YDamping, _targetDamping,
                Time.deltaTime * distance * dampingVelocity);
        }

        private void CameraFollowController()
        {
            _targetPosition = _player.FacingLeft ? _leftPosition : _rightPosition;
            float distance = Vector3.Distance(_targetPosition, transform.localPosition);
            if (distance < 0.02f) return;
            transform.localPosition =
                Vector3.MoveTowards(transform.localPosition, _targetPosition,
                    Time.deltaTime * distance * offsetVelocity);
        }
    }
}