using System;
using System.Collections.Generic;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public class FxManager : Service<FxManager>
    {
        protected override bool DonDestroyOnLoad => true;

        private Dictionary<PlayerFx, PlayerFeedbackFx> _playerFx;
        private Dictionary<EnemyFx, EnemyFeedbackFx> _enemyFx;
        private Dictionary<BossFx, BossFeedbackFx> _bossFx;

        protected override void Awake()
        {
            base.Awake();
            _playerFx = new Dictionary<PlayerFx, PlayerFeedbackFx>();
            _enemyFx = new Dictionary<EnemyFx, EnemyFeedbackFx>();
            _bossFx = new Dictionary<BossFx, BossFeedbackFx>();

            var feedbacks = GetComponentsInChildren<FeedbackFx>();
            foreach (var feedback in feedbacks) AddFxToDictionary(feedback);
        }

        private void AddFxToDictionary(FeedbackFx feedback)
        {
            switch (feedback)
            {
                case PlayerFeedbackFx playerFeedbackFx:
                    _playerFx.TryAdd(playerFeedbackFx.FxType, playerFeedbackFx);
                    break;
                case EnemyFeedbackFx enemyFeedbackFx:
                    _enemyFx.TryAdd(enemyFeedbackFx.FxType, enemyFeedbackFx);
                    break;
                case BossFeedbackFx bossFeedbackFx:
                    _bossFx.TryAdd(bossFeedbackFx.FxType, bossFeedbackFx);
                    break;
            }
        }

        /// <summary>
        /// Play Player feedback fx.
        /// </summary>
        /// <param name="fxType">PlayerFx type.</param>
        /// <param name="position">Position to play fx.</param>
        /// <param name="scale">Instanced prefab scale.</param>
        /// <param name="flipX">Sprite looking direction.</param>
        /// <param name="randomizeRotation">Randomize rotation on Z axis.</param>
        public void PlayFx(PlayerFx fxType, Vector2 position = default, float scale = 1f, bool flipX = false,
            bool randomizeRotation = false)
        {
            if (!_playerFx.TryGetValue(fxType, out PlayerFeedbackFx result))
            {
                FxNotFound(fxType);
                return;
            }

            result.PlayFx(position, scale, flipX, randomizeRotation);
        }
        
        /// <summary>
        /// Play Enemy feedback fx.
        /// </summary>
        /// <param name="fxType">EnemyFx type.</param>
        /// <param name="position">Position to play fx.</param>
        /// <param name="scale">Instanced prefab scale.</param>
        /// <param name="flipX">Sprite looking direction.</param>
        /// <param name="randomizeRotation">Randomize rotation on Z axis.</param>
        public void PlayFx(EnemyFx fxType, Vector2 position = default, float scale = 1f, bool flipX = false,
            bool randomizeRotation = false)
        {
            if (!_enemyFx.TryGetValue(fxType, out EnemyFeedbackFx result))
            {
                FxNotFound(fxType);
                return;
            }

            result.PlayFx(position, scale, flipX, randomizeRotation);
        }
        
        /// <summary>
        /// Play Boss feedback fx.
        /// </summary>
        /// <param name="fxType">BossFx type.</param>
        /// <param name="position">Position to play fx.</param>
        /// <param name="scale">Instanced prefab scale.</param>
        /// <param name="flipX">Sprite looking direction.</param>
        /// <param name="randomizeRotation">Randomize rotation on Z axis.</param>
        public void PlayFx(BossFx fxType, Vector2 position = default, float scale = 1f, bool flipX = false,
            bool randomizeRotation = false)
        {
            if (!_bossFx.TryGetValue(fxType, out BossFeedbackFx result))
            {
                FxNotFound(fxType);
                return;
            }

            result.PlayFx(position, scale, flipX, randomizeRotation);
        }

        private void FxNotFound(Enum e) => Debug.LogError($"{e} Fx not found.");
    }
}