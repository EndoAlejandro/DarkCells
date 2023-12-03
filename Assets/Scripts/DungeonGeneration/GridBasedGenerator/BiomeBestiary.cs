using DarkHavoc.Enemies;
using UnityEngine;

namespace DarkHavoc.DungeonGeneration.GridBasedGenerator
{
    [CreateAssetMenu(menuName = "Biome Bestiary", fileName = "BiomeBestiary", order = 0)]
    public class BiomeBestiary : ScriptableObject
    {
        [SerializeField] private Enemy[] bestiary;
        public Enemy[] Bestiary => bestiary;
    }
}