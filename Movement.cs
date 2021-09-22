using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    protected Rigidbody2D rb;

    // movement
    float x;
    bool jumping, crouching, sprinting;
    [Header("Movement")]
    public float speed = 10f;
    public bool isSliperry;

    //gravity
    private GameObject groundCheck;
    [Header("Gravity")]
    public LayerMask whatIsGround;
    float endOfYScale;
    bool isGrounded = false;
    public int gravityScale = 20;

    //Jumping
    [Header("Jump")]
    public float jumpForce = 150f;
    public KeyCode jumpInput = KeyCode.Space;

    //crouching
    [Header("Crouch")]
    public bool canCrouch = true;
    public KeyCode crouchInput = KeyCode.LeftShift;
    bool crouchUp;
    float defaultSize;
    float reducedSize;
    float speedWhileCrouching;

    //sprinting
    [Header("Sprint")]
    public bool canSprint = true;
    public KeyCode sprintInput = KeyCode.LeftControl;
    bool sprintDown, sprintUp;
    float fasterSpeed;
    float defaultSpeed;

    private void Start()
    {
        //rb help
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //other mechanics of the game 
        defaultSize = transform.localScale.y;
        reducedSize = transform.localScale.y / 2f;
        speedWhileCrouching = speed / 2;

        defaultSpeed = speed;
        fasterSpeed = speed * 2f;

        //for groundcheck
        groundCheckk();
    }

    private void Update()
    {
        myInput();

        movementHelp();
        rotation();

        jump();
        crouch();
        sprint();
    }

    private void myInput()
    {
        x = Input.GetAxisRaw("Horizontal");

        jumping = Input.GetKeyDown(jumpInput);

        crouching = Input.GetKey(crouchInput) && x > 0.5f || Input.GetKey(crouchInput) && x < -0.5f;
        crouchUp = Input.GetKeyUp(crouchInput);

        sprintDown = Input.GetKey(sprintInput) && x > 0.5f|| Input.GetKey(sprintInput) && x < -0.5f;
        sprintUp = Input.GetKeyUp(sprintInput);
    }

    private void groundCheckk()
    {
        //ground check
        GameObject check = new GameObject("Check");
        groundCheck = GameObject.Find("Check");
        endOfYScale = transform.lossyScale.y;
    }

    private void FixedUpdate()
    {
        movement();

        gravity();
    }

    /// <summary>
    /// Main movement
    /// </summary>
    public void movement()
    {
        float multiplerS = 750f;
        rb.AddForce(Vector2.right * x * speed * multiplerS * Time.fixedDeltaTime);
    }

    public void movementHelp()
    {
        float drag;

        if (!isSliperry)
        {
            drag = 6f;
        } else
        {
            drag = 0.52f;
        }

        rb.drag = drag;
        groundCheck.transform.position = new Vector2(transform.position.x, transform.position.y - endOfYScale);

        //is grounded system
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.00011f, whatIsGround);
    }

    /*Other mechanics*/
    private void jump()
    {
        float multiplerJ = 45f;

        if (isGrounded)
        {
            if (jumping)
            {
                rb.AddForce(Vector2.up * multiplerJ * jumpForce);
            }
        }
    }

    void gravity()
    {
        float multiplerG = 5f;

        rb.AddForce(Vector2.down * gravityScale * multiplerG);
    }

    /// <summary>
    /// rotation for
    /// your player
    /// </summary>
    public void rotation()
    {
        if (x == 1)
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        if (x == -1)
        {
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    //Crouching
    public void crouch()
    {
        if (isGrounded && canCrouch)
        {
            if (crouching)
            {
                startCrouching();
            }
            if (!crouching)
            {
                stopCrouching();
            }
            if (crouchUp)
            {
                speed = defaultSpeed;
            }
        }
    }

    private void startCrouching()
    {
        speed = speedWhileCrouching;
        transform.localScale = new Vector3(transform.localScale.x, reducedSize, transform.localScale.z);
    }
    public void stopCrouching()
    {
        transform.localScale = new Vector3(transform.localScale.x, defaultSize, transform.localScale.z);
    }

    //sprinting
    public void sprint()
    {
        if (isGrounded && canSprint)
        {
            if (sprintDown)
            {
                faster();
            }
            if (sprintUp)
            {
                normalSpeed();
            }
        }
    }

    private void faster()
    {
        speed = fasterSpeed;
    }
    private void normalSpeed()
    {
        speed = defaultSpeed;
    }
}
