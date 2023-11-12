using Cinemachine;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc
{
    public class CameraManager : Singleton<CameraManager>
    {
        public CinemachineVirtualCamera VirtualCamera { get; private set; }
        public CinemachineFramingTransposer Transposer { get; private set; }

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            Transposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public void SetTarget(Transform target)
        {
            VirtualCamera.m_Follow = target;
            VirtualCamera.m_LookAt = target;
        }
    }
}