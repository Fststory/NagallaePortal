using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaxisRotate : MonoBehaviour
{
    public float YrotSpeed = 7.0f;

    void Start()
    {
        
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        transform.eulerAngles += new Vector3(0,mx,0) * YrotSpeed * Time.deltaTime;
    }
}
