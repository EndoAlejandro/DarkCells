using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [Serializable]
    public struct WallDetection
    {
        [Header("Wall")]
        [SerializeField] private float distanceCheck;

        [SerializeField] private float topOffset;
        [SerializeField] private float midOffset;
        [SerializeField] private float bottomOffset;

        [SerializeField] private float horizontalOffset;
        [SerializeField] private LayerMask wallLayer;

        [Header("Ledge")]
        [SerializeField] private Vector2 ledgeDetectorOffset;
        [SerializeField] private float ledgeDetectorRadius;


        public float DistanceCheck => distanceCheck;
        public float TopOffset => topOffset;
        public float MidOffset => midOffset;
        public float BottomOffset => bottomOffset;
        public float HorizontalOffset => horizontalOffset;
        public Vector2 LedgeDetectorOffset => ledgeDetectorOffset;
        public float LedgeDetectorRadius => ledgeDetectorRadius;
        public LayerMask WallLayer => wallLayer;
    }
}