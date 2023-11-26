using System;
using UnityEngine;

namespace DarkHavoc.CameraComponents
{
    [ExecuteInEditMode]
    public class ParallaxCamera : MonoBehaviour
    {
        public event Action<float> OnCameraTranslate;
        private float _oldPosition;
        private void Start() => _oldPosition = transform.position.x;

        private void Update()
        {
            if (Math.Abs(transform.position.x - _oldPosition) < .05f) return;
            OnCameraTranslate?.Invoke(_oldPosition - transform.position.x);
            _oldPosition = transform.position.x;
        }
    }
}