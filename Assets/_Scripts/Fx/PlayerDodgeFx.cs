using DarkHavoc.Managers;
using DarkHavoc.PlayerComponents;
using DarkHavoc.Pooling;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public class PlayerDodgeFx : AnimatedPoolAfterSecond
    {
        private Player _player;
        private SpriteRenderer _spriteRenderer;

        protected override void OnEnable()
        {
            _player ??= ServiceLocator.GetService<GameManager>()?.Player;
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
            
            if (_player == null)
            {
                ReturnToPool();
                return;
            }

            _spriteRenderer.sprite = PlayerAnimation.Sprite;
            _spriteRenderer.flipX = _player.FacingLeft;
            base.OnEnable();
        }
    }
}