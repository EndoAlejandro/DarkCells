using System;
using System.Collections.Generic;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace Calcatz.MeshPathfinding
{
    public class MasterWayPoints : Service<MasterWayPoints>
    {
        public List<Node> Nodes { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Nodes = new List<Node>();
            DebugOnly();
        }

        [Obsolete]
        private void DebugOnly()
        {
            var result = FindObjectsOfType<Waypoints>();
            foreach (var waypoints in result)
            {
                AddNodes(waypoints.Nodes);
            }
        }

        public void AddNode(Node node, Vector3 position = default)
        {
            var newNode = new GameObject("Node").AddComponent<Node>();
            newNode.transform.position = position;
            newNode.neighbours = node.neighbours;
            newNode.transform.SetParent(transform);
            Nodes.Add(node);
        }

        public void AddNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var newNode = new GameObject("Node").AddComponent<Node>();
                // newNode.transform.position
                newNode.SourceNode = node;
                // newNode.neighbours = node.neighbours;
                newNode.transform.SetParent(transform);
                Nodes.Add(node);
            }
        }

        public Node FindNode(Vector3 position)
        {
            int waypointIndex = 0;
            float distance = Vector3.Distance(position, Nodes[0].transform.position);
            for (int i = 0; i < Nodes.Count; i++)
            {
                float newDistance = Vector3.Distance(position, Nodes[i].transform.position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    waypointIndex = i;
                }
            }

            return Nodes[waypointIndex];
        }

        public Node FindNodeLessThanHeight(Vector3 _position, float _unitHeight)
        {
            int waypointIndex = 0;
            float maxY = _position.y + _unitHeight;
            float distance = Vector3.Distance(_position, Nodes[0].transform.position);
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].transform.position.y > maxY) continue;
                float newDistance = Vector3.Distance(_position, Nodes[i].transform.position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    waypointIndex = i;
                }
            }

            return Nodes[waypointIndex];
        }
    }
}