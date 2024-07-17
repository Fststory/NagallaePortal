using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity_Jump : MonoBehaviour
{
    GameObject feet;

    public float gravity = -9.81f; // �߷� ���ӵ�
    public float maxFallSpeed = 20f; // �ִ� ���� �ӵ�
    public float groundCheckDistance = 0.1f; // ���� ���� �Ÿ�
    public float jumpForce = 10f; // ���� ��

    private Vector3 velocity;
    public bool isGrounded = false;

    private void Start()
    {
        feet = GameObject.Find("Feet");
    }

    private void Update()
    {
        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // �ִ� ���� �ӵ� ����
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        // ���� ����
        isGrounded = IsGrounded();

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = 0f;
        }

        // ���� �Է� ó��
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }        

        // ��ü �̵�
        transform.Translate(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        // ���� ���� ���� ����
        // (��: ����ĳ��Ʈ �Ǵ� Ʈ���� �ݶ��̴� ���)
        RaycastHit hit;
        if (Physics.Raycast(feet.transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        // ���� ���� ����
        if (isGrounded)
        {
            velocity.y = jumpForce;
        }
    }
}
