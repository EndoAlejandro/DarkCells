using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class GridBasedLevelGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int roomSize;
        [SerializeField] private Vector2Int levelSize;

        [SerializeField] private Transform globalTilemaps;
        [SerializeField] private Transform prefabRoomsPool;

        private GridRoom[] _prefabGridRooms;
        private GridRoomData[,] _roomDataMatrix;
        
        private readonly Dictionary<string, Tilemap> _globalTilemaps = new();

        public GridRoomData InitialRoom { get; private set; }

        private void Awake()
        {
            _roomDataMatrix = new GridRoomData[levelSize.x, levelSize.y];

            // Load Prefabs
            _prefabGridRooms = prefabRoomsPool.GetComponentsInChildren<GridRoom>(true);

            // Fill global Tilemaps
            Tilemap[] tiles = globalTilemaps.GetComponentsInChildren<Tilemap>();
            foreach (var tilemap in tiles) _globalTilemaps.Add(tilemap.gameObject.tag, tilemap);
        }

        [ContextMenu("Generate Level")]
        public void GenerateLevel()
        {
            SetRoomsPrefabsState(true);

            CalculatePath();
            InstantiateTiles();

            SetRoomsPrefabsState(false);
        }

        private void SetRoomsPrefabsState(bool state)
        {
            foreach (var room in _prefabGridRooms) room.gameObject.SetActive(state);
        }

        private void InstantiateTiles()
        {
            for (int i = 0; i < _roomDataMatrix.GetLength(0); i++)
            for (int j = 0; j < _roomDataMatrix.GetLength(1); j++)
            {
                var dataRoom = _roomDataMatrix[i, j];
                if (dataRoom == null) continue;

                GridRoomVariant selectedPrefabGridRoomVariant = SelectPrefabGridRoom(dataRoom);
                CopyGridRoomLayers(selectedPrefabGridRoomVariant, i, j);
            }
        }

        private GridRoomVariant SelectPrefabGridRoom(GridRoomData dataRoom)
        {
            GridRoom selectedPrefabGridRoomVariant = _prefabGridRooms[0];

            foreach (var gridRoom in _prefabGridRooms)
            {
                if (gridRoom.Directions != dataRoom.Directions) continue;
                selectedPrefabGridRoomVariant = gridRoom;
                break;
            }

            return selectedPrefabGridRoomVariant.GetRandomVariant();
        }

        private void CopyGridRoomLayers(GridRoomVariant prefabGridRoomVariant, int x, int y)
        {
            var tileLayers = prefabGridRoomVariant.GetTileLayers();
            foreach (var tilemap in tileLayers)
            {
                if (!_globalTilemaps.ContainsKey(tilemap.tag)) continue;
                CopyGridRoomLayer(tilemap, _globalTilemaps[tilemap.tag], x, y);
            }
        }

        private void CopyGridRoomLayer(Tilemap source, Tilemap target, int x, int y)
        {
            foreach (Vector3Int position in source.cellBounds.allPositionsWithin)
            {
                TileBase tile = source.GetTile(position);
                if (tile == null) continue;

                var offsetPosition = new Vector3Int(position.x + roomSize.x * x,
                    position.y - roomSize.y * y, position.z);
                target.SetTile(offsetPosition, tile);
            }
        }

        private void CalculatePath()
        {
            var initialX = Random.Range(0, _roomDataMatrix.GetLength(0));
            InitialRoom = new GridRoomData(new Vector2Int(initialX, 0));
            InitialRoom.SetDirection(Vector2Int.down);
            _roomDataMatrix[initialX, 0] = InitialRoom;

            Vector2Int currentPosition = InitialRoom.Position;

            int safeExit = 100;
            while (safeExit > 0)
            {
                safeExit--;

                var currentRoom = _roomDataMatrix[currentPosition.x, currentPosition.y];
                var direction = GetNextDirection(currentRoom);
                if (direction == Vector2Int.zero)
                {
                    currentRoom.SetDirection(Vector2Int.up);
                    break;
                }

                currentRoom.SetDirection(direction);

                var customPosition = currentRoom.Position;
                var room = new GridRoomData(customPosition + direction);

                room.SetDirection(direction * -1);
                _roomDataMatrix[room.Position.x, room.Position.y] = room;
                currentPosition = room.Position;
            }
        }

        private Vector2Int GetNextDirection(GridRoomData sourceRoom)
        {
            bool rightCheck = sourceRoom.Directions.y == 0 &&
                              sourceRoom.Position.x < _roomDataMatrix.GetLength(0) - 1;
            bool bottomCheck = sourceRoom.Directions.x == 0 &&
                               sourceRoom.Position.y < _roomDataMatrix.GetLength(1) - 1;
            bool leftCheck = sourceRoom.Directions.w == 0 &&
                             sourceRoom.Position.x > 0;

            if (rightCheck & !bottomCheck & !leftCheck) return Vector2Int.right;
            if (!rightCheck & !bottomCheck & leftCheck) return Vector2Int.left;
            if (!rightCheck & bottomCheck & !leftCheck) return Vector2Int.up;
            if (rightCheck & !bottomCheck & leftCheck)
                return Random.Range(0f, 1f) > .5f ? Vector2Int.right : Vector2Int.left;
            if (rightCheck & bottomCheck & !leftCheck)
                return Random.Range(0f, 1f) > .7f ? Vector2Int.up : Vector2Int.right;
            if (!rightCheck & bottomCheck & leftCheck)
                return Random.Range(0f, 1f) > .7f ? Vector2Int.up : Vector2Int.left;
            if (rightCheck & bottomCheck & leftCheck)
            {
                float prob = Random.Range(0f, 1f);
                return prob switch
                {
                    > .8f => Vector2Int.up,
                    > .4f => Vector2Int.left,
                    _ => Vector2Int.right
                };
            }

            return Vector2Int.zero;
        }

        public Vector2 GetWorldPosition(GridRoomData data) =>
            new(roomSize.x * data.Position.x, roomSize.y * data.Position.y);

        public CompositeCollider2D GetLevelBounds() =>
            _globalTilemaps["CameraCollider"].TryGetComponent(out CompositeCollider2D collider)
                ? collider
                : null;
    }
}