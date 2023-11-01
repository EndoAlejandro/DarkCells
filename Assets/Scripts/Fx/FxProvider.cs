using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using DarkHavoc.Pooling;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public enum FxType
    {
        Telegraph,
    }

    public class FxProvider : Singleton<FxProvider>
    {
        [SerializeField] private AnimatedPoolAfterSecond telegraphAttackPrefab;

        private Dictionary<FxType, PooledMonoBehaviour> _fxDictionary;

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            _fxDictionary = new Dictionary<FxType, PooledMonoBehaviour> { { FxType.Telegraph, telegraphAttackPrefab } };
        }

        public void GetFx(FxType fxType, Vector2 position)
        {
            if (!_fxDictionary.TryGetValue(fxType, out PooledMonoBehaviour result))
                return;

            result.Get<PooledMonoBehaviour>(position, Quaternion.identity);
        }
    }
}