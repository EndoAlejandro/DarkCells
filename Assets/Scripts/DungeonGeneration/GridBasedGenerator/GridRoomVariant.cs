using UnityEngine;
using UnityEngine.Tilemaps;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class GridRoomVariant : MonoBehaviour
    {
        private Tilemap[] _roomTileMaps;

        public Tilemap[] GetTileLayers()
        {
            if(_roomTileMaps == null || _roomTileMaps.Length == 0)
                _roomTileMaps = GetComponentsInChildren<Tilemap>(true);
            
            return _roomTileMaps;
        }
    }
}