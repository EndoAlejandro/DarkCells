using System.Collections;
using Cinemachine;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc
{
    public class CameraManager : Singleton<CameraManager>
    {
        public CinemachineVirtualCamera VirtualCamera { get; private set; }
        public CinemachineFramingTransposer Transposer { get; private set; }
        private CinemachineConfiner2D _confiner;

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            Transposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _confiner = VirtualCamera.GetComponent<CinemachineConfiner2D>();
        }

        public void SetTarget(Transform target)
        {
            VirtualCamera.m_Follow = target;
            VirtualCamera.m_LookAt = target;
        }

        public void SetCameraBounds(CompositeCollider2D composite) => StartCoroutine(SetCameraBoundsAsync(composite));

        private IEnumerator SetCameraBoundsAsync(CompositeCollider2D composite)
        {
            _confiner.enabled = false;
            _confiner.InvalidateCache();
            _confiner.m_BoundingShape2D = composite;
            yield return null;
            _confiner.InvalidateCache();
            _confiner.enabled = true;
        }
    }
}