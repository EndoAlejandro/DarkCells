using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies;
using DarkHavoc.Senses;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.HeartHoarder.States
{
    public class HeartHoarderWalkAttackState : BossAttackState
    {
        public override bool Ended => _timer <= 0f;
        private readonly HeartHoarder _heartHoarder;
        private readonly float _duration;
        private float _timer;

        public HeartHoarderWalkAttackState(HeartHoarder heartHoarder, BossAnimation animation, EnemyHitBox hitBox,
            AnimationState animationState, float offset, float duration) : base(heartHoarder, animation, hitBox,
            animationState, offset)
        {
            _heartHoarder = heartHoarder;
            _duration = duration;
        }

        public override void Tick() => _timer -= Time.deltaTime;

        public override void FixedTick()
        {
            boss.Move(boss.FacingLeft ? -1 : 1);
            var result = EntityVision.WallRayCast(boss.MidPoint.position,
                boss.FacingLeft ? Vector2.left : Vector2.right, _heartHoarder.WallDetectionDistance,
                _heartHoarder.WallLayerMask, out _);
            if (result)
            {
                boss.SetFacingLeft(!boss.FacingLeft);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _timer = _duration;
        }
    }
}