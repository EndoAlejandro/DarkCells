/*
 * This is an example of how we utilize the path result that has been calculated.
 */

using UnityEngine;
using Calcatz.MeshPathfinding;

namespace Calcatz.Example {
    public class AIBehaviour : PathfindingUserBase, ICharacterInput {

        private int moveDirection;

        private PlatformerCharacter character;
        public PlatformerCharacter Character {
            get {
                return character;
            }
        }

        [SerializeField] private bool disableAI;
        public bool Enabled {
            get { return !disableAI; }
            set { disableAI = !value; }
        }

        protected bool doubleJumped;


        public virtual void Start() {
            character = GetComponent<PlatformerCharacter>();
            float unitHeight = character.col.bounds.max.y - transform.position.y;

            pathfinding.StartFindPath(unitHeight);
        }


        protected virtual void FixedUpdate() {
            moveDirection = 0;

            if (!disableAI) {
                Node[] pathResult = pathfinding.GetPathResult();
                if (pathResult != null) {

                    Node nextNode;
                    if (pathfinding.IncludeStartingNode) {
                        nextNode = pathResult.Length > 1 ? pathResult[1] : null;
                    }
                    else {
                        nextNode = pathResult.Length > 0 ? pathResult[0] : null;
                    }

                    //Here is the decision tree to decide what to do with the path information

                    if (nextNode != null) {

                        //If closest node is higher than even character's head, then consider jumping
                        bool considerJumping = character.col.bounds.max.y < nextNode.transform.position.y
                                                && character.col.bounds.min.y < nextNode.transform.position.y;

                        if (considerJumping) {
                            TryJumping();
                        }

                        if (pathfinding.IncludeStartingNode) {
                            if (considerJumping && !character.collisions.below) {
                                //Run towards the starting node, make sure to stand on the ground first.
                                //This is to prevent the case in which the character is moving to the next node without capability of jumping.
                                RunTowardsNode(pathResult[0]);
                            }
                            else {
                                RunTowardsNode(nextNode);
                            }
                        }
                        else {
                            //Run towards the closest node
                            RunTowardsNode(nextNode);
                        }
                    }
                    else {
                        if (!character.collisions.below) {
                            if (!doubleJumped) {
                                if (character.velocity.y <= 0) {
                                    character.Jump();
                                    doubleJumped = true;
                                }
                            }
                        }
                    }

                }
            }
            character.MoveInput(moveDirection);
        }

        private void TryJumping() {
            //If character is previously not touching ground, then check if it can double jump
            if (!character.collisions.below) {
                if (!doubleJumped) {
                    if (character.velocity.y <= 0) {
                        character.Jump();
                        doubleJumped = true;
                    }
                }
            }
            else {
                character.Jump();
                doubleJumped = false;
            }
        }

        private void RunTowardsNode(Node _targetNode) {
            if (character.col.bounds.center.x < _targetNode.transform.position.x) {
                if (!character.collisions.below) {
                    if (character.collisions.faceDir == 1) {
                        moveDirection = 1;
                        character.RotateModel(moveDirection);
                    }
                }
                else {
                    moveDirection = 1;
                    character.RotateModel(moveDirection);
                }
            }

            if (character.col.bounds.center.x > _targetNode.transform.position.x) {
                if (!character.collisions.below) {
                    if (character.collisions.faceDir == -1) {
                        moveDirection = -1;
                        character.RotateModel(moveDirection);
                    }
                }
                else {
                    moveDirection = -1;
                    character.RotateModel(moveDirection);
                }
            }
        }

    }
}
