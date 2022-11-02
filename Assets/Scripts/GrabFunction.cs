using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabFunction : MonoBehaviour
{
    public Transform camTransform;
    public Transform grabPoint;
    public LayerMask layer;
    public UsefulObject holdingobj;
    float distance = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   if (holdingobj == null)
            {
                if (Physics.Raycast(camTransform.position, camTransform.forward, out RaycastHit hit, distance, layer))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform.TryGetComponent<UsefulObject>(out holdingobj))
                    {
                        holdingobj.Grab(grabPoint.transform);
                        Debug.Log(holdingobj);
                    }
                }
            }
        else
            {
                holdingobj.Drop();
                holdingobj = null;
            }
        }
    }
}
