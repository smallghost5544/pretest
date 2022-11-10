using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerCam : MonoBehaviour
{   
    [Header("左右靈敏度")]
    public float sensX;
    [Header("上下靈敏度")]
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        orientation = GameObject.Find("Orientation").transform;
    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        Debug.Log(yRotation);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        SetYrotationLimit();
    }

    private void SetYrotationLimit()
    {
        if (yRotation > 360) yRotation = 0;
        if (yRotation < 0) yRotation = 360;
    }

    public void walljumpRotate(Vector3 forward , Vector3 wallnormal)
    {
        var rotate = Vector3.Angle(forward, wallnormal);
        Debug.Log(rotate);
        orientation.rotation = Quaternion.Euler(0, rotate, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
    public void DoTiltSlow(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.5f);
    }
}