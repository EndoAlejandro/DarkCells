using System.Collections;
using Cinemachine;
using DarkHavoc.Managers;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc
{
    public class CameraManager : Service<CameraManager>
    {
        [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera menuVirtualCamera;

        private CinemachineConfiner2D _confiner;

        public CinemachineFramingTransposer Transposer { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Transposer = mainVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _confiner = mainVirtualCamera.GetComponent<CinemachineConfiner2D>();
        }

        private void Start()
        {
            GameManager.OnGamePauseChanged += GameManagerOnGamePauseChanged;
        }

        private void GameManagerOnGamePauseChanged(bool isPaused)
        {
            mainVirtualCamera.gameObject.SetActive(!isPaused);
            menuVirtualCamera.gameObject.SetActive(isPaused);
        }

        public void SetTarget(Transform target)
        {
            mainVirtualCamera.m_Follow = target;
            mainVirtualCamera.m_LookAt = target;

            menuVirtualCamera.m_Follow = target.parent;
            menuVirtualCamera.m_LookAt = target.parent;
        }

        public void SetCameraBounds(CompositeCollider2D bounds)
        {
            if (bounds == null) return;
            StartCoroutine(SetCameraBoundsAsync(bounds));
        }

        private IEnumerator SetCameraBoundsAsync(CompositeCollider2D composite)
        {
            _confiner.enabled = false;
            _confiner.InvalidateCache();
            _confiner.m_BoundingShape2D = composite;
            yield return null;
            _confiner.InvalidateCache();
            _confiner.enabled = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.OnGamePauseChanged -= GameManagerOnGamePauseChanged;
        }
    }
}