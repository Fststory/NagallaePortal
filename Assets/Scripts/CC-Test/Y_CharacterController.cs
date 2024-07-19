using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_CharacterController : MonoBehaviour
{
    public float rotSpeed = 200f;

    float mx = 0;

    void Start()
    {
        
    }

    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");

        mx += mouse_X * rotSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
