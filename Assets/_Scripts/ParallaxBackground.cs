using UnityEngine;

namespace DarkHavoc
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Vector2 multiplier;
        [SerializeField] private bool infiniteHorizontal;
        [SerializeField] private bool infiniteVertical;

        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private Vector3 _deltaMovement;
        private Vector2 _textureUnitSize;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            _lastCameraPosition = _cameraTransform.position;

            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Texture2D texture = sprite.texture;
            _textureUnitSize = new Vector2(texture.width / sprite.pixelsPerUnit,
                texture.height / sprite.pixelsPerUnit);
        }

        private void LateUpdate()
        {
            _deltaMovement = _cameraTransform.position - _lastCameraPosition;
            transform.position += new Vector3(_deltaMovement.x * multiplier.x, _deltaMovement.y * multiplier.y, 0f);
            _lastCameraPosition = _cameraTransform.position;

            if (infiniteHorizontal &&
                Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSize.x)
            {
                float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSize.x;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }

            if (infiniteVertical && 
                Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUnitSize.y)
            {
                float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureUnitSize.y;
                transform.position = new Vector3(_cameraTransform.position.x, transform.position.y + offsetPositionY);
            }
        }
    }
}