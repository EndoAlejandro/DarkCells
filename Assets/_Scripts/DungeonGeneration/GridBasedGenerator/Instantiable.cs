﻿using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    public struct Instantiable
    {
        public Transform Prefab { get; }
        public Vector3 WorldPosition { get; }

        public Instantiable(Transform prefab, Vector3 worldPosition)
        {
            Prefab = prefab;
            WorldPosition = worldPosition;
        }
    }
}