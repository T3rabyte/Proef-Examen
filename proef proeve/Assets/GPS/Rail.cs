using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField]
    Transform closestWaypoint1;
    [SerializeField]
    Transform closestWaypoint2;
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform desiredLocation;

    [SerializeField]
    float distanceBC;
    [SerializeField]
    float distanceAC;
    [SerializeField]
    float distanceAB;
    [SerializeField]
    float angleCAB;
    [SerializeField]
    float angleACB;
    [SerializeField]
    float angleCAD;
    [SerializeField]
    float distanceDC;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        distanceBC = Vector2.Distance(closestWaypoint1.position, closestWaypoint2.position);
        distanceAC = Vector2.Distance(closestWaypoint1.position, player.position);
        distanceAB = Vector2.Distance(player.position, closestWaypoint2.position);
        angleCAB = Mathf.Atan2(distanceAC, distanceAB) * Mathf.Rad2Deg;
        angleACB = Mathf.Atan2(distanceAC, distanceBC) * Mathf.Rad2Deg;
        angleCAD = 180f - angleACB - 90f;

        distanceDC = distanceAC * Mathf.Sin(angleCAD);

        Debug.Log(distanceDC);


        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(player.position, closestWaypoint1.position);
        Gizmos.DrawLine(player.position, closestWaypoint2.position);
    }
}
