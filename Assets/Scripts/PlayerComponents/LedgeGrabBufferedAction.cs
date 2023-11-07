using System;
using DarkHavoc.PlayerComponents.PlayerActions;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [Serializable]
    public class LedgeGrabBufferedAction : BufferedAction
    {
        public override bool IsAvailable => _canGrabLedge && _cooldown <= 0f;

        private bool _canGrabLedge;
        private float _cooldown;

        public LedgeGrabBufferedAction(Player player, float bufferTime, Func<bool> inputTrigger) : base(player,
            bufferTime, inputTrigger)
        {
            _canGrabLedge = true;
            WasActionPressed = true;

            Player.OnLedgeGrabChanged += PlayerOnLedgeGrabChanged;
        }

        private void PlayerOnLedgeGrabChanged(bool value)
        {
            _canGrabLedge = !value;
            _cooldown = value ? 0f : .1f;
        }

        public override void Tick()
        {
            if (_cooldown > 0f) _cooldown -= Time.deltaTime;
            base.Tick();
        }

        public override void UseAction()
        {
            _canGrabLedge = true;
        }
    }
}