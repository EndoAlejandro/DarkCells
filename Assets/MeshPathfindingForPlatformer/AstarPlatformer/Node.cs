using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Calcatz.MeshPathfinding {
    public class Node : MonoBehaviour {

#if UNITY_EDITOR
        [MenuItem("GameObject/Mesh Pathfinding/Create Node", false, 0)]
        private static Node CreateNode() {
            Waypoints waypoints = null;
            Node originNode = null;
            if (Selection.activeGameObject == null) {
                waypoints = SearchWaypoints();
            } else {
                waypoints = Selection.activeGameObject.GetComponent<Waypoints>();
                if (waypoints == null) {
                    originNode = Selection.activeGameObject.GetComponent<Node>();
                    if (originNode != null) {
                        waypoints = originNode.transform.parent.GetComponent<Waypoints>();
                    }
                    else {
                        waypoints = SearchWaypoints();
                    }
                }
            }
            GameObject go = new GameObject("Node");
            go.transform.parent = waypoints.transform;
            Node newNode = go.AddComponent<Node>();
            if (originNode != null) {
                go.transform.position = originNode.transform.position;
                go.transform.rotation = originNode.transform.rotation;
                go.transform.localScale = originNode.transform.localScale;
                if (originNode.neighbours == null) {
                    originNode.neighbours = new List<Node>();
                }
                originNode.neighbours.Add(newNode);
                newNode.neighbours = new List<Node>();
                newNode.neighbours.Add(originNode);
            }
            Selection.activeGameObject = go;
            return newNode;
        }

        private static Waypoints SearchWaypoints() {
            Waypoints waypoints = FindObjectOfType<Waypoints>();
            if (waypoints == null) {
                waypoints = Waypoints.CreateWaypoints();
            }

            return waypoints;
        }
#endif

        public class Data : IHeapItem<Data> {
            public Node nodeObject;
            public float gCost, hCost;
            public Data parent;
            public bool onPath;
            int heapIndex;

            public Data(Node _nodeObject) {
                nodeObject = _nodeObject;
                gCost = hCost = 0;
            }

            public int HeapIndex {
                get { return heapIndex; }
                set { heapIndex = value; }
            }

            public float fCost {
                get { return gCost + hCost; }
            }

            public int CompareTo(Data nodeData) {
                int compare = fCost.CompareTo(nodeData.fCost);
                if (compare == 0) {
                    compare = hCost.CompareTo(nodeData.hCost);
                }
                return -1 * compare;
            }
            public void ResetNode() {
                gCost = hCost = 0;
                parent = null;
            }
        }

        [Tooltip("This node will be ignored in calculation if it's not traversable.")]
        public bool traversable = true;
        public List<Node> neighbours;

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.15f);
            if (neighbours == null) return;
            List<Node> emptyNodes = new List<Node>();
            foreach (Node node2 in neighbours) {
                if (node2 != null) {
                    try {
                        if (node2.neighbours != null) {
                            if (!node2.neighbours.Contains(this)) {
                                Gizmos.color = Color.white;
                            }
                        }
                    }
                    catch {
                        emptyNodes.Add(node2);
                    }
                    Gizmos.DrawLine(transform.position, node2.transform.position);
                }
                else {
                    emptyNodes.Add(node2);
                }
            }
            foreach (Node emptyNode in emptyNodes) {
                neighbours.Remove(emptyNode);
            }
        }

    }
}
