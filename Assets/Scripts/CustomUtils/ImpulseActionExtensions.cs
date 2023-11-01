using DarkHavoc.ImpulseComponents;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.CustomUtils
{
    public static class ImpulseActionExtensions
    {
        /// <summary>
        /// Return Impulse velocity with a direction.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="direction">Normalized looking direction. [-1=left][1=right]</param>
        /// <returns>Max Velocity of impulse.</returns>
        public static float GetTargetVelocity(this ImpulseAction action, int direction) =>
            action.Force * action.Direction * direction;

        /// <summary>
        /// Smooth deceleration of the impulse using custom delta.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="currentVelocity">Velocity before deceleration.</param>
        /// <param name="delta">FrameRate based deceleration.</param>
        /// <returns>Reduced initial velocity.</returns>
        public static float Decelerate(this ImpulseAction action, float currentVelocity, float delta) =>
            Mathf.MoveTowards(currentVelocity, 0f, delta * action.Deceleration);

        public enum ImpulseDirection
        {
            Forward,
            Backward
        }
    }
}