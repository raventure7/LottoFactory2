using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WayPoint : MonoBehaviour
{
    [SerializeField]
    private WayPoint nextWaypoint;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public WayPoint GetnextWaypoint()
    {
        return nextWaypoint;
    }
}
