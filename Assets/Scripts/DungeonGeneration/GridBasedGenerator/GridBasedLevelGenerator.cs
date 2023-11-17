using UnityEngine;
using UnityEngine.Tilemaps;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class GridBasedLevelGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int roomSize;
        [SerializeField] private Vector2Int levelSize;
        [SerializeField] private GridRoom[] rooms;

        [SerializeField] private Tilemap platformsTileMap;
        [SerializeField] private Tilemap visualsTileMap;
        [SerializeField] private Tilemap cameraColliderTileMap;

        private GridRoomData[,] _roomDataMatrix;

        public GridRoomData InitialRoom { get; private set; }

        private void Awake() => _roomDataMatrix = new GridRoomData[levelSize.x, levelSize.y];

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
            foreach (var room in rooms) room.gameObject.SetActive(state);
        }

        private void InstantiateTiles()
        {
            for (int i = 0; i < _roomDataMatrix.GetLength(0); i++)
            for (int j = 0; j < _roomDataMatrix.GetLength(1); j++)
            {
                var room = _roomDataMatrix[i, j];
                if (room == null) continue;

                GridRoom selectedGridRoom = rooms[0];
                foreach (var gridRoom in rooms)
                {
                    if (gridRoom.Directions != room.Directions) continue;
                    selectedGridRoom = gridRoom;
                    break;
                }

                CopyPlatformTiles(selectedGridRoom, i, j);
                CopyVisualTiles(selectedGridRoom, i, j);
                CopyCameraColliderTiles(selectedGridRoom, i, j);
            }
        }

        private void CopyPlatformTiles(GridRoom room, int x, int y)
        {
            Tilemap roomTilemap = room.PlatformsTileMap;

            foreach (Vector3Int position in roomTilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int offsetPosition = new Vector3Int(position.x + roomSize.x * x,
                    position.y - roomSize.y * y, position.z);
                TileBase tile = room.PlatformsTileMap.GetTile(position);

                if (tile != null) platformsTileMap.SetTile(offsetPosition, tile);
            }
        }

        private void CopyVisualTiles(GridRoom room, int x, int y)
        {
            Tilemap roomTilemap = room.VisualsTileMap;

            foreach (Vector3Int position in roomTilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int offsetPosition = new Vector3Int(position.x + roomSize.x * x,
                    position.y - roomSize.y * y, position.z);
                TileBase tile = room.VisualsTileMap.GetTile(position);

                if (tile != null) visualsTileMap.SetTile(offsetPosition, tile);
            }
        }

        private void CopyCameraColliderTiles(GridRoom room, int x, int y)
        {
            Tilemap roomTilemap = room.CameraColliderTileMap;

            foreach (Vector3Int position in roomTilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int offsetPosition = new Vector3Int(position.x + roomSize.x * x,
                    position.y - roomSize.y * y, position.z);
                TileBase tile = room.CameraColliderTileMap.GetTile(position);

                if (tile != null) cameraColliderTileMap.SetTile(offsetPosition, tile);
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

        public CompositeCollider2D GetLevelBounds() => cameraColliderTileMap.GetComponent<CompositeCollider2D>();
    }
}