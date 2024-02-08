using UnityEngine;

namespace DarkHavoc.Enemies.ShockSweeper
{
    public class ShockSweeperAnimation : EnemyAnimation
    {
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        private ShockSweeper _shockSweeper;

        protected override void Awake()
        {
            base.Awake();
            _shockSweeper = enemy as ShockSweeper;
        }

        protected override void Update()
        {
            base.Update();
            animator.SetFloat(Vertical, _shockSweeper.GetNormalizedVertical());
        }
    }
}