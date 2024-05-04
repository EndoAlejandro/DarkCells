using System;
using System.Collections.Generic;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace Calcatz.MeshPathfinding
{
    public class MasterWayPoints : Service<MasterWayPoints>
    {
        private Dictionary<Node, Node> _nodePairs;
        public List<Node> Nodes { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Nodes = new List<Node>();
        }

        public void CreateNode(Node sourceNode, Vector3 position = default)
        {
            var newNode = new GameObject("Node").AddComponent<Node>();
            newNode.transform.position = position;
            newNode.transform.SetParent(transform);
            newNode.SetSourceNode(sourceNode);
            Nodes.Add(newNode);
            _nodePairs.Add(sourceNode, newNode);
        }

        public void CleanNodeDictionary()
        {
            _nodePairs ??= new Dictionary<Node, Node>();
            _nodePairs.Clear();
        }

        public void ConnectNodes()
        {
            foreach (var node in Nodes)
            {
                if (node.SourceNode == null) continue;
                var sourceNeighbours = node.SourceNode.neighbours;
                foreach (var sourceNeighbour in sourceNeighbours)
                {
                    if (_nodePairs.TryGetValue(sourceNeighbour, out Node newNeighbour))
                        node.AddNeighbour(newNeighbour);
                }

                node.SetSourceNode(null);
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