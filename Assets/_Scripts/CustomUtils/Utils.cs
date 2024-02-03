using UnityEngine;

namespace DarkHavoc.CustomUtils
{
    public static class Constants
    {
        public static readonly float HitAnimationDuration = 0.2f;
        public static readonly float TurnAwait = 0.25f;
        public static readonly LayerMask PlayerLayer = ~LayerMask.NameToLayer("Player");
    }
}