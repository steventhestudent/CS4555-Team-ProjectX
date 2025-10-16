using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Create a ray from the camera to the mouse postion
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Check if the ray hits the ground(NavMesh)
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, UnityEngine.AI.NavMesh.AllAreas))
            {
                //Move the agent to the clicked postion
                navAgent.SetDestination(hit.point);
            }
        }
    }
}
