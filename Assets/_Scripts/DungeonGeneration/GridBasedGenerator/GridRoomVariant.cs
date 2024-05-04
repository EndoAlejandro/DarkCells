using Calcatz.MeshPathfinding;
using DarkHavoc.CustomUtils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class GridRoomVariant : MonoBehaviour
    {
        public Waypoints WayPoints => waypoints;

        [SerializeField] private Transform spawnPointsContainer;
        [SerializeField] private Transform instantiables;
        [SerializeField] private Waypoints waypoints;

        private Tilemap[] _roomTileMaps;

        public Tilemap[] GetTileLayers()
        {
            if (_roomTileMaps == null || _roomTileMaps.Length == 0)
                _roomTileMaps = GetComponentsInChildren<Tilemap>(true);

            return _roomTileMaps;
        }

        public Transform[] GetSpawnPoints() => spawnPointsContainer?.GetChildren();
        public Transform[] GetInstantiables() => instantiables?.GetChildren();
    }
}