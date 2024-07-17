using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity_Jump : MonoBehaviour
{
    GameObject feet;

    public float gravity = -9.81f; // 중력 가속도
    public float maxFallSpeed = 20f; // 최대 낙하 속도
    public float groundCheckDistance = 0.1f; // 지면 감지 거리
    public float jumpForce = 10f; // 점프 힘

    private Vector3 velocity;
    public bool isGrounded = false;

    private void Start()
    {
        feet = GameObject.Find("Feet");
    }

    private void Update()
    {
        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 최대 낙하 속도 제한
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        // 지면 감지
        isGrounded = IsGrounded();

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = 0f;
        }

        // 점프 입력 처리
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }        

        // 물체 이동
        transform.Translate(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        // 지면 감지 로직 구현
        // (예: 레이캐스트 또는 트리거 콜라이더 사용)
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
        // 점프 로직 구현
        if (isGrounded)
        {
            velocity.y = jumpForce;
        }
    }
}
