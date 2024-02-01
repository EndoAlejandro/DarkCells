using UnityEngine;

namespace Calcatz.Example {
    [RequireComponent(typeof(Collider))]
    public class RaycastController : MonoBehaviour {

        public LayerMask collisionMask;
        public LayerMask groundMask;

        public const float skinWidth = .4f;
        public const float distanceBetweenRays = 0.25f;

        [HideInInspector] public int horizontalRayCount = 16;
        [HideInInspector] public int verticalRayCount = 8;

        [HideInInspector] public float horizontalRaySpacing;
        [HideInInspector] public float verticalRaySpacing;
        [HideInInspector] public Collider col;

        public RaycastOrigins raycastOrigins;

        public virtual void Awake() {
            col = GetComponent<Collider>();
        }

        public virtual void Start() {
            CalculateRaySpacing();
            BuildRaycastOrigins();
        }

        public void UpdateRaycastOrigins() {
            Bounds bounds = col.bounds;
            bounds.Expand(skinWidth * -2);
            raycastOrigins.centerBottom.transform.position = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            raycastOrigins.bottomLeft.transform.position = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
            raycastOrigins.bottomRight.transform.position = new Vector3(bounds.max.x, bounds.min.y, bounds.center.z);
            raycastOrigins.topLeft.transform.position = new Vector3(bounds.min.x, bounds.max.y, bounds.center.z);
            raycastOrigins.topRight.transform.position = new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);
        }

        public virtual void BuildRaycastOrigins() {
            Bounds bounds = col.bounds;
            bounds.Expand(skinWidth * -2);
            GameObject centerBottom = new GameObject("Bottom Center Raycast");
            centerBottom.transform.position = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            centerBottom.transform.parent = transform;
            GameObject bottomLeft = new GameObject("Bottom Left Raycast");
            bottomLeft.transform.position = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
            bottomLeft.transform.parent = transform;
            GameObject bottomRight = new GameObject("Bottom Right Raycast");
            bottomRight.transform.position = new Vector3(bounds.max.x, bounds.min.y, bounds.center.z);
            bottomRight.transform.parent = transform;
            GameObject topLeft = new GameObject("Top Left Raycast");
            topLeft.transform.position = new Vector3(bounds.min.x, bounds.max.y, bounds.center.z);
            topLeft.transform.parent = transform;
            GameObject topRight = new GameObject("Top Right Raycast");
            topRight.transform.position = new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);
            topRight.transform.parent = transform;
            raycastOrigins.centerBottom = centerBottom.transform;
            raycastOrigins.bottomLeft = bottomLeft.transform;
            raycastOrigins.bottomRight = bottomRight.transform;
            raycastOrigins.topLeft = topLeft.transform;
            raycastOrigins.topRight = topRight.transform;
        }

        public void CalculateRaySpacing() {
            Bounds bounds = col.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        [System.Serializable]
        public struct RaycastOrigins {
            public Transform centerBottom;
            public Transform topLeft, topRight;
            public Transform bottomLeft, bottomRight;
        }
    }
}