using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    // �÷��̾��� ���� ���� üũ(?)�� ������ ������ٵ�� ���õ� ��ũ��Ʈ�Դϴ�.

    Rigidbody rb;   // ������ٵ� ĳ���� ����
    public bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();     // ������ٵ� ĳ��
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}
