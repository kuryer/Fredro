using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    public delegate void Movement();
    Movement movement;
    bool isRepairing;
    // Start is called before the first frame update
    void Awake()
    {
        Setup();
    }

    void Setup()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        
    }

    void FixedUpdate()
    {
        if (isRepairing)
            return;
        WalkingMovement();


    }
    #region Climbing
    
    void ClimbingMovement()
    {

    }

    #endregion
    void WalkingMovement()
    {
        var position = rb.position + Vector2.right * GatherInput() * movementSpeed;
        rb.MovePosition(position);
    }
    void Jump()
    {
        rb.AddForce(Vector2.up * 100f * jumpForce);

    }
    int GatherInput()
    {
        int acc = 0;
        if (Input.GetKey(KeyCode.A))
            acc = -1;
        else if (Input.GetKey(KeyCode.D))
            acc = 1;
        return acc;
    }
}
