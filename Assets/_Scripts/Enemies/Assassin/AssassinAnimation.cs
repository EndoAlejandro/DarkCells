using UnityEngine;

namespace DarkHavoc.Enemies.Assassin
{
    public class AssassinAnimation : EnemyAnimation
    {
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        
        private Assassin _assassin;

        protected override void Awake()
        {
            base.Awake();
            _assassin = enemy as Assassin;
        }
    }
}