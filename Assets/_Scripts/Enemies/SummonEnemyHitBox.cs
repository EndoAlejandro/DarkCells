using System;
using System.Collections.Generic;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DarkHavoc.Enemies
{
    public class SummonEnemyHitBox : EnemyHitBox
    {
        [SerializeField] private Enemy summonPrefab;
        [SerializeField] private float spawnRange = 2f;
        [SerializeField] private int maxSimultaneousSpawn = 3;

        private List<ITakeDamage> _spawnedEnemies;
        private Vector2 _target;

        private void OnEnable() => _spawnedEnemies = new List<ITakeDamage>();

        public override bool IsPlayerInRange() =>
            _spawnedEnemies.Count < maxSimultaneousSpawn && base.IsPlayerInRange();

        protected override void OverlapHitBox()
        {
            var size = isCircle ? OverlapCircle() : OverlapBox();

            for (int i = 0; i < size; i++)
            {
                if (!results[i].TryGetComponent(out Player player) ||
                    !results[i].transform.root.TryGetComponent(out player)) continue;
                this.player = player;
                return;
            }

            player = null;
        }

        public void TryToAttack(Vector2 target, bool isUnstoppable = false)
        {
            target.x += Random.Range(-spawnRange / 2, spawnRange / 2);

            RaycastHit2D result = new RaycastHit2D();
            for (int i = 0; i < 10; i++)
            {
                result = Physics2D.Raycast(target, Vector2.down, 15f, LayerMask.NameToLayer("Terrain"));
                if (result.point != Vector2.zero) break;
            }

            _target = result.point == Vector2.zero ? target : result.point;

            TryToAttack(isUnstoppable);
        }

        [Obsolete]
        public override DamageResult TryToAttack(bool isUnstoppable = false)
        {
            SetUnstoppable(isUnstoppable);

            DamageResult result = DamageResult.Failed;
            if (entity.Player)
            {
                result = DamageResult.Success;
                var summon = Instantiate(summonPrefab, _target, Quaternion.identity);
                summon.OnDeath += SummonOnDeath;
                _spawnedEnemies.Add(summon);
            }
            else
            {
                Debug.Log("Failed");
            }

            StartCoroutine(CooldownAsync());
            return result;
        }

        private void SummonOnDeath(ITakeDamage takeDamage)
        {
            takeDamage.OnDeath -= SummonOnDeath;
            _spawnedEnemies.Remove(takeDamage);
        }
    }
}