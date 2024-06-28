using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bbb : MonoBehaviour
{
    public Transform hint;
    public float length = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt( hint );

        /*Ray c = new Ray( Camera.main.transform.position, Camera.main.transform.forward );

        if( Physics.Raycast( c, out RaycastHit chit ) )
        {
            hint.position = chit.point;

            float normal_dot = Vector3.Dot( chit.normal, Vector3.up );
            if ( -0.25f < normal_dot && normal_dot < 0.25f )
            {
                Vector3 o = Camera.main.transform.position + Camera.main.transform.forward * ( chit.distance + 0.5f );
                o.y = chit.point.y + 3f;
                Ray r = new Ray( o, Vector3.down );
                if( Physics.Raycast( r, out RaycastHit hit ) )
                {
                    hint.position = hit.point;
                }
            }
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( transform.position + transform.forward * 10f, 1f );
    }
}
