using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.Managers;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.TarnishedWidow.States
{
    public class TarnishedWidowJumpUpAttackState : BossAttackState
    {
        private readonly float _duration;
        private float _timer;

        public TarnishedWidowJumpUpAttackState(DarkHavoc.Boss.TarnishedWidow.TarnishedWidow tarnishedWidow, BossAnimation animation,
            EnemyHitBox hitBox, float offset) : base(tarnishedWidow,
            animation, hitBox, AnimationState.JumpAttack, offset) =>
            _duration = tarnishedWidow.JumpHitBox.TelegraphTime;

        public override void Tick() => _timer -= Time.deltaTime;

        public override void OnEnter()
        {
            Ended = false;

            animation.OnAttackPerformed += AnimationOnAttackPerformed;
            animation.OnAttackEnded += AnimationOnAttackEnded;

            ((DarkHavoc.Boss.TarnishedWidow.TarnishedWidow)boss).JumpUp();
            _timer = _duration;
        }
    }

    public class TarnishedWidowJumpDownAttackState : BossAttackState
    {
        private Player _player;
        private Vector3 _point;

        public TarnishedWidowJumpDownAttackState(DarkHavoc.Boss.TarnishedWidow.TarnishedWidow tarnishedWidow, BossAnimation animation,
            EnemyHitBox hitBox, float offset) : base(tarnishedWidow,
            animation, hitBox, AnimationState.JumpAttack, offset)
        {
        }


        public override void OnEnter()
        {
            _player ??= ServiceLocator.GetService<GameManager>().Player;
            var result = Physics2D.Raycast(_player.transform.position, Vector2.down,
                50, LayerMask.NameToLayer("Terrain/Ground"));
            _point = result ? result.point : _player.transform.position;
            ((DarkHavoc.Boss.TarnishedWidow.TarnishedWidow)boss).Teleport(_point);
            base.OnEnter();
        }

        protected override void AnimationOnAttackPerformed()
        {
            base.AnimationOnAttackPerformed();
            ((DarkHavoc.Boss.TarnishedWidow.TarnishedWidow)boss).JumpDown();
        }

        public override void OnExit()
        {
            base.OnExit();
            boss.ActivateBuff(5f);
        }
    }
}