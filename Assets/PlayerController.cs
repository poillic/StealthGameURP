using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header( "Actions" )]
    public InputAction _moveAction;
    public InputAction _runAction;
    public InputAction _jumpAction;
    public InputAction _sneakAction;

    public Rigidbody _rb;

    [Header( "Check Ground" )]
    public Transform checkGroundTransform;
    public Vector3 checkGroundDimension;
    public LayerMask groundLayer;

    public PlayerState currentState;
    public enum PlayerState
    {
        IDLE, JOG, RUN, SNEAK, FALL, JUMP
    }

    private bool isGrounded = true;
    private Vector2 moveDirection;

    private void OnEnable()
    {
        _moveAction.Enable();
        _runAction.Enable();
        _jumpAction.Enable();
        _sneakAction.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = _moveAction.ReadValue<Vector2>();

        CheckGround();
        OnStateUpdate();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector3( moveDirection.x * 5f, _rb.velocity.y, moveDirection.y * 5f );
    }

    public void CheckGround()
    {
        Collider[] bodies = Physics.OverlapBox( checkGroundTransform.position, checkGroundDimension,Quaternion.identity, groundLayer );

        isGrounded = bodies.Length > 0;

    }

    private void OnDrawGizmos()
    {
        if( isGrounded )
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawCube( checkGroundTransform.position, checkGroundDimension );
    }

    public void OnStateEnter()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.JOG:
                break;
            case PlayerState.RUN:
                break;
            case PlayerState.SNEAK:
                break;
            case PlayerState.FALL:
                break;
            case PlayerState.JUMP:
                _rb.AddForce( Vector3.up * 5f, ForceMode.Impulse );
                break;
            default:
                break;
        }
    }

    public void OnStateUpdate()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:
                if ( !isGrounded && _rb.velocity.y < 0f )
                {
                    TransitionToState( PlayerState.FALL );
                }
                else if( _runAction.WasPerformedThisFrame() && moveDirection.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
                else if ( _jumpAction.WasPerformedThisFrame() )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if( _sneakAction.WasPerformedThisFrame() )
                {
                    TransitionToState( PlayerState.SNEAK );
                }
                break;
            case PlayerState.JOG:

                if ( _runAction.WasPerformedThisFrame() && moveDirection.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
                else if ( _jumpAction.WasPerformedThisFrame() )
                {
                    TransitionToState( PlayerState.JUMP );
                }

                break;
            case PlayerState.RUN:

                if( moveDirection.magnitude == 0f )
                {
                    TransitionToState( PlayerState.IDLE );
                }
                else
                {
                    if( _runAction.WasReleasedThisFrame() )
                    {
                        TransitionToState( PlayerState.JOG );
                    }
                }

                if ( _jumpAction.WasPerformedThisFrame() )
                {
                    TransitionToState( PlayerState.JUMP );
                }

                break;
            case PlayerState.SNEAK:

                if( _sneakAction.WasPerformedThisFrame() )
                {
                    if( moveDirection.magnitude > 0f )
                    {
                        TransitionToState( PlayerState.JOG );
                    }
                    else
                    {
                        TransitionToState( PlayerState.IDLE );
                    }
                }

                break;
            case PlayerState.FALL:

                if( isGrounded )
                {
                    if( moveDirection.magnitude > 0f )
                    {
                        TransitionToState( PlayerState.JOG );
                    }
                    else
                    {
                        TransitionToState( PlayerState.IDLE );
                    }
                }

                break;
            case PlayerState.JUMP:

                if( !isGrounded && _rb.velocity.y < 0f )
                {
                    TransitionToState( PlayerState.FALL );
                }

                break;
            default:
                break;
        }
    }

    public void OnStateExit()
    {

    }

    public void TransitionToState( PlayerState newState )
    {

    }


}
