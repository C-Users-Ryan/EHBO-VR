using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpointselector : MonoBehaviour
{
    public bool spawnAtPointA;
    public bool spawnAtPointB;
    public bool spawnAtPointC;

    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    public GameObject objectToMove;

    void Start()
    {
        SelectSpawnPoint();
    }

    void SelectSpawnPoint()
    {
        if (objectToMove == null)
        {
            Debug.LogError("No object assigned to move!");
            return;
        }

        if (spawnAtPointA)
        {
            MoveObjectTo(pointA);
        }
        else if (spawnAtPointB)
        {
            MoveObjectTo(pointB);
        }
        else if (spawnAtPointC)
        {
            MoveObjectTo(pointC);
        }
        else
        {
            Debug.LogWarning("No spawn point selected, defaulting to Point A.");
            MoveObjectTo(pointA); // Default if none is selected
        }
    }

    void MoveObjectTo(Transform spawnPoint)
    {
        objectToMove.transform.position = spawnPoint.position;
    }
}

