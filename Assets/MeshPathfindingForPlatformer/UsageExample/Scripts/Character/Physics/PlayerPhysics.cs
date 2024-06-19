using UnityEngine;

namespace Calcatz.Example {
    public class PlayerPhysics : RaycastController {

        public CollisionInfo collisions;
        public bool useGravity;

        public float maxJumpHeight = 4.2f;
        public float minJumpHeight = 0.5f;
        public float timeToJumpApex = 0.35f;
        public float moveSpeed = 11;

        public Vector2 velocity;
        protected float velocityXSmoothing;
        protected float accelerationTimeAirborne = 0.2f;
        protected float accelerationTimeGrounded = 0.1f;

        protected float maxJumpVelocity;
        protected float minJumpVelocity;

        private float gravity;

        private float maxClimbAngle = 85;
        private float maxDescendAngle = 85;

        protected float rayLengthFront;
        protected float rayYStep;
        protected float rayLengthAttack;

        public float RayLengthAttack {
            set { rayLengthAttack = value; }
        }

        public override void Start() {
            base.Start();

            useGravity = true;
            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

            rayLengthFront = (col.bounds.max.x - col.bounds.min.x) / 2 + 0.02f;   //0.02f front collision tolerance
            rayYStep = ((col.bounds.max.y - col.bounds.min.y) / 2) - 0.1f;      //0.001f distance gap of surface and raycast origin
            rayLengthAttack = 0.5f;
        }

        protected virtual void FixedUpdate() {
            CollisionCheck();
            if (useGravity) {
                velocity.y += gravity * Time.deltaTime;
            }
        }

        public void Move(bool standingOnPlatform = false) {
            collisions.Reset();
            collisions.velocityOld = velocity;

            if (velocity.y < 0) {
                DescendSlope();
            }

            HorizontalCollisions();

            if (velocity.y != 0) {
                VerticalCollisions();
            }

            transform.Translate(new Vector3(velocity.x, velocity.y, 0));

            if (standingOnPlatform) {
                collisions.below = true;
            }
        }

        private void CollisionCheck() {
            if (collisions.below) {
                velocity.y = 0;
            }
        }

