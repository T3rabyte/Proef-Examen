using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{

    [SerializeField]
    private Transform[] waypoints; 

    void Update()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < transform.childCount -1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
       //Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }
}
