using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public delegate void Movement();
    Movement movement;
    Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] PlayerMovementSO playerVars;
    int X;
    int Y;
    bool canMove = true;

    [Header("Jump")]
    float lastJumpPressed;

    [Header("Ground Collisions")]
    [SerializeField] float groundRayLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float rayOffset;
    [SerializeField] Vector3 groundRayPosition;
    [SerializeField] bool isGrounded;
    private Vector3 groundRayoffset;

    [Header("Climbing")]
    bool canClimb;
    private bool isClimbing;
    [SerializeField]bool isDownside;
    [SerializeField]bool isUpside;
    bool isFinalLadder;

    [Header("Animations")]
    SpriteRenderer spriteRenderer;
    Animator animator;
    private bool isFacingRight;
    public bool isHit = false;
    public bool isInTransition;
    private string currentState;
    bool isRepairing;

    PlayerSound sound;

    public enum gravityState
    {
        Normal,
        Climbing
    }

    void Awake()
    {
        Cursor.visible = false;
        Setup();
    }

    void Setup()
    {
        sound = GetComponent<PlayerSound>();
        movement = BasicMovement;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;
        isFacingRight = true;
    }


    private void Update()
    {
        Timer();
        X = GatherInputX();
        Y = GatherInputY();

        if (isClimbing && !isHit) ChangeAnimationState("Player_Climb");

        if (!canMove || isClimbing)
            return;
            
        DetectSideOfAnimation();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            lastJumpPressed = playerVars.jumpBuffer;

        Jump();
    }
    void FixedUpdate()
    {
        if((canClimb && Y > 0 && !isClimbing && isDownside) || (canClimb && Y < 0 && !isClimbing && isUpside))
            StartClimbing();
        if (isGrounded && Y < 0 && isClimbing && isDownside)
            StopClimbing();

        movement();
        CheckCollisions();
    }
    public void SetIsHit(bool state)
    {
        isHit = state;
    }
    public void SetCanMove(bool state)
    {
        canMove = state;
    }
    public void setIsRepairing(bool state)
    {
        isRepairing = state;
    }
    public void SetCanClimb(bool state, bool isdownside, bool isupside, bool isFinal)
    {
        if (!state)
        {
            StopClimbing();
        }
        isFinalLadder = isFinal;
        canClimb = state;
        isDownside = isdownside;
        isUpside = isupside;
    }

    public void SetClimbingBools(bool isdownside, bool isupside)
    {
        isDownside = isdownside;
        isUpside = isupside;
    }

    private void BasicMovement()
    {
        if (!canMove || isClimbing || isHit)
            return;

        if (X != 0 && rb.velocity.y <= 0f && isGrounded && !isRepairing)
            ChangeAnimationState("Player_Run");
        else if(X == 0 && rb.velocity.y <= 0f && isGrounded && !isRepairing)
            ChangeAnimationState("Player_Idle");

        float maxSpeed = X * playerVars.movementSpeed;

        float baseVelocity = rb.velocity.x;

        float speedDif = maxSpeed - baseVelocity;

        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? playerVars.acceleration : playerVars.decceleration;

        float movement = (Mathf.Abs(speedDif) * accelRate) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }

    void ClimbingMovement()
    {
        Vector3 direction = new Vector2(X, Y);
        rb.MovePosition(transform.position + direction.normalized * playerVars.climbingSpeed);
    }

    void StartClimbing()
    {
        movement = ClimbingMovement;
        isClimbing = true;
        rb.velocity = Vector2.zero;
        SwitchGravity(gravityState.Climbing);
        ChangeCollisionRelation(!isFinalLadder);
    }

    void StopClimbing()
    {
        SwitchGravity(gravityState.Normal);
        ChangeCollisionRelation(false);
        isClimbing = false;
        movement = BasicMovement;
        isInTransition = false;
    }

    void ChangeCollisionRelation(bool state)
    {
        Physics2D.IgnoreLayerCollision(6, 3, state);
    }

    #region Jump
    private void Jump()
    {
        if (lastJumpPressed > 0 && isGrounded /* Jump Buffer */)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * playerVars.jumpForce, ForceMode2D.Impulse);
            lastJumpPressed = 0f;
            sound.AudioJump();
            Debug.Log("jump man");
            ChangeAnimationState("Player_Jump");
        }
    }



    #endregion

    int GatherInputX()
    {
        if (isInTransition || isRepairing)
            return 0;
        int acc = 0;
        if (Input.GetKey(KeyCode.A))
            acc = -1;
        else if (Input.GetKey(KeyCode.D))
            acc = 1;
        return acc;
    }
    int GatherInputY()
    {
        int acc = 0;
        if (Input.GetKey(KeyCode.W))
            acc = 1;
        else if (Input.GetKey(KeyCode.S) && !isInTransition)
            acc = -1;
        return acc;
    }


    #region Raycast

    void CheckCollisions()
    {
        //Ground Collision
        isGrounded = Physics2D.Raycast((transform.position + groundRayPosition), Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast((transform.position + groundRayPosition) - (2 * groundRayoffset), Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast((transform.position + groundRayPosition) - groundRayoffset, Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast((transform.position + groundRayPosition) + (2 * groundRayoffset), Vector3.down, groundRayLength, groundLayer) ||
            Physics2D.Raycast((transform.position + groundRayPosition) + groundRayoffset, Vector3.down, groundRayLength, groundLayer);
    }

    #endregion

    #region Hit

    public void PlayerHit()
    {
        SetIsHit(true);
        SetCanMove(false);
        ChangeAnimationState("Player_Hit");
    }


    #endregion


    #region Animations

    void DetectSideOfAnimation()
    {
        if (X < 0f && isFacingRight)
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
        }
        else if (X > 0f && !isFacingRight)
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        Debug.Log("Changed to" + newState);
        animator.Play(newState);
        currentState = newState;
    }

    #endregion


    #region Gizmos
    private void OnDrawGizmos()
    {
        groundRayoffset = new Vector3(rayOffset, 0f, 0f);
        //Ground Rays
        Gizmos.DrawRay((transform.position + groundRayPosition), Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) - groundRayoffset, Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) - (2 * groundRayoffset), Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) + (2 * groundRayoffset), Vector3.down * groundRayLength);
        Gizmos.DrawRay((transform.position + groundRayPosition) + groundRayoffset, Vector3.down * groundRayLength);
    }

    #endregion


    #region Timer

    void Timer()
    {
        JumpBufferTimer();
        TouchGroundScript();
    }

    void TouchGroundScript()
    {
        if (isGrounded && !isClimbing)
        {
            movement = BasicMovement;
            SwitchGravity(gravityState.Normal);
        }
    }
    void JumpBufferTimer()
    {
        if (lastJumpPressed > 0)
            lastJumpPressed -= Time.deltaTime;
    }

    void SwitchGravity(gravityState state)
    {
        switch (state)
        {
            case gravityState.Normal:
                {
                    rb.gravityScale = playerVars.generalGravity;
                    //GravityState = gravityState.Normal;
                    break;
                }
            case gravityState.Climbing:
                {
                    rb.gravityScale = 0f;
                    //GravityState = gravityState.Climbing;
                    break;
                }
        }
    }

    #endregion

}
