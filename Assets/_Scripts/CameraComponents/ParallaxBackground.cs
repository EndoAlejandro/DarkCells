using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkHavoc.CameraComponents
{
    [ExecuteInEditMode][Obsolete("Do not use this class.", true)]
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private ParallaxCamera parallaxCamera;
        private readonly List<ParallaxLayer> _parallaxLayers = new List<ParallaxLayer>();

        private void Start()
        {
            if (parallaxCamera == null)
                parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

            if (parallaxCamera != null)
                parallaxCamera.OnCameraTranslate += Move;

            SetLayers();
        }

        private void SetLayers()
        {
            _parallaxLayers.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

                if (layer == null) continue;
                layer.name = "Layer-" + i;
                _parallaxLayers.Add(layer);
            }
        }

        private void Move(float delta)
        {
            foreach (ParallaxLayer layer in _parallaxLayers) layer.Move(delta);
        }
    }
}