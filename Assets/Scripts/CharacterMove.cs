using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{   
    public Rigidbody rb;
    public Transform head;
    public float speed = 10;
    public float jumpspeed = 30;
    public float Xsensitive = 1.5f;
    public float Ysensitive = 1f;
    public float wallrunspeed = 10f;
    float normalrunspeed;
    public bool wallrunning;
    Vector3 newvelocity;
    public bool isGround = true;
    public bool isJumping = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        normalrunspeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X")* Xsensitive);
        Move();
        if (wallrunning) speed = wallrunspeed;
        else speed = normalrunspeed;

    }

    private void FixedUpdate()
    {
        //Move();
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 2f * Time.deltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 1.5f * Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        Vector3 v3 = head.eulerAngles;
        v3.x -= Input.GetAxis("Mouse Y") * Ysensitive;
        v3.x = LimitXAngle(v3.x, -85f, 85f);
        head.eulerAngles = v3;
    }

    private void Move()
    {
        //float h = Input.GetAxis("Horizontal")*Time.deltaTime * speed;
        //float v = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        //Vector3 vec = new Vector3(h,0,v);
        //rb.MovePosition(vec);
        newvelocity = Vector3.up * rb.velocity.y;
        newvelocity.x = Input.GetAxis("Horizontal") * speed;
        newvelocity.z = Input.GetAxis("Vertical") * speed;
        Jump();
        rb.velocity = transform.TransformDirection(newvelocity);
    }



    void Jump()
    {
        if (isGround && !wallrunning)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                rb.AddForce(transform.up * jumpspeed , ForceMode.VelocityChange);
                isJumping = true;
            }
        }
        //else if (wallrunning)
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        //newvelocity.y = jumpspeed;
        //        rb.AddForce(transform.up * jumpspeed, ForceMode.Impulse);
        //        isJumping = true;
        //    }
    }

    public float LimitXAngle(float angle , float min ,float max)
    {
        if (angle > 180) angle -= 360;
        else if (angle < -180) angle += 360;

        if (angle > max) angle = max;
        if (angle < min) angle = min; 
        return angle;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGround = true;
        isJumping = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }
}
