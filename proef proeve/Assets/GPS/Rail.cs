using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{

    public float closestDistance = Mathf.Infinity;
    public float secondClosestDistance = Mathf.Infinity;
    public string waypointTag = "waypoint";

    [SerializeField]
    private Transform closestWaypoint1;
    [SerializeField]
    private Transform closestWaypoint2;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform projectedObject;

    private Vector3 projectedPosition;

    void Start()
    {
        // Find all objects with the "waypoint" tag
        waypoints = GameObject.FindGameObjectsWithTag(waypointTag);

        FindWaypoints();
    }

    void Update()
    {
        RailLockOn();

        if (projectedObject.position == closestWaypoint1.position || projectedObject.position == closestWaypoint2.position)
        {
            FindWaypoints();
        }
    }

    private void FindWaypoints()
    {
        Transform previousClosestWaypoint1 = closestWaypoint1;
        Transform previousClosestWaypoint2 = closestWaypoint2;
        closestDistance = Mathf.Infinity;
        secondClosestDistance = Mathf.Infinity;

        // Loop through all waypoints to find the closest and second closest
        foreach (GameObject waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.transform.position);
            if (distance < closestDistance)
            {
                if (closestWaypoint1 != waypoint.transform) // added condition
                {
                    secondClosestDistance = closestDistance;
                    closestWaypoint2 = closestWaypoint1;
                }
                closestDistance = distance;
                closestWaypoint1 = waypoint.transform;
            }
            else if (distance < secondClosestDistance && closestWaypoint1 != waypoint.transform) // added condition
            {
                secondClosestDistance = distance;
                closestWaypoint2 = waypoint.transform;
            }
        }

        // Check if we have switched to a new set of waypoints
        if (previousClosestWaypoint1 != closestWaypoint1 || previousClosestWaypoint2 != closestWaypoint2)
        {
            // Set projectedObject to closestWaypoint1 to avoid any sudden jumps
            projectedObject.position = closestWaypoint1.position;
        }
    }

    private void RailLockOn()
    {
        Vector3 direction = closestWaypoint2.position - closestWaypoint1.position;
        Vector3 directionNormalized = direction.normalized;

        Vector3 playerPositionRelativeToWP1 = player.position - closestWaypoint1.position;

        float projectionDistance = Vector3.Dot(playerPositionRelativeToWP1, directionNormalized);
        projectionDistance = Mathf.Clamp(projectionDistance, 0f, direction.magnitude);

        projectedPosition = closestWaypoint1.position + projectionDistance * directionNormalized;

        projectedObject.position = projectedPosition;

        Vector3 playerToProjected = projectedPosition - player.position;
        float angle = Vector3.SignedAngle(player.forward, playerToProjected, Vector3.up);
        player.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(player.position, closestWaypoint1.position);
        Gizmos.DrawLine(player.position, closestWaypoint2.position);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(player.position, projectedPosition);
    }
}
