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

    [Header("Animations")]
    SpriteRenderer spriteRenderer;
    private bool isFacingRight;

    public enum gravityState
    {
        Normal,
        Climbing
    }

    void Awake()
    {
        Setup();
    }

    void Setup()
    {
        movement = BasicMovement;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }


    private void Update()
    {
        Timer();
        X = GatherInputX();
        Y = GatherInputY();


        if (isClimbing)
            movement();

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
        if(!isClimbing)
            movement();
        CheckCollisions();
    }

    public void SetCanMove(bool state)
    {
        canMove = state;
    }

    public void CanMove(bool state)
    {
        canMove = state;
    }

    public void SetCanClimb(bool state, bool isdownside, bool isupside)
    {
        if (!state)
        {
            StopClimbing();
        }
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
        if (!canMove || isClimbing)
            return;

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
        transform.position += direction.normalized * playerVars.climbingSpeed * Time.deltaTime;
    }

    void StartClimbing()
    {
        movement = ClimbingMovement;
        isClimbing = true;
        rb.velocity = Vector2.zero;
        SwitchGravity(gravityState.Climbing);
        ChangeCollisionRelation(true);
        Debug.Log("Start climbing");

    }

    void StopClimbing()
    {
        SwitchGravity(gravityState.Normal);
        ChangeCollisionRelation(false);
        isClimbing = false;
        movement = BasicMovement;
        Debug.Log("Stop climbing");
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
            //playerAnims.ChangeAnimationState(AnimationState.Jump_Player.ToString());
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * playerVars.jumpForce, ForceMode2D.Impulse);
            lastJumpPressed = 0f;
        }
    }

    #endregion
    int GatherInputX()
    {
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
        else if (Input.GetKey(KeyCode.S))
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
