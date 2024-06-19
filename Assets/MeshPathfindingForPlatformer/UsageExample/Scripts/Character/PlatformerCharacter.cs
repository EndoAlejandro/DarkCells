using UnityEngine;
using System.Collections.Generic;

namespace Calcatz.Example {
    public class PlatformerCharacter : PlayerPhysics {

        public Role role;
        public Transform model;
        public Animator animator;

        public float Speed {
            get { return velocity.x; }
        }

        public bool ControllerEnabled {
            get { return inputScripts[0].Enabled; }
            set {
                for (int i = 0; i < inputScripts.Count; i++) {
                    inputScripts[i].Enabled = value;
                }
            }
        }

        [SerializeField] private List<GameObject> inputScriptGameobjects;
        private List<ICharacterInput> inputScripts = new List<ICharacterInput>();

        private bool doubleJumpFlag;

        private AnimatorParameters animParams;

        public override void Awake() {
            base.Awake();
            col = GetComponent<Collider>();
            animator = model.GetComponent<Animator>();
            animParams = new AnimatorParameters();
        }
        
        protected override void FixedUpdate() {
            base.FixedUpdate();
            CheckBoundsCollision();
            animator.SetBool(animParams.isGrounded, collisions.below);
        }

        public void MoveInput(int direction) {
            float targetVelocity = direction * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            Move();
            animator.SetFloat(animParams.xSpeed, Mathf.Abs(velocity.x * 40));
            animator.SetFloat(animParams.ySpeed, velocity.y);
        }

        public void RotateModel(int direction) {
            collisions.faceDir = direction;
            float newRotation = ClampModelRotation(model.localRotation.eulerAngles.y);
            newRotation = Mathf.Clamp(newRotation + direction * 1080 * Time.deltaTime, -90, 90);
            model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, newRotation, model.localRotation.eulerAngles.z);
        }

        private float ClampModelRotation(float rotation) {
            if (rotation > 90) rotation -= 360;
            else if (rotation < -90) rotation += 360;
            return rotation;
        }

        public void Jump() {
            if ((collisions.below || doubleJumpFlag)) {
                velocity.y = maxJumpVelocity;
                doubleJumpFlag = !doubleJumpFlag;
            }
        }


        public void CheckBoundsCollision() {
            foreach (PlatformerCharacter character in GameManager.Instance.characters) {
                if (character.GetInstanceID() != this.GetInstanceID()) {
                    if (CheckVerticalBounds(character.col)) {
                        if (collisions.right) {
                            if (character.collisions.right) {
                                transform.Translate(new Vector3(-2 * Time.deltaTime, 0, 0));
                            }
                        }
                        if (collisions.left) {
                            if (character.collisions.left) {
                                transform.Translate(new Vector3(2 * Time.deltaTime, 0, 0));
                            }
                        }

                        if (CheckRightCollision(character.col)) {
                            float offset = col.bounds.max.x + (col.bounds.extents.x / 2.5f) - character.col.bounds.min.x;
                            if (character.collisions.right) {
                                transform.Translate(new Vector3(-offset, 0, 0));
                            } else if (character.Speed <= -moveSpeed / 4f) {
                                transform.Translate(new Vector3(-offset, 0, 0));
                                character.transform.Translate(new Vector3(offset, 0, 0));
                            } else {
                                character.transform.Translate(new Vector3(offset, 0, 0));
                            }
                        } else if (CheckRightOverlap(character.col)) {
                            if (character.Speed <= -moveSpeed / 4f) {
                                transform.Translate(new Vector3(-4 * Time.deltaTime, 0, 0));
                                character.transform.Translate(new Vector3(4 * Time.deltaTime, 0, 0));
                            } else {
                                if (character.collisions.right) {
                                    transform.Translate(new Vector3(-4 * Time.deltaTime, 0, 0));
                                } else {
                                    character.transform.Translate(new Vector3(4 * Time.deltaTime, 0, 0));
                                }
                            }
                        } else if (CheckLeftCollision(character.col)) {
                            float offset = col.bounds.min.x - (col.bounds.extents.x / 2.5f) - character.col.bounds.max.x;
                            if (character.collisions.left) {
                                transform.Translate(new Vector3(-offset, 0, 0));
                            } else if (character.Speed >= moveSpeed / 4f) {
                                transform.Translate(new Vector3(-offset, 0, 0));
                                character.transform.Translate(new Vector3(offset, 0, 0));
                            } else {
                                character.transform.Translate(new Vector3(offset, 0, 0));
                            }
                        } else if (CheckLeftOverlap(character.col)) {
                            if (character.Speed >= moveSpeed / 4f) {
                                transform.Translate(new Vector3(4 * Time.deltaTime, 0, 0));
                                character.transform.Translate(new Vector3(-4 * Time.deltaTime, 0, 0));
                            } else {
                                if (character.collisions.left) {
                                    transform.Translate(new Vector3(4 * Time.deltaTime, 0, 0));
                                } else {
                                    character.transform.Translate(new Vector3(-4 * Time.deltaTime, 0, 0));
                                }
                            }
                        }
                    }
                }
            }

        }

        public class AnimatorParameters {
            public int xSpeed;
            public int ySpeed;
            public int isGrounded;
            public int reset;

            public AnimatorParameters() {
                xSpeed = Animator.StringToHash("xSpeed");
                ySpeed = Animator.StringToHash("ySpeed");
                isGrounded = Animator.StringToHash("isGrounded");
                reset = Animator.StringToHash("reset");
            }
        }

        public enum Role {
            Player = 0,
            CPU
        }
    }
}
