using UnityEngine;

namespace DarkHavoc.Fx
{
    public class BossFeedbackFx : FeedbackFx
    {
        public BossFx FxType => fxType;
        [SerializeField] private BossFx fxType;
    }
}