using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dashing : MonoBehaviour
{
    public Transform orientation;
    public Transform playercam;
    public PlayerCam cam;
    public float dashFov;
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;

    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYspeed;
    public float dashDuration;

    public float dashCD;
    private float dashCDtimer;

    public bool UsingCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVelocity = true;

    private Vector3 delayForcetoApply;
    public KeyCode dashkey = KeyCode.LeftShift;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(dashkey)) Dash();
        if (dashCDtimer > 0) dashCDtimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCDtimer > 0) return;
        else dashCDtimer = dashCD;
        pm.Dashing = true;
        pm.maxYspeed = maxDashYspeed;
        cam.DoFov(dashFov);

        Transform forwardT;
        if (UsingCameraForward) forwardT = playercam;
        else forwardT = orientation;


        var direction = GetDirection(forwardT);
        var forceApply = direction * dashForce + orientation.up * dashUpwardForce;
        if (disableGravity) rb.useGravity = false;
        delayForcetoApply = forceApply;
        Invoke(nameof(DelayDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayDashForce()
    {
        if (resetVelocity) rb.velocity = Vector3.zero;
        rb.AddForce(delayForcetoApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.Dashing = false;
        pm.maxYspeed = 0;
        cam.DoFov(60);
        if (disableGravity) rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var direction = new Vector3();
        if (allowAllDirections) direction = forwardT.forward * v + forwardT.right * h;
        else direction = forwardT.forward;
        if(v==0 && h==0) direction = forwardT.forward;
        return direction.normalized;
    }
}
