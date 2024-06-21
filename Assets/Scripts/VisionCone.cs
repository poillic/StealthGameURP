using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public int sides = 16;
    public float length = 10f;
    public float radius = 4f;

    public LayerMask collisionLayer;

    public MeshFilter viewMeshFilter;
    public MeshCollider viewCollider;
    Mesh viewMesh;
    // Start is called before the first frame update
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "Cone";
        viewMeshFilter.mesh = viewMesh;

        viewCollider.sharedMesh = viewMesh;
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 centerPoint = transform.position + transform.forward * length;

        if ( Physics.Raycast( transform.position, transform.forward, out RaycastHit chit, length, collisionLayer ) )
        {
            centerPoint = chit.point;
        }

        Debug.DrawLine( centerPoint, centerPoint + Vector3.up * 0.1f, Color.red );

        /*Vector3[] */ vertices = new Vector3[sides+1];
        vertices[ 0 ] = Vector3.zero;
         triangles = new int[ (sides) * 3 ];
        Debug.Log( triangles.Length );

        float step = ( 2f * Mathf.PI ) / (float) sides;

        for ( int i = 0; i < sides; i++ )
        {
            //Debug.Log( i );
            Vector3 y = transform.up * Mathf.Sin( i * step ) *radius;
            Vector3 x = transform.right * Mathf.Cos( i * step ) *radius;
            Vector3 z = transform.forward * length;
            Vector3 pointPosition = x + y + z;

            if ( Physics.Raycast( transform.position, pointPosition, out RaycastHit hit, length, collisionLayer ) )
            {
                vertices[i+1] = transform.InverseTransformPoint( hit.point );
            }
            else
            {
                Debug.Log( $"{i} - {pointPosition}"  );
                vertices[i+1] = transform.InverseTransformPoint( transform.position + pointPosition );
            }

            if ( i < sides - 1 )
            {
                triangles[ i * 3 ] = i + 2;
                triangles[ i * 3 + 2 ] = 0;
                triangles[ i * 3 + 1 ] = i + 1;
            }else if( i == sides - 1 )
            {
                triangles[ i * 3 ] = 1;
                triangles[ i * 3 + 2 ] = 0;
                triangles[ i * 3 + 1 ] = i + 1;
            }
        }
        
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
        viewCollider.sharedMesh = viewMesh;
    }

    private void OnDrawGizmos()
    {
        float step = (2f * Mathf.PI) / (float)sides;

        for ( int i = 0; i < sides; i++ )
        {
            Vector3 y = transform.up * Mathf.Sin( i * step ) *radius;
            Vector3 x = transform.right * Mathf.Cos( i * step ) * radius;
            Vector3 z = transform.forward * length;
            Vector3 pointPosition = x + y + z;
            Gizmos.color = Color.white;
            Gizmos.DrawSphere( transform.position + pointPosition, 0.1f );
            Gizmos.color = Color.green;
            Gizmos.DrawRay( transform.position, pointPosition );
        }
    }

    public Vector3[] vertices;
    public int[] triangles;
}