        protected void HorizontalCollisions() {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            if (Mathf.Abs(velocity.x) < skinWidth) {
                rayLength = skinWidth + 0.1f;
            }
            for (int it = 0; it < 2; it++) {
                float directionX = (it == 0) ? collisions.faceDir : -collisions.faceDir;
                for (int i = 0; i < horizontalRayCount; i++) {
                    Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft.position : raycastOrigins.bottomRight.position;
                    rayOrigin += Vector3.up * (horizontalRaySpacing * i);
                    Vector3 rayDir = transform.rotation * Vector3.right * directionX;
                    Ray ray = new Ray(rayOrigin, rayDir);
                    RaycastHit hit = new RaycastHit();

                    //Debug.DrawRay(rayOrigin, rayDir * rayLength, Color.red);

                    if (Physics.Raycast(ray, out hit, rayLength, collisionMask)) {
                        if (hit.distance == 0) {
                            continue;
                        }

                        float boundsX = (directionX == 1) ? col.bounds.max.x : col.bounds.min.x;

                        float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                        if (i == 0 && slopeAngle <= maxClimbAngle) {
                            if (collisions.descendingSlope) {
                                collisions.descendingSlope = false;
                                velocity = collisions.velocityOld;
                            }
                            float distanceToSlopeStart = 0;
                            if (slopeAngle != collisions.slopeAngleOld) {
                                distanceToSlopeStart = hit.distance - skinWidth;
                                velocity.x -= distanceToSlopeStart * directionX;
                            }
                            ClimbSlope(slopeAngle);
                            if (GetVelocityXSign(velocity.x) == 0 || directionX == GetVelocityXSign(velocity.x)) {
                                transform.Translate(new Vector3(hit.point.x - boundsX, 0, 0));
                            }
                            rayLength = hit.distance;
                        }

                        if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
                            if (GetVelocityXSign(velocity.x) == 0 || directionX == GetVelocityXSign(velocity.x)) {
                                transform.Translate(new Vector3(hit.point.x - boundsX, 0, 0));
                            }
                            rayLength = hit.distance;

                            if (collisions.climbingSlope) {
                                velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad * velocity.x);
                            }

                            collisions.left = directionX == -1;
                            collisions.right = directionX == 1;
                        }
                    }
                }
            }
        }

        protected void VerticalCollisions() {
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;
            for (int i = 0; i < verticalRayCount; i++) {
                Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft.position : raycastOrigins.topLeft.position;
                rayOrigin += transform.rotation * Vector3.right * (verticalRaySpacing * i + velocity.x);
                Ray ray = new Ray(rayOrigin, Vector3.up * directionY);
                RaycastHit hit = new RaycastHit();

                //Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);

                if (Physics.Raycast(ray, out hit, rayLength, groundMask)) {
                    if (directionY == -1) {
                        velocity.y = (hit.distance - skinWidth) * directionY;
                    }
                    rayLength = hit.distance;

                    if (collisions.climbingSlope) {
                        velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }
            }
            if (collisions.climbingSlope) {
                float directionX = Mathf.Sign(velocity.x);
                rayLength = Mathf.Abs(velocity.x) + skinWidth;
                Vector3 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft.position : raycastOrigins.bottomRight.position) + Vector3.up * velocity.y;
                Ray ray = new Ray(rayOrigin, transform.rotation * Vector3.forward * directionX);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit, rayLength, groundMask)) {
                    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    if (slopeAngle != collisions.slopeAngleOld) {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                        collisions.slopeAngle = slopeAngle;
                    }
                }
            }
        }

        protected void ClimbSlope(float slopeAngle) {
            float moveDistance = Mathf.Abs(velocity.x);
            float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

            if (velocity.y <= climbVelocityY) {
                velocity.y = climbVelocityY;
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                collisions.below = true;
                collisions.climbingSlope = true;
                collisions.slopeAngle = slopeAngle;
            }
        }

        protected void DescendSlope() {
            float directionX = Mathf.Sign(velocity.x);
            Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight.position : raycastOrigins.bottomLeft.position;    //kebalikan karena menuruni slope
            Ray ray = new Ray(rayOrigin, -Vector3.up);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask)) {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
                    if (Mathf.Sign(hit.normal.x) == directionX) {                                                           //arah normal == arah player, berarti menuruni slope
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {    //cek jarak karena raycast infinity    
                            float moveDistance = Mathf.Abs(velocity.x);
                            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                            velocity.y -= descendVelocityY;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                        }
                    }
                }
            }
        }

        protected bool CheckVerticalBounds(Collider col2) {
            return (col.bounds.min.y < col2.bounds.center.y) &&
                    (col.bounds.max.y > col2.bounds.center.y);
        }

        protected bool CheckRightCollision(Collider col2) {
            return (col.bounds.max.x + col.bounds.extents.x / 2.5f >= col2.bounds.min.x) &&
                    (col.bounds.max.x < col2.bounds.min.x);
        }

        protected bool CheckRightOverlap(Collider col2) {
            return (col.bounds.max.x >= col2.bounds.min.x) &&
                    (col.bounds.center.x <= col2.bounds.center.x);
        }

        protected bool CheckLeftCollision(Collider col2) {
            return (col.bounds.min.x - col.bounds.extents.x / 2.5f <= col2.bounds.max.x) &&
                    (col.bounds.min.x > col2.bounds.max.x);
        }

        protected bool CheckLeftOverlap(Collider col2) {
            return (col.bounds.min.x <= col2.bounds.max.x) &&
                    (col.bounds.center.x >= col2.bounds.center.x);
        }

        public void AddForce(Vector2 force) {
            collisions.below = false;
            velocity = force;
        }

        private int GetVelocityXSign(float velX) {
            if (velX > moveSpeed / 2) {
                return 1;
            } else if (velX < -moveSpeed / 2) {
                return -1;
            } else {
                return 0;
            }
        }

        [System.Serializable]
        public struct CollisionInfo {
            public bool above, below;
            public bool left, right;
            public bool climbingSlope;
            public bool descendingSlope;

            public float slopeAngle, slopeAngleOld;
            public Vector2 velocityOld;
            public int faceDir;

            public void Reset() {
                above = below = false;
                left = right = false;
                climbingSlope = false;
                descendingSlope = false;

                slopeAngleOld = slopeAngle;
                slopeAngle = 0;
            }
        }

    }
}
