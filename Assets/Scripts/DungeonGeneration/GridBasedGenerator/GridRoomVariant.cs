using UnityEngine;
using UnityEngine.Tilemaps;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class GridRoomVariant : MonoBehaviour
    {
        [SerializeField] private Transform spawnPointsContainer;

        private Tilemap[] _roomTileMaps;

        public Tilemap[] GetTileLayers()
        {
            if (_roomTileMaps == null || _roomTileMaps.Length == 0)
                _roomTileMaps = GetComponentsInChildren<Tilemap>(true);

            return _roomTileMaps;
        }

        public Transform[] GetSpawnPoints()
        {
            int count = spawnPointsContainer.childCount;
            var children = new Transform[count];
            for (int i = 0; i < count; i++) children[i] = spawnPointsContainer.GetChild(i);
            return children;
        }
    }
}