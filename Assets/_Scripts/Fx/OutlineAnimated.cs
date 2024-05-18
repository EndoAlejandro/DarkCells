using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.Fx
{
    public class OutlineAnimated : MonoBehaviour
    {
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

        [SerializeField] float animationSpeed = 1f;
        [SerializeField] private Vector2 animationRange;

        private SpriteRenderer _renderer;
        private MaterialPropertyBlock _propertyBlock;

        private float _animationIndex;
        private float _indexSin;
        private Color _outlineColor;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _propertyBlock = new MaterialPropertyBlock();

            _animationIndex = Random.Range(0f, 50f);

            GetColor();

            RenderParams rp = new RenderParams(_renderer.material) { matProps = _propertyBlock };
        }

        private void GetColor() => _outlineColor = _renderer.material.GetColor(OutlineColor);

        private void Start()
        {
            animationRange.x = Mathf.Clamp(animationRange.x, 0f, 1f);
            animationRange.y = Mathf.Clamp(animationRange.y, 0f, 1f);
        }

        private void Update()
        {
            GetColor();
            _animationIndex = (_animationIndex + Time.deltaTime * animationSpeed) % 100f;
            _indexSin = Mathf.Sin(_animationIndex);
            _outlineColor.a = _indexSin.Map(-1, 1, animationRange.x, animationRange.y);
            _renderer.material.SetColor(OutlineColor, _outlineColor);
        }
    }
}