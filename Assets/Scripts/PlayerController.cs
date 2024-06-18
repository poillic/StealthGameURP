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

    public PlayerDataSO playerDatas;
    public Rigidbody _rb;
    public Animator _animator;

    [Header( "Ground Position" )]
    public float rayLength;

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
    private float _currentSpeed;

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

        _animator.SetFloat( "X", moveDirection.x * _currentSpeed / playerDatas.runSpeed );
        _animator.SetFloat( "Y", moveDirection.y * _currentSpeed / playerDatas.runSpeed );
        /* BREATH OF THE WILD */
        //_animator.SetFloat( "X", 0f );
        //_animator.SetFloat( "Y", moveDirection.magnitude * _currentSpeed / playerDatas.runSpeed );
        _animator.SetFloat( "velocityY", _rb.velocity.y );
        _animator.SetBool( "isGrounded", isGrounded );

        CheckGround();
        OnStateUpdate();
    }

    private void FixedUpdate()
    {
        Vector3 CameraForward = new Vector3( Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z ).normalized;
        Vector3 moveRightLeft = Camera.main.transform.right * moveDirection.x;
        Vector3 moveForwardBackward = CameraForward * moveDirection.y;
        Vector3 direction = moveRightLeft + moveForwardBackward;

        Vector3 vel = new Vector3( direction.x * _currentSpeed, _rb.velocity.y, direction.z * _currentSpeed );

        if ( isGrounded && _rb.velocity.y <= 0f )
        {
            _rb.useGravity = false;
            vel.y = 0f;
        }
        else
        {
            _rb.useGravity = true;
            vel.y = _rb.velocity.y;
        }


        if ( moveDirection.magnitude > 0f )
        {
            transform.forward = CameraForward;
            /* BREATH OF THE WILD */
            //transform.forward = direction;
        }
        _rb.velocity = vel;
    }

    public void CheckGround()
    {
        Collider[] bodies = Physics.OverlapBox( checkGroundTransform.position, checkGroundDimension,Quaternion.identity, groundLayer );

        isGrounded = bodies.Length > 0;
    }

    public void StickToGround()
    {
        if ( Physics.Raycast( transform.position, Vector3.down, out RaycastHit hit, rayLength, groundLayer ) )
        {
            _rb.MovePosition( hit.point + new Vector3( 0f, rayLength, 0f ) );
        }
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

        Gizmos.DrawCube( checkGroundTransform.position, checkGroundDimension*2f );

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay( transform.position, Vector3.down * rayLength );
    }

    public void OnStateEnter()
    {
        switch ( currentState )
        {
            case PlayerState.IDLE:
                _currentSpeed = 0f;
                break;
            case PlayerState.JOG:
                _currentSpeed = playerDatas.jogSpeed;
                
                break;
            case PlayerState.RUN:
                _currentSpeed = playerDatas.runSpeed;
                break;
            case PlayerState.SNEAK:
                break;
            case PlayerState.FALL:
                break;
            case PlayerState.JUMP:
                _rb.AddForce( Vector3.up * playerDatas.jumpForce, ForceMode.Impulse );
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

                StickToGround();

                if ( moveDirection.magnitude > 0f )
                {
                    TransitionToState( PlayerState.JOG );
                }
                else if( !isGrounded && _rb.velocity.y < 0f )
                {
                    TransitionToState( PlayerState.FALL );
                }
                else if( _runAction.WasPerformedThisFrame() && moveDirection.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
                else  if ( _jumpAction.WasPerformedThisFrame() )
                {
                    TransitionToState( PlayerState.JUMP );
                }
                else if( _sneakAction.WasPerformedThisFrame() )
                {
                    TransitionToState( PlayerState.SNEAK );
                }
                break;
            case PlayerState.JOG:

                StickToGround();

                if ( _runAction.WasPerformedThisFrame() && moveDirection.magnitude > 0f )
                {
                    TransitionToState( PlayerState.RUN );
                }
                else if ( _jumpAction.WasPerformedThisFrame() )
                {
                    TransitionToState( PlayerState.JUMP );
                }else if( moveDirection.magnitude == 0F )
                {
                    TransitionToState( PlayerState.IDLE );
                }

                break;
            case PlayerState.RUN:

                StickToGround();

                if ( moveDirection.magnitude == 0f )
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
                StickToGround();
                if ( _sneakAction.WasPerformedThisFrame() )
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
                else if ( isGrounded )
                {
                    if ( moveDirection.magnitude > 0f )
                    {
                        TransitionToState( PlayerState.JOG );
                    }
                    else
                    {
                        TransitionToState( PlayerState.IDLE );
                    }
                }

                break;
            default:
                break;
        }
    }

    public void OnStateExit()
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
                break;
            default:
                break;
        }
    }

    public void TransitionToState( PlayerState newState )
    {
        OnStateExit();
        currentState = newState;
        OnStateEnter();
    }


}
