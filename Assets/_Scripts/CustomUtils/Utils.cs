using UnityEngine;

namespace DarkHavoc.CustomUtils
{
    public static class Constants
    {
        public static readonly float HitAnimationDuration = 0.2f;
        public static readonly float TurnAwait = 0.25f;
        public static readonly LayerMask PlayerLayer = ~LayerMask.NameToLayer("Player");
        public static readonly Color EnemyOutlineColor = new Color(126f/255f,191f/255f,198f/255f, 128f/255f);
        public static readonly Color BossOutlineColor = new Color(126f/255f,191f/255f,198f/255f, 128f/255f);
    }
}