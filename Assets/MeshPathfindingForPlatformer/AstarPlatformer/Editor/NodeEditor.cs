using UnityEditor;
using UnityEngine;
using Kit.Editor;
using System.Collections.Generic;

namespace Calcatz.MeshPathfinding {
    [CustomEditor(typeof(Node)), CanEditMultipleObjects]
    class NodeEditor : Editor {

        private ReorderableListExtend neighboursList;

        private void OnEnable() {
            neighboursList = new ReorderableListExtend(serializedObject, "neighbours", true, false, true, true);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            Node nodeTarget = (Node)target;
            serializedObject.Update();

            if (nodeTarget.neighbours != null) {
                if (nodeTarget.neighbours.Count > 0) {
                    neighboursList.DoLayoutList();
                }
            }

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();

            if (Selection.gameObjects.Length > 1) {
                if (GUILayout.Button("Connect Selected Nodes")) {
                    List<Node> nodes = new List<Node>();
                    foreach (GameObject go in Selection.gameObjects) {
                        Node node = go.GetComponent<Node>();
                        if (node != null) {
                            nodes.Add(node);
                        }
                    }
                    if (nodes.Count > 1) {
                        foreach(Node nodeA in nodes) {
                            foreach (Node nodeB in nodes) {
                                if (nodeA != nodeB) {
                                    if (nodeA.neighbours == null) nodeA.neighbours = new System.Collections.Generic.List<Node>();
                                    if (!nodeA.neighbours.Contains(nodeB)) {
                                        nodeA.neighbours.Add(nodeB);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
        static void DrawGameObjectName(Transform transform, GizmoType gizmoType) {
            Node node = transform.GetComponent<Node>();
            if (node != null) {
                Handles.SphereHandleCap(transform.GetInstanceID(), transform.position, Quaternion.identity, 0.3f, EventType.Repaint);
                if (node.neighbours != null) {
                    foreach (Node neighbour in node.neighbours) {
                        try {
                            if (neighbour.neighbours != null) {
                                if (!neighbour.neighbours.Contains(node)) {
                                    float distance = Vector3.Distance(neighbour.transform.position, node.transform.position);
                                    Quaternion rot = Quaternion.LookRotation(neighbour.transform.position - node.transform.position);
                                    Handles.ArrowHandleCap(transform.GetInstanceID(), transform.position, rot, distance / 2, EventType.Repaint);
                                }
                            }
                        }
                        catch {

                        }
                    }
                }
                Handles.Label(transform.position, transform.gameObject.name);
            }
        }
    }
}