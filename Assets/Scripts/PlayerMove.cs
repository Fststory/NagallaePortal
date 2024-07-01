using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Player�� �̵� �ӵ� ����
    public float moveSpeed = 5;

    // �߷� ����
    float gravity = -1f;
    // ���� �ӷ� ����
    float yVelocity = 0;


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

        // ���� ī�޶� �������� ������ ��ȯ�Ѵ�. (���� ���� ����)
        dir = Camera.main.transform.TransformDirection(dir);


        // p = p0 + vt
        transform.position += dir * moveSpeed * Time.deltaTime;

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
    }
}
