using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] float detectionLimit = 2f;
    [SerializeField] List<Transform> pathPoints;
    public int startIndex = 2;
    private int pointIndex = 0;

    public enum PatrolType
    {
        LOOP,PINGPONG
    }
    public PatrolType patrolType;
    private bool reverseIndex = false;
    private bool isWaiting = false;
    // Start is called before the first frame update
    void Start()
    {
        pointIndex = Mathf.Clamp(startIndex, 0, pathPoints.Count -1 );

        agent.SetDestination( pathPoints[ pointIndex ].position );
    }

    // Update is called once per frame
    void Update()
    {
        if( agent.remainingDistance <= detectionLimit && !isWaiting )
        {
            switch ( patrolType )
            {
                case PatrolType.LOOP:
                    pointIndex++;
                    pointIndex = pointIndex % ( pathPoints.Count );
                    StartCoroutine( Wait() );
                    break;
                case PatrolType.PINGPONG:

                    if( reverseIndex )
                    {
                        pointIndex--;

                        if( pointIndex <= 0 )
                        {
                            reverseIndex = false;
                        }
                    }
                    else
                    {
                        pointIndex++;

                        if( pointIndex >= pathPoints.Count - 1 )
                        {
                            reverseIndex = true;
                        }
                    }
                    //agent.SetDestination( pathPoints[ pointIndex ].position );
                    StartCoroutine( Wait() );
                    break;
                default:
                    break;
            }

        }
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds( 0.5f );
        agent.SetDestination( pathPoints[ pointIndex ].position );
        isWaiting = false;
    }
}
