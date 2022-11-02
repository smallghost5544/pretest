using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    public LayerMask Wall;
    public LayerMask Ground;
    public float wallRunForce;
    public float wallruntimelimit;
    float wallruntimer;

    private float h;
    private float v;

    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftwallhit;
    private RaycastHit rightwallhit;
    private bool leftwall;
    private bool rightwall;

    public Transform orientation;
    private CharacterMove characterMove;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterMove = GetComponent<CharacterMove>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckWall();
    }

    void CheckWall()
    {
        rightwall = Physics.Raycast(transform.position, orientation.right, out rightwallhit, wallCheckDistance, Wall);
        leftwall = Physics.Raycast(transform.position, -orientation.right, out leftwallhit, wallCheckDistance, Wall);

    }
    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, Ground);
    }

    private void stateMachine()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if((leftwall|| rightwall) && v > 0 && AboveGround())
         {
        }
    }
}
