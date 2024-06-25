using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5;     // �ӵ��� �ӽ� ��������.

    void Start()
    {
        
    }

    void Update()
    {
        // �÷��̾��� Ű �Է¿� ���� ������(�����¿�)�� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        // p = p0 + vt
        transform.position += dir * moveSpeed * Time.deltaTime;

    }
}
