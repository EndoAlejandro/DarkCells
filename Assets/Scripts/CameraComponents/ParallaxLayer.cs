using UnityEngine;

namespace DarkHavoc.CameraComponents
{
    [ExecuteInEditMode]
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float parallaxFactor;

        public void Move(float delta)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.x -= delta * parallaxFactor;

            transform.localPosition = newPosition;
        }
    }
}