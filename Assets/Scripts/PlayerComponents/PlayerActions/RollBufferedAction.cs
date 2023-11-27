using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    public class RollBufferedAction : BufferedAction
    {
        private bool _canRoll;
        private float _cooldown;

        public RollBufferedAction(Player player, float bufferTime, Func<bool> inputTrigger) :
            base(player, bufferTime, inputTrigger)
        {
        }

        protected override bool CanBuffer() => _cooldown <= 0f && _canRoll && base.CanBuffer();

        public override void Tick()
        {
            if (_cooldown > 0f) _cooldown -= Time.deltaTime;
            if (Player.Grounded) _canRoll = true;
            base.Tick();
        }


        public override void UseAction()
        {
            if (!_canRoll) return;

            base.UseAction();
            _canRoll = false;
            _cooldown = Player.Stats.RollCooldown + Player.Stats.RollAction.Time;
            Player.SetSpeedBonus(Player.Stats.SpeedBonus);
        }
    }
}