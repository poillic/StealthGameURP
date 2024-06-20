using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public float radius = 10f;

    private void OnValidate()
    {
        Sphere();    
    }

    [ContextMenu("Sphere")]
    public void Sphere()
    {
        foreach ( Transform child in transform )
        {
            float theta = Mathf.Acos( child.localPosition.z / radius );
            float phi = Mathf.Atan2( child.localPosition.y, child.localPosition.x );
            float y = radius * Mathf.Sin( theta ) * Mathf.Sin( phi );

            Debug.Log( $"{child.name} - theta : {child.localPosition.z / radius}, phi {phi}, y {y}" );

            Vector3 localPos = new Vector3( child.localPosition.x, y, child.localPosition.z );
            child.localPosition = localPos;
        }
    }

    private void Reset()
    {
        foreach ( Transform child in transform )
        {
            Vector3 localPos = new Vector3( child.localPosition.x, 0f, child.localPosition.z );
            child.localPosition = localPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere( transform.position, radius );
    }
}
