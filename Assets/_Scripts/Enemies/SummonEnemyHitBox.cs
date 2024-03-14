using System;
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

        private Vector2 _target;

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
            /*OverlapHitBox();*/

            DamageResult result = DamageResult.Failed;
            if (entity.Player)
            {
                // TODO: Spawn in the flour in a random range.
                result = DamageResult.Success;
                Instantiate(summonPrefab, _target, Quaternion.identity);
            }
            else
            {
                Debug.Log("Failed");
            }

            StartCoroutine(CooldownAsync());
            return result;
        }
    }
}