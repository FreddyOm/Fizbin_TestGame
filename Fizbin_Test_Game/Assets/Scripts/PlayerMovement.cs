using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Names of the inputs
    enum InputKeys
    {
        left,
        right,
        jump,
        run
    }

    /* 
     *Corresponding keyCodes for the inputs
     *Has to be the same order as "InputKeys"
     */
    private KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.A,              //left
        KeyCode.D,              //right
        KeyCode.Space,          //jump
        KeyCode.LeftShift       //run
    };

    // Variables
    private Rigidbody2D playerRigidbody;
    protected int groundLayer = 8;
    [Header("Movement")]
    [Tooltip("The acceleration of the player when walking.")]
    public float walkAcceleration = 20f;
    [Tooltip("The acceleration of the player when running.")]
    public float runAcceleration = 7f;
    [Tooltip("The maximum velocity, the player can have, when walking.")]
    public float maxPlayerWalkVelocity = 3f;
    [Tooltip("The maximum velocity, the player can have, when running.")]
    public float maxPlayerRunVelocity = 4f;
    [Tooltip("The maximum velocity cahnge that should be applied.")]
    public float maxDecelerationDelta = .2f;
    bool isRunning = false;
    float horizontalMovement = 0;
    [Header("Jumping")]
    [Tooltip("When true, player can jump. When false, player cannot jump.")]
    public bool canJump = true;
    [Tooltip("The force applied to the player in vertical direction when jumping.")]
    public float jumpForce = 10f;
    bool isGrounded = false;

    //Start
    void Start()
    {
        playerRigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Read Inputs every frame
    void Update()
    {
        if (isGrounded)
        {
            InputDetection();
        }
    }

    //Movement and Deceleration
    private void FixedUpdate()
    {
        //horizontalMovement without running
        if(horizontalMovement != 0 && isGrounded && !isRunning && Mathf.Abs(playerRigidbody.velocity.x) < maxPlayerWalkVelocity)
        {
            playerRigidbody.AddForce(new Vector2(horizontalMovement * Time.fixedDeltaTime * 100, 0), ForceMode2D.Force);
        }
        else if (horizontalMovement != 0 && isGrounded && isRunning && Mathf.Abs(playerRigidbody.velocity.x) < maxPlayerRunVelocity)
        {
            playerRigidbody.AddForce(new Vector2(horizontalMovement * Time.fixedDeltaTime * 100, 0), ForceMode2D.Force);
        }
        else if (horizontalMovement == 0 && isGrounded)
        {
            //Decelerate if there is no input
            Decelerate();
        }
    }
    public void InputDetection()
    {
        float movement = 0;

        //Check for Jump Input
        if (Input.GetKey(keyCodes[(int)InputKeys.jump]) && canJump && isGrounded)
        {
            //Jump
            Jump();
        }

        //Check for horizontal movement
        if (Input.GetKey(keyCodes[(int)InputKeys.left]) && isGrounded)
        {
            //Move left
            if (!isRunning)
            {
                movement += -walkAcceleration;
            }
            else
            {
                movement += -runAcceleration;
            }
        }
        if (Input.GetKey(keyCodes[(int)InputKeys.right]) && isGrounded)
        {
            //Move right
            if (!isRunning)
            {
                movement += walkAcceleration;
            }
            else
            {
                movement += runAcceleration;
            }
        }

        //Check if user pressed the key associated with "run"
        if (Input.GetKey(keyCodes[(int)InputKeys.run]) && isGrounded)
        {
            //Run
            isRunning = true;
        }else if(!Input.GetKey(keyCodes[(int)InputKeys.run]))
        {
            isRunning = false;
        }

        //Set new horizontal movement value
        horizontalMovement = movement;

    }

    //Move current horizontal velocity to zero
    void Decelerate()
    {
        playerRigidbody.velocity = new Vector2(Mathf.MoveTowards(playerRigidbody.velocity.x, 0, maxDecelerationDelta), playerRigidbody.velocity.y);
    }

    //Reset vertical player velocity and add vrtical force for jumping
    void Jump()
    {
        //Jump
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
        playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }


    //Checks, if player is on the ground
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.gameObject.layer == groundLayer)
        {
            isGrounded = true;
        }
    }

    //Checks, if player is in the air
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }
    }
}
