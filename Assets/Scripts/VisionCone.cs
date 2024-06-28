using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [Tooltip("Tags that will trigger the Detected method")]
    public List<string> itemTags;
    public GameObject detectedObject;
    public UnityEvent<GameObject> OnDetected;
    public bool objectFound = false;

    public int sides = 16;
    public float length = 10f;
    public float radius = 4f;

    public LayerMask collisionLayer;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    // Start is called before the first frame update
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "Cone";
        viewMeshFilter.mesh = viewMesh;
    }

    // Update is called once per frame
    void Update()
    {
        detectedObject = null;
        objectFound = false;

        Vector3 centerPoint = transform.forward * length;
        vertices = new Vector3[ sides + 2 ];
        vertices[ 0 ] = Vector3.zero;
        vertices[ vertices.Length - 1 ] = transform.InverseTransformPoint( transform.position + centerPoint );

        if ( Physics.Raycast( transform.position, transform.forward, out RaycastHit chit, length, collisionLayer ) )
        {
            vertices[ vertices.Length - 1 ] = transform.InverseTransformPoint( chit.point );
            Detect( chit.collider );
        }

        triangles = new int[ (sides) * 3 * 2 ];

        float step = ( 2f * Mathf.PI ) / (float) sides;

        for ( int i = 0; i < sides; i++ )
        {
            Vector3 y = transform.up * Mathf.Sin( i * step ) *radius;
            Vector3 x = transform.right * Mathf.Cos( i * step ) *radius;
            Vector3 z = transform.forward * length;
            Vector3 pointPosition = x + y + z;

            if ( Physics.Raycast( transform.position, pointPosition, out RaycastHit hit, length, collisionLayer ) )
            {
                vertices[i+1] = transform.InverseTransformPoint( hit.point );
                Detect( hit.collider );
            }
            else
            {
                vertices[i+1] = transform.InverseTransformPoint( transform.position + pointPosition );
            }

            if ( i < sides - 1 )
            {
                triangles[ i * 6 ] = i + 2;
                triangles[ i * 6 + 1 ] = i + 1;
                triangles[ i * 6 + 2 ] = 0;
                triangles[ i * 6 + 3 ] = i + 1;
                triangles[ i * 6 + 4 ] = i + 2;
                triangles[ i * 6 + 5 ] = vertices.Length - 1;
            }
            else if( i == sides - 1 )
            {

                triangles[ i * 6 ] = 1;
                triangles[ i * 6 + 1 ] = i + 1;
                triangles[ i * 6 + 2 ] = 0;
                triangles[ i * 6 + 3 ] = i + 1;
                triangles[ i * 6 + 4 ] = 1;
                triangles[ i * 6 + 5 ] = vertices.Length - 1;
            }
        }
        
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
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

    private void Detect( Collider other )
    {
        if( itemTags.Contains( other.tag ) )
        {

            if( !objectFound )
            {
                detectedObject = other.gameObject;
                OnDetected.Invoke( detectedObject );
                objectFound = true;
            }
        }
    }

    public Vector3[] vertices;
    public int[] triangles;



}
