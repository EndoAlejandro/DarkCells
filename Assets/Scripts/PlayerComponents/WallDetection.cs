using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DarkHavoc.PlayerComponents
{
    [Serializable]
    public struct WallDetection
    {
        [Header("Wall")]
        [SerializeField] private float bottomOffset;

        [SerializeField] private float horizontalOffset;
        [SerializeField] private float distanceCheck;

        [FormerlySerializedAs("wallLayerMask")] [SerializeField]
        private LayerMask wallLayer;

        [Header("Ledge")]
        [SerializeField] private float topOffset;

        [SerializeField] private Vector2 ledgeDetectorOffset;
        [SerializeField] private float ledgeDetectorRadius;


        public float DistanceCheck => distanceCheck;
        public float TopOffset => topOffset;
        public float BottomOffset => bottomOffset;
        public float HorizontalOffset => horizontalOffset;
        public Vector2 LedgeDetectorOffset => ledgeDetectorOffset;
        public float LedgeDetectorRadius => ledgeDetectorRadius;
        public LayerMask WallLayer => wallLayer;
    }
}