using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public class GridBasedLevelGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int roomSize;
        [SerializeField] private Vector2Int levelSize;
        [SerializeField] private GridRoom[] rooms;

        [SerializeField] private Tilemap globalTileMap;

        private GridRoomData[,] _roomDataMatrix;

        private void Awake()
        {
            _roomDataMatrix = new GridRoomData[levelSize.x, levelSize.y];
        }

        private void Start()
        {
            GenerateLevel();
            InstantiateTiles();

            foreach (var room in rooms) room.gameObject.SetActive(false);
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

                CopyTile(selectedGridRoom, i, j);
            }
        }

        private void CopyTile(GridRoom room, int x, int y)
        {
            Tilemap roomTilemap = room.Tilemap;

            foreach (Vector3Int position in roomTilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int offsetPosition = new Vector3Int(position.x + roomSize.x * x,
                    position.y - roomSize.y * y, position.z);
                TileBase tile = room.Tilemap.GetTile(position);

                if (tile != null) globalTileMap.SetTile(offsetPosition, tile);
            }
        }

        private void GenerateLevel()
        {
            var initialX = Random.Range(0, _roomDataMatrix.GetLength(0));
            var initialRoom = new GridRoomData(new Vector2Int(initialX, 0));
            initialRoom.SetDirection(Vector2Int.down);
            _roomDataMatrix[initialX, 0] = initialRoom;

            Vector2Int currentPosition = initialRoom.Position;
            int safeExit = 100;
            while (safeExit > 0)
            {
                safeExit--;

                var currentRoom = _roomDataMatrix[currentPosition.x, currentPosition.y];
                var direction = GetNextDirection(currentRoom);
                if (direction == Vector2Int.zero) break;

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
    }
}