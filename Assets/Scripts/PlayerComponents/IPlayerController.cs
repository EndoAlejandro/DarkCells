using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}