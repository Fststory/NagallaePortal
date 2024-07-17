using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XaxisRotate : MonoBehaviour
{
    public float XrotSpeed = 7.0f;

    void Start()
    {
        
    }

    void Update()
    {
        float my = Input.GetAxis("Mouse Y");
        transform.eulerAngles += new Vector3(-my, 0, 0) * XrotSpeed * Time.deltaTime;
    }
}
