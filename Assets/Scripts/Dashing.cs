using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dashing : MonoBehaviour
{
    public Transform orientation;
    public Transform playercam;
    public PlayerCam cam;
    [Header("衝刺時FOV")]
    public float dashFov;
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;

    public float dashForce;
    public float dashUpwardForce;
    [Header("限制向上衝刺動力")]
    public float maxDashYspeed;
    [Header("衝刺持續時間")]
    public float dashDuration;
    [Header("冷卻時間")]
    public float dashCD;
    private float dashCDtimer;

    [Header("是否依照鏡頭方位衝刺")]
    public bool UsingCameraForward = true;
    [Header("允許各方向鍵衝刺")]
    public bool allowAllDirections = true;
    [Header("忽略重力")]
    public bool disableGravity = false;
    [Header("衝刺前速率歸0")]
    public bool resetVelocity = true;

    private Vector3 delayForcetoApply;
    public KeyCode dashkey = KeyCode.LeftShift;

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
        SetState();
        cam.DoFov(dashFov);
        SetFinalForce();

        if (disableGravity) rb.useGravity = false;
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

    private void SetState()
    { 
        pm.Dashing = true;
        pm.maxYspeed = maxDashYspeed;
    }

    private void SetFinalForce()
    {
        Transform forwardT;
        if (UsingCameraForward) forwardT = playercam;
        else forwardT = orientation;
        var direction = GetDirection(forwardT);
        delayForcetoApply = direction * dashForce + orientation.up * dashUpwardForce;
    }
}
