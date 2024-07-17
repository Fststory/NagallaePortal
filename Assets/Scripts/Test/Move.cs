using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // wasd �Է��� ���� ���� ���͸� ����
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0, z);
        // ���� ���͸� �÷��̾��� ȸ���� �������� ����
        dir = transform.TransformDirection(dir);
        // p = p0 + vt;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
