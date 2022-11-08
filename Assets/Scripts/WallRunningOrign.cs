using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunningOrign : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;
    public float wallrotate = 15f;
    bool exitingWall;
    float exitWalltime = 0.2f;
    float exitwalltimer;

    [Header("Jump")]
    public KeyCode jumpkey = KeyCode.Space;
    public float walljumpforce = 7f;
    public float walljumpslideforce = 12f;
    Vector3 forcetoApply;
    bool inwalljump = false;

    [Header("Input")]
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    public GameObject rotatecam;
    public CharacterMove pm;
    //private PlayerMovementTutorial pm;
    public Rigidbody rb;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //pm = GetComponent<CharacterMove>();
        //pm = GetComponent<PlayerMovementTutorial>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
        if (Input.GetMouseButton(1)) Dash();
        if (inwalljump)
        {
            transform.position = Vector3.Lerp(transform.position, forcetoApply, Time.deltaTime );
            Debug.Log("Lerp");
            if (transform.position.magnitude - forcetoApply.magnitude < 0.5f)
                inwalljump = false;
        }

    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
 
      
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();
            if (Input.GetKeyDown(jumpkey))
            {
                WallJump();
                inwalljump = true;
            }
        }
        else if (exitingWall)
        {
            if (pm.wallrunning) StopWallRun();
            if (exitWalltime > 0) exitWalltime -= Time.deltaTime;
            if (exitWalltime < 0) exitingWall= false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        wallRunTimer = maxWallRunTime;
    }

    private void WallRunningMovement()
    {
        //rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(wallForward * wallRunForce* Time.deltaTime, ForceMode.Force);

        rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        //if (downwardsRunning)
        //    rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        // push to wall force
        //if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        //    rb.AddForce(-wallNormal * 1, ForceMode.Force);
    }

    public void WallJump()
    {
        exitingWall = true;
        exitwalltimer = exitWalltime;
        Vector3 wallnormal = wallRight ? rightWallhit.normal : leftWallhit.normal; //success
        forcetoApply = transform.up * walljumpforce + wallnormal * walljumpslideforce;
        //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //rb.AddForce(forcetoApply *Time.deltaTime, ForceMode.Impulse);
    }


    private void Dash()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce((transform.forward * 10f), ForceMode.Impulse);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        rb.useGravity = true;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 7)
    //        pm.wallrunning = true;
    //}
}
