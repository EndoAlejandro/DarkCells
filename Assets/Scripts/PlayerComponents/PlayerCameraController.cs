using Cinemachine;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private float offsetVelocity = 6f;
        [SerializeField] private float dampingVelocity = 2f;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private CinemachineFramingTransposer _transposer;
        private Player _player;

        private float _distance;
        private Vector3 _leftPosition;
        private Vector3 _rightPosition;
        private Vector3 _targetPosition;

        private float _maxDamping;
        private float _targetDamping;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Start()
        {
            FallingDampingSetup();
            CameraFollowSetup();
        }

        private void FallingDampingSetup()
        {
            _maxDamping = _transposer.m_YDamping;
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
            float distance = Mathf.Abs(_transposer.m_YDamping - _targetDamping);
            if (distance < 0.02f) return;
            _transposer.m_YDamping = Mathf.MoveTowards(_transposer.m_YDamping, _targetDamping,
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