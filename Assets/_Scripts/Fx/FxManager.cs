using System.Collections.Generic;
using DarkHavoc.Pooling;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public enum FxType
    {
        Telegraph,
        DangerousTelegraph,
        ColossalMelee,
    }

    public class FxManager : Service<FxManager>
    {
        [SerializeField] private AnimatedPoolAfterSecond telegraphAttackPrefab;
        [SerializeField] private AnimatedPoolAfterSecond dangerousTelegraphAttackPrefab;
        [SerializeField] private AnimatedPoolAfterSecond colossalMeleeExplosion;

        private Dictionary<FxType, PooledMonoBehaviour> _fxDictionary;

        protected override void Awake()
        {
            base.Awake();
            _fxDictionary = new Dictionary<FxType, PooledMonoBehaviour>
            {
                { FxType.Telegraph, telegraphAttackPrefab },
                { FxType.DangerousTelegraph, dangerousTelegraphAttackPrefab },
                { FxType.ColossalMelee, colossalMeleeExplosion },
            };
        }

        public void GetFx(FxType fxType, Vector2 position, float scale = 1f)
        {
            if (!_fxDictionary.TryGetValue(fxType, out PooledMonoBehaviour result))
                return;

            var pooled = result.Get<PooledMonoBehaviour>(position, Quaternion.identity);
            pooled.transform.localScale = Vector3.one * scale;
        }
    }
}