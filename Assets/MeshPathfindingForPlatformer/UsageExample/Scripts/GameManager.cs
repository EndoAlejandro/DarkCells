using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calcatz.MeshPathfinding;

namespace Calcatz.Example {
    public class GameManager : MonoBehaviour {

        private static GameManager instance;
        public static GameManager Instance {
            get {
                return instance;
            }
        }

        public List<PlatformerCharacter> characters = new List<PlatformerCharacter>();

        private void Awake() {
            instance = this;
            List<PlatformerCharacter> charactersToRemove = new List<PlatformerCharacter>();
            foreach(PlatformerCharacter character in characters) {
                if (!character.gameObject.activeInHierarchy) {
                    charactersToRemove.Add(character);
                }
            }
            foreach (PlatformerCharacter character in charactersToRemove) {
                characters.Remove(character);
            }
        }

        private void Start() {
            foreach(PlatformerCharacter charA in characters) {
                foreach(PlatformerCharacter charB in characters) {
                    if (charA != charB) {
                        Physics.IgnoreCollision(characters[0].GetComponent<Collider>(), characters[1].GetComponent<Collider>());
                    }
                }
            }
            Physics.IgnoreLayerCollision(9, 10);

        }
    }
}