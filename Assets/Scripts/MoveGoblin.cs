using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveGoblin : MonoBehaviour
{
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown( 0 ) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );


            if( Physics.Raycast( ray, out RaycastHit hit ) )
            {
                agent.SetDestination( hit.point );
                GameObject.CreatePrimitive( PrimitiveType.Sphere ).transform.position = hit.point;
            }
        }        
    }
}
