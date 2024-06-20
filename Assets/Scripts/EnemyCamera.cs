using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : MonoBehaviour
{
    public Transform startPatrol;
    public Transform endPatrol;

    public float timer = 1f;

    public CameraState currentState;
    public enum CameraState
    {
        PATROL, CHASE, RESET, INACTIVE
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = CameraState.PATROL;
    }

    // Update is called once per frame
    void Update()
    {
        switch ( currentState )
        {
            case CameraState.PATROL:

                Vector3 lookAtPosition = Vector3.Lerp( startPatrol.position, endPatrol.position, Mathf.PingPong( Time.time, timer ) / timer );
                transform.LookAt( lookAtPosition );

                break;
            case CameraState.CHASE:
                break;
            case CameraState.RESET:
                break;
            case CameraState.INACTIVE:
                break;
            default:
                break;
        }
    }
}
