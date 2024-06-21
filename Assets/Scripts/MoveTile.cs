using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public Vector2 dimension;
    public float radius = 10f;

    private void OnValidate()
    {
        Sphere();
        Debug.Log( Mathf.Atan2( dimension.y, dimension.x ) );
    }

    [ContextMenu("Sphere")]
    public void Sphere()
    {
        foreach ( Transform child in transform )
        {
            float theta = Mathf.Acos( child.localPosition.z / radius );
            float phi = Mathf.Atan2( child.localPosition.y, child.localPosition.x );
            float y = radius * Mathf.Sin( theta ) * Mathf.Sin( phi );

            Debug.Log( $"{child.name} - theta : {theta}, phi {phi}, y {y}" );
            Debug.Log( $"{phi}, y {child.localPosition.y}, x {child.localPosition.x}" );

            Vector3 localPos = new Vector3( child.localPosition.x, y, child.localPosition.z );
            child.localPosition = localPos;
        }
    }

    [ContextMenu("Generate Cube")]
    public void Cube()
    {
        foreach ( Transform child in transform )
        {
            DestroyImmediate( child.gameObject );
        }

        for ( int x = 0; x < dimension.x; x++ )
        {
            for ( int y = 0; y < dimension.y; y++ )
            {
                GameObject go = GameObject.CreatePrimitive( PrimitiveType.Cube );
                go.transform.position = new Vector3( x - dimension.x / 2f, 0, y - dimension.y / 2f );
                go.transform.parent = transform;
                go.name = $"Cube{x}-{y}";
            }
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
