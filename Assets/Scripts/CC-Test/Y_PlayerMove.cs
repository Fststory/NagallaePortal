using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        dir = Camera.main.transform.TransformDirection(dir);

        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
