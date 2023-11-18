using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class GridRoom : MonoBehaviour
    {
        [SerializeField] private Vector4 directions;

        private GridRoomVariant[] _variants;
        public Vector4 Directions => directions;
        private void SetupVariants() => _variants ??= GetComponentsInChildren<GridRoomVariant>(true);

        private void Awake() => _variants = GetComponentsInChildren<GridRoomVariant>(true);

        public GridRoomVariant GetRandomVariant()
        {
            //SetupVariants();
            _variants = GetComponentsInChildren<GridRoomVariant>(true);
            int index = Random.Range(0, _variants.Length);
            return GetVariant(index);
        }

        private GridRoomVariant GetVariant(int index) => _variants[index];
    }
}