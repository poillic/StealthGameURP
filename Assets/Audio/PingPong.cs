using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    [Tooltip("Time to travel from Start to End")]
    public float pingDuration;
    public Transform startPoint;
    public Transform endPoint;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp( startPoint.position, endPoint.position, Mathf.PingPong( Time.timeSinceLevelLoad, pingDuration ) / pingDuration );
    }
}
