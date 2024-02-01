using Calcatz.MeshPathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public Waypoints waypoints;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        GameObject unitGO = Instantiate(unitPrefab);
        Pathfinding unitPathfinding = unitGO.GetComponent<Pathfinding>();
        unitPathfinding.Waypoints = waypoints;
        unitPathfinding.SetTarget(target);
    }
}