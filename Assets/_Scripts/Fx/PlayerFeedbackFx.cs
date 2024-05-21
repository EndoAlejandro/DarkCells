using UnityEngine;

namespace DarkHavoc.Fx
{
    public class PlayerFeedbackFx : FeedbackFx
    {
        public PlayerFx FxType => fxType;
        [SerializeField] private PlayerFx fxType;
    }
}