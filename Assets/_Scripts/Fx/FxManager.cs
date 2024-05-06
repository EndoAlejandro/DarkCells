using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using DarkHavoc.Pooling;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public enum FxType
    {
        SwordSlash,

        //
        Telegraph,
        DangerousTelegraph,
        ColossalMelee,
        ArcherAttack,

        //
        DualSlicer1,
        DualSlicer2,
        DualSlicer3,

        //
        HeavySlicer1,
        HeavySlicer2,
    }

    public class FxManager : Service<FxManager>
    {
        [Header("Player")]
        [SerializeField] private AnimatedPoolAfterSecond swordSlashPrefab;

        [Header("Enemies")]
        [SerializeField] private AnimatedPoolAfterSecond telegraphAttackPrefab;

        [SerializeField] private AnimatedPoolAfterSecond dangerousTelegraphAttackPrefab;
        [SerializeField] private AnimatedPoolAfterSecond colossalMeleeExplosion;
        [SerializeField] private AnimatedPoolAfterSecond archerAttack;

        [Header("Dual Slicer")]
        [SerializeField] private AnimatedPoolAfterSecond dualSlicerAttack1;

        [SerializeField] private AnimatedPoolAfterSecond dualSlicerAttack2;
        [SerializeField] private AnimatedPoolAfterSecond dualSlicerAttack3;

        [Header("Heavy Slicer")]
        [SerializeField] private AnimatedPoolAfterSecond heavySlicerAttack1;

        [SerializeField] private AnimatedPoolAfterSecond heavySlicerAttack2;

        private Dictionary<FxType, PooledMonoBehaviour> _fxDictionary;

        protected override void Awake()
        {
            base.Awake();
            _fxDictionary = new Dictionary<FxType, PooledMonoBehaviour>
            {
                { FxType.SwordSlash, swordSlashPrefab },
                { FxType.Telegraph, telegraphAttackPrefab },
                { FxType.DangerousTelegraph, dangerousTelegraphAttackPrefab },
                { FxType.ColossalMelee, colossalMeleeExplosion },
                { FxType.ArcherAttack, archerAttack },
                { FxType.DualSlicer1, dualSlicerAttack1 },
                { FxType.DualSlicer2, dualSlicerAttack2 },
                { FxType.DualSlicer3, dualSlicerAttack3 },
                { FxType.HeavySlicer1, heavySlicerAttack1 },
                { FxType.HeavySlicer2, heavySlicerAttack2 },
            };
        }

        public void PlayFx(FxType fxType, Vector2 position, float scale = 1f, bool flipX = false,
            bool randomizeRotation = false)
        {
            if (!_fxDictionary.TryGetValue(fxType, out PooledMonoBehaviour result))
                return;

            var pooled = result.Get<PooledMonoBehaviour>(position,
                randomizeRotation ? Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) : Quaternion.identity);
            pooled.transform.localScale = Vector3.one * scale;
            if (flipX) pooled.transform.localScale = pooled.transform.localScale.With(x: -scale);
        }
    }
}