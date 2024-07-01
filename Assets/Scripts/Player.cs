using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player의 움직임에 관한 Script 입니다.
    // ㄴ [ w,a,s,d ], [ space ], [ ctrl ]

    #region PlayerMove()
    // Player의 이동 속도 변수
    public float moveSpeed = 5;
    
    // Rigidbody 선언 및 점프력 변수
    public Rigidbody playerBody;
    public float jumpForce;
    // 발 위치, 바닥 레이어 선언;
    public Transform feetTransform;
    public LayerMask floorMask;
    #endregion

    #region PlayerRotate()
    // 회전 속도 변수
    public float rotSpeed = 200f;
    // 마우스 좌우 이동(y축 회전 값) 변수
    float mx = 0;
    #endregion



    void Update()
    {
        PlayerMove();
        PlayerRotate();
    }



    void PlayerMove()
    {
        // 플레이어의 키 입력에 따라 움직임(전후좌우)을 구현
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(moveX, 0, moveZ);
        dir.Normalize();

        // 메인 카메라를 기준으로 방향을 변환한다. (로컬 방향 벡터)
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        // p = p0 + vt
        transform.position += dir * moveSpeed * Time.deltaTime;

        // [space]를 누르면 점프 할건데..
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 만약 발이 땅에 닿아 있다면..
            if (Physics.CheckSphere(feetTransform.position, 0.5f, floorMask))
            {
                playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

    }

    void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        mx += mouseX * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
