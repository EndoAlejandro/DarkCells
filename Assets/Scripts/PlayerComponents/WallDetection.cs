using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [Serializable]
    public struct WallDetection
    {
        [SerializeField] private float distanceCheck;
        [SerializeField] private float topOffset;
        [SerializeField] private float bottomOffset;
        [SerializeField] private LayerMask wallLayerMask;
        public float DistanceCheck => distanceCheck;
        public float TopOffset => topOffset;
        public float BottomOffset => bottomOffset;
        public LayerMask WallLayerMask => wallLayerMask;
    }
}