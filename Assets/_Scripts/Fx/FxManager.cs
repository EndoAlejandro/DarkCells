using System.Collections.Generic;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public class FxManager : Service<FxManager>
    {
        protected override bool DonDestroyOnLoad => true;

        private Dictionary<FxType, FeedbackFx> _fxDictionary;

        protected override void Awake()
        {
            base.Awake();
            _fxDictionary = new Dictionary<FxType, FeedbackFx>();
            var feedbacks = GetComponentsInChildren<FeedbackFx>();
            foreach (var feedback in feedbacks)
            {
                _fxDictionary.TryAdd(feedback.FxType, feedback);
            }
        }

        public void PlayFx(FxType fxType, Vector2 position = default, float scale = 1f, bool flipX = false,
            bool randomizeRotation = false)
        {
            if (!_fxDictionary.TryGetValue(fxType, out FeedbackFx result)) return;

            result.PlayFx(position, scale, flipX, randomizeRotation);
        }
    }
}