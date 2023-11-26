using UnityEngine;

namespace DarkHavoc.DungeonGeneration
{
    public class SpringRoom : MonoBehaviour
    {
        private SpringJoint2D _spring;
        public void Setup(Rigidbody2D target)
        {
            _spring = GetComponent<SpringJoint2D>();
            _spring.connectedBody = target;
            _spring.distance = Random.Range(.25f, 1.5f);
        }
    }
}