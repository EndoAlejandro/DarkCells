using System.Collections.Generic;
using DarkHavoc.Pooling;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public enum FxType
    {
        Telegraph,
        ColossalMelee,
    }

    public class FxProvider : Service<FxProvider>
    {
        [SerializeField] private AnimatedPoolAfterSecond telegraphAttackPrefab;
        [SerializeField] private AnimatedPoolAfterSecond colossalMeleeExplosion;

        private Dictionary<FxType, PooledMonoBehaviour> _fxDictionary;

        protected override void Awake()
        {
            base.Awake();
            _fxDictionary = new Dictionary<FxType, PooledMonoBehaviour>
            {
                { FxType.Telegraph, telegraphAttackPrefab },
                { FxType.ColossalMelee, colossalMeleeExplosion },
            };
        }

        public void GetFx(FxType fxType, Vector2 position)
        {
            if (!_fxDictionary.TryGetValue(fxType, out PooledMonoBehaviour result))
                return;

            result.Get<PooledMonoBehaviour>(position, Quaternion.identity);
        }
    }
}