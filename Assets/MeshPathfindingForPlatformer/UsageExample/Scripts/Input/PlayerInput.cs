using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


namespace Calcatz.Example {
    [RequireComponent(typeof(PlatformerCharacter))]
    public class PlayerInput : MonoBehaviour, ICharacterInput {

        protected int moveDirection = 0;
        protected PlatformerCharacter character;

        private bool disableInput;
        public bool Enabled {
            get { return !disableInput; }
            set { disableInput = !value; }
        }

        private bool jumpFlag;
        private bool previousCollisionBelow;

        void Awake() {
            character = GetComponent<PlatformerCharacter>();
        }

        public virtual void Update() {
            moveDirection = 0;

            if (!disableInput) {
                if (Input.GetKey(KeyCode.A)) {
                    moveDirection = -1;
                    character.RotateModel(moveDirection);
                } else if (Input.GetKey(KeyCode.D)) {
                    moveDirection = 1;
                    character.RotateModel(moveDirection);
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    jumpFlag = true;
                }
            }
        }

        private void FixedUpdate() {
            if (previousCollisionBelow) {
                if (!character.collisions.below) {
                    jumpFlag = true;
                }
            }

            if (jumpFlag) {
                character.Jump();
                jumpFlag = false;
            }

            character.MoveInput(moveDirection);

            previousCollisionBelow = character.collisions.below;
        }
    }
}
