using UnityEngine;

namespace DarkHavoc.Fx
{
    public class EnemyFeedbackFx : FeedbackFx
    {
        public EnemyFx FxType => fxType;
        [SerializeField] private EnemyFx fxType;
    }
}