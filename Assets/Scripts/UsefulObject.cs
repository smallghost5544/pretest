using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefulObject : MonoBehaviour
{
    Transform GrabPoint;
    Rigidbody rb;
    public float lerpspeed = 0.2f;
    // Start is called before the first frame update

    public void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }
    public void Grab(Transform t)
    {
        GrabPoint = t;
        //rb.useGravity = false;
        //rb.freezeRotation = true;
        DestroyImmediate(rb);
        transform.SetParent(GrabPoint);
        transform.localPosition = Vector3.zero;
    }
    public void Drop()
    {
        GrabPoint = null;
        transform.parent = null;
        rb = gameObject.AddComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //if (GrabPoint != null)
        //{
        //    //var NewPosition = Vector3.Lerp(transform.position , GrabPoint.position , Time.deltaTime * lerpspeed);
        //    //rb.MovePosition(NewPosition);
            
        //}
    }
}
