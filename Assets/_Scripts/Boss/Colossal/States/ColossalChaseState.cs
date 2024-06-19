using DarkHavoc.Boss.SharedStates;
using UnityEngine;

namespace DarkHavoc.Boss.Colossal.States
{
    public class ColossalChaseState : BossChaseState
    {
        public bool RangedAvailable { get; private set; }
        public bool MeleeAvailable { get; private set; }
        public bool BuffAvailable { get; private set; }
        public bool BoomerangAvailable => _boomerangCdTimer <= 0f && PlayerInFront;

        private bool PlayerInFront => (boss.FacingLeft && _direction < 0) ||
                                      (!boss.FacingLeft && _direction > 0);

        private float _boomerangCooldown;
        private float _boomerangCdTimer;

        public ColossalChaseState(Colossal colossal, float stoppingDistance) : base(colossal, stoppingDistance)
        {
            _boomerangCooldown = ((Colossal)boss).BoomerangArms.Cooldown;
        }

        public override void Tick()
        {
            base.Tick();
            _boomerangCdTimer -= Time.deltaTime;
        }

        public override async void FixedTick()
        {
            SetDirection();

            boss.Move(Stop || _changingDirection ? 0 : _direction);

            if (_changingDirection) return;
            MeleeAvailable = ((Colossal)boss).MeleeHitBox.IsPlayerInRange();
            BuffAvailable = ((Colossal)boss).BuffHitBox.IsPlayerInRange();
            RangedAvailable = ((Colossal)boss).RangedHitBox.IsPlayerInRange();

            if (_direction < 0 && !boss.FacingLeft && !MeleeAvailable) await ModifyFacingDirection(true);
            else if (_direction > 0 && boss.FacingLeft && !MeleeAvailable) await ModifyFacingDirection(false);
        }

        public override void OnExit()
        {
            BuffAvailable = false;
            RangedAvailable = false;
            MeleeAvailable = false;
            base.OnExit();
        }

        public void BoomerangCooldown()
        {
            _boomerangCdTimer = _boomerangCooldown;
        }
    }
}