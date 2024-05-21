using DarkHavoc.CustomUtils;
using DarkHavoc.Pooling;
using MoreMountains.Feedbacks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DarkHavoc.Fx
{
    [RequireComponent(typeof(MMF_Player))]
    public abstract class FeedbackFx : MonoBehaviour
    {
        [SerializeField] private AnimatedPoolAfterSecond prefabFx;

        private MMF_Player _feedback;

        private void Awake() => _feedback = GetComponent<MMF_Player>();

        public void PlayFx(Vector2 position = default, float scale = 1f, bool flipX = false,
            bool randomizeRotation = false)
        {
            _feedback.PlayFeedbacks(position);
            if (!prefabFx) return;

            var rotation = randomizeRotation ? Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) : Quaternion.identity;
            var pooled = prefabFx.Get<PooledMonoBehaviour>(position, rotation);
            pooled.transform.localScale = Vector3.one * scale;
            if (flipX) pooled.transform.localScale = pooled.transform.localScale.With(x: -scale);
        }
    }
}