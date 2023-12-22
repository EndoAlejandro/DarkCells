using System;
using UnityEngine;

namespace DarkHavoc
{
    [Serializable]
    public struct BiomeData
    {
        [SerializeField] private Biome biome;
        [SerializeField] private int steps;
        public Biome Biome => biome;
        public int Steps => steps;
    }
}