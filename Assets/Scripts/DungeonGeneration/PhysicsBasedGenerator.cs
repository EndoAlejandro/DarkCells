using System;
using System.Collections.Generic;
using DarkHavoc.CustomUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DarkHavoc.DungeonGeneration
{
    public class PhysicsBasedGenerator : MonoBehaviour
    {
        [SerializeField] private SpringRoom roomPrefab;

        [SerializeField] private int roomsAmount = 4;
        [SerializeField] private Vector2 generationRadius = new Vector2(.5f, 2f);

        private List<SpringRoom> _rooms = new List<SpringRoom>();
        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

        private void Start() => GenerateRooms();

        [ContextMenu("Regenerate Rooms")]
        private void ReGenerateRooms()
        {
            foreach (var room in _rooms) Destroy(room.gameObject);
            _rooms.Clear();
            GenerateRooms();
        }

        private void GenerateRooms()
        {
            for (int i = 0; i < roomsAmount; i++)
            {
                float randomRadius = Random.Range(generationRadius.x, generationRadius.y);
                Vector2 roomPosition = Random.insideUnitCircle * randomRadius;
                var room = Instantiate(roomPrefab, roomPosition, Quaternion.identity);
                _rooms.Add(room);
            }

            //_rooms.Shuffle();
            _rooms[0].Setup(_rigidbody);
            for (int i = 1; i < _rooms.Count; i++)
            {
                var target = _rooms[i - 1].GetComponent<Rigidbody2D>();
                _rooms[i].Setup(target);
            }
        }
    }
}