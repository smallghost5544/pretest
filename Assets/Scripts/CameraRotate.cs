using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Vector3 nextPosition;
    public Quaternion nextRotation;
    [Header("左右靈敏度")]
    public float XrotationPower = 3f;
    [Header("上下靈敏度")]
    public float YrotationPower = 3f;
    public float rotationLerp = 0.5f;

    public float speed = 1f;


    public GameObject followTransform;

    private void Update()
    {

        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        //Rotate the Follow Target transform based on the input
        //攝影機轉動 上下為軸左右轉
        followTransform.transform.rotation *= Quaternion.AngleAxis(h * XrotationPower, Vector3.up);

        //攝影機轉動跟隨 右方為軸 上下轉
        followTransform.transform.rotation *= Quaternion.AngleAxis(-v * YrotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 75)
        {
            angles.x = 75;
        }

        followTransform.transform.localEulerAngles = angles;

        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

}
