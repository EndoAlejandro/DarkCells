using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.TarnishedWidow.States
{
    public class TarnishedWidowJumpUpAttackState : BossAttackState
    {
        private readonly float _duration;
        private float _timer;

        public TarnishedWidowJumpUpAttackState(TarnishedWidow tarnishedWidow, BossAnimation animation,
            EnemyHitBox hitBox, float offset) : base(tarnishedWidow,
            animation, hitBox, AnimationState.JumpAttack, offset) =>
            _duration = tarnishedWidow.JumpHitBox.TelegraphTime;

        public override void Tick() => _timer -= Time.deltaTime;

        public override void OnEnter()
        {
            base.OnEnter();
            ((TarnishedWidow)boss).JumpUp();
            _timer = _duration;
        }
    }

    public class TarnishedWidowJumpDownAttackState : BossAttackState
    {
        private Player _player;
        private Vector3 _point;

        public TarnishedWidowJumpDownAttackState(TarnishedWidow tarnishedWidow, BossAnimation animation,
            EnemyHitBox hitBox, float offset) : base(tarnishedWidow,
            animation, hitBox, AnimationState.JumpAttack, offset)
        {
        }


        public override void OnEnter()
        {
            base.OnEnter();
            _player ??= ServiceLocator.GetService<GameManager>().Player;
            var result = Physics2D.Raycast(_player.transform.position, Vector2.down,
                50, LayerMask.NameToLayer("Terrain/Ground"));
            _point = result ? result.point : _player.transform.position;
            ((TarnishedWidow)boss).Teleport(_point);
        }

        protected override void AnimationOnAttackPerformed()
        {
            base.AnimationOnAttackPerformed();
            ((TarnishedWidow)boss).JumpDown();
        }

        public override void OnExit()
        {
            base.OnExit();
            boss.ActivateBuff(5f);
        }
    }
}